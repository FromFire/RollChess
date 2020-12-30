using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using PlayerChoices = Structure.PlayerChoices;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {
    //棋子数量默认是4
    int PLAYERNUMBER = Structure.Constants.PLAYERNUMBER;

    //显示高光tilemap层
    public HighlightDisplay highlightDisplay;

    //HUD层
    public HUD hud;

    // 存储棋盘信息
    public Board board;
    // 存储棋子信息
    public TokenSet tokenSet;

    // 当前状态
    // waiting: 等待玩家操作
    // moved: 玩家操作完成，等待处理
    public enum Status{waiting, moved};
    public Status status = Status.waiting;

    // 玩家操控状态
    public List<PlayerChoices> playerChoices;
    // 目前正在行动的玩家，他只能移动己方的棋子
    public int nowPlayer;

    // 棋子选中情况
    // 是否已有棋子被选中
    bool isTokenChoosed = false;
    // 当前选中的棋子坐标
    Vector2Int choosedTokenPos;
    // 当前选中棋子可到达的位置，以及通向它的路线
    List<(Vector2Int pos, List<Vector2Int> route)> reachablePos;
    // 当前可走步数
    int step;

    // 初始化全局
    void Start()
    {
        //无Entrance情况下的默认值，用于调试Game场景
        string filename = "Maps/FourPlayers";
        // 默认2玩家，一定不会出错
        playerChoices = new List<PlayerChoices> {PlayerChoices.Player, PlayerChoices.Player, PlayerChoices.Banned, PlayerChoices.Banned};

        //读取Entrance传来的Message
        if(GameObject.Find("MessageToGame") != null) {
            Message message = GameObject.Find("MessageToGame").GetComponent<Message>();
            // 获取地图
            filename = "Maps/" + message.GetMessage<string> ("mapFilename");
            // 获取玩家数量
            playerChoices = message.GetMessage<List<PlayerChoices>> ("playerChoice");
            // 销毁Message，避免它被重复创建
            GameObject.Destroy(message.gameObject);
        }
        
        //读取地图json文件
        BoardEntity boardEntity = LoadMapFromJson(filename);

        //初始化玩家信息
        nowPlayer = 0;

        //初始化board
        board.Init(boardEntity.map, boardEntity.special, boardEntity.portal);

        //初始化tokenSet
        tokenSet.Init(boardEntity.tokens);
        for(int i=0; i<playerChoices.Count; i++) {
            if(playerChoices[i] == PlayerChoices.Banned) {
                tokenSet.removePlayer(i);
            }
        }

        //初始化选中信息
        reachablePos = new List<(Vector2Int pos, List<Vector2Int> route)>();
    }

    // Update is called once per frame
    void Update()
    {
        // 避免与UI按键冲突
        if (Cursor.isOverUI()) {
            return;
        }

        //获取鼠标所在点的点在tilemap上的坐标
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector2Int pos = highlightDisplay.WorldToCell(loc);
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);

        // 若鼠标所点坐标为非法坐标（在地图之外），则不进行操作
        if(!board.IsInBoard(pos)) {
            return;
        }

        // 判断鼠标是否在可走的格子上，若不在，取消路径高亮
        if(pos3 != highlightDisplay.highlightRouteEnd) {
            highlightDisplay.CancelRouteHightlight();
        }
        if(isTokenChoosed) {
            for(int i=0; i<reachablePos.Count; i++) {
                Vector2Int grid = reachablePos[i].pos;
                List<Vector2Int> route = reachablePos[i].route;
                if(grid == pos) {
                    highlightDisplay.HighlightRoute(route);
                }
            }
        }
    }

    //查询是否有获胜的玩家，若没有，返回-1
    //若只有一个玩家棋子数不为0，则该玩家获胜
    public int FindWinner() {
        int winnerCandidate = -1; //可能赢（棋子数）的玩家序号
        for(int i=0; i<PLAYERNUMBER; i++) {
            if(tokenSet.GetTokenNumber(i) != 0) {
                //若已查询的所有玩家棋子数均为0，此玩家可能会赢
                if(winnerCandidate == -1) {
                    winnerCandidate = i;
                } 
                //若在此人之前有玩家棋子数不为0，则无人获胜
                else {
                    return -1;
                }
            }
        }
        return winnerCandidate;
    }

    //掷骰子
    //是RollButton的OnClick函数
    public void RollDice() {
        //生成随机数
        step = new System.Random().Next(6)+1;
        Debug.Log(step);

        //隐藏按钮
        hud.ShowRollStep(step);
    }
    
    // TODO: 回调RollDice给HUD


    //移动棋子
    public void Move(Vector2Int from, Vector2Int to, List<Vector2Int> route) {
        Debug.Log("move: ("+ from.x + "." + from.y + ") -> (" + to.x + "." + to.y + ") ");

        //若目的点是传送门，将其传送
        //传送门须放在走子前面，否则会有BUG：B站在传送站处，A踩到传送站，传送前会吃掉B
        if(board.GetEffect(to) == SingleGrid.Effect.Portal) {
            Vector2Int target = board.GetPortalTarget(to);
            tokenSet.MoveToken(to, target);
        }

        //由tokenSet进行操作
        tokenSet.MoveToken(from, to);

        //检测危桥
        board.DetectBrokenBridge(route);

        //修改状态为moved
        status = Status.moved;

        //显示roll点按钮，隐藏步数按钮
        hud.ShowRollButton();
    }

    // 选中格子
    public void ChooseGrid(Vector3 loc) {
        // 1. 判定是否是走子（已有棋子被选中，且此次点击的是可到达的格子）
        //      是：移动棋子，return
        //      否：清空棋子选中状态
        // 2. 判定是否可走的格子
        //      是：选中该格子（将其高亮，取消先前的高亮）
        //      否：取消选中（取消先前的高亮）
        // 3. 判定该格是否有己方棋子
        //      是：预览可走位置（高亮它可以到达的所有格子）

        //获取点击的点在tilemap上的坐标
        //若鼠标所点坐标为非法坐标（在地图之外），则不进行操作
        Vector2Int pos = highlightDisplay.WorldToCell(loc);  // TODO: 用TilemapDisplay来干这件事
        if(!board.IsInBoard(pos)) {
            return;
        }
        Debug.Log("choose: ("+ pos.x + "." + pos.y + ")");

        //检测是否是走子
        if(isTokenChoosed) {
            for(int i=0; i<reachablePos.Count; i++) {
                Vector2Int grid = reachablePos[i].pos;
                List<Vector2Int> route = reachablePos[i].route;
                if(grid == pos) {
                    //显示走子效果
                    Move(choosedTokenPos, pos, route);

                    //取消所有选中
                    ClearChoose();
                    return;
                }
            }
        }

        //取消所有选中
        ClearChoose();

        //显示选中效果
        if(board.IsWalkable(pos)) {
            highlightDisplay.HighlightGrid(pos, HighlightDisplay.Color.blue);
        }

        //检测格子上是否有己方棋子，有则选中它
        List<Token> tokens = tokenSet.Find(pos);
        if(tokens.Count != 0 && tokens[0].player == nowPlayer) {
            AddChoose(pos);
        }
    }
    
    // TODO: 统一命名 ChooseGrid{ ChooseTokenSource(AddChoose), ChooseTokenTarget(Move) }

    // 选中棋子，并显示它能到达的所有位置
    void AddChoose(Vector2Int pos) {
        //获取该棋子它所有可达位置
        List<(Vector2Int now, List<Vector2Int> pre)> reachableGrids = board.GetReachableGrids(pos, step);

        //维护选中信息
        isTokenChoosed = true;
        choosedTokenPos = pos;
        this.reachablePos = reachableGrids;

        //显示高亮
        for(int i=0; i<reachablePos.Count; i++) {
            Vector2Int grid = reachablePos[i].pos;
            highlightDisplay.HighlightGrid(grid, HighlightDisplay.Color.yellow);
        }
    }

    // 取消选中棋子，显示效果
    void ClearChoose() {
        //维护选中信息
        isTokenChoosed = false;
        reachablePos.Clear();

        //取消所有高光
        highlightDisplay.CancelHighlight();
    }

    //从json文件中读取地图
    // TODO: 提取到Utility里边
    public BoardEntity LoadMapFromJson(string filename) {
        Debug.Log("加载地图：" + filename);

        //读取json字符串
        TextAsset text = Resources.Load<TextAsset>(filename);
        string json = text.text;
        //将json字符串转换为BoardEntity类
        BoardEntity boardEntity = BoardEntity.FromJson(json);
        //boardEntity.toConsole();
        return boardEntity;
    }

}
