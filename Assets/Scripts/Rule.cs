using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    //显示高光tilemap层
    SpecialEffectDisplay specialEffectDisplay;

    //HUD层
    HUD hud;

    // 存储棋盘信息
    Board board;
    // 存储棋子信息
    TokenSet tokenSet;

    // 当前状态
    // waiting: 等待玩家操作
    // moved: 玩家操作完成，等待处理
    public enum Status{waiting, moved};
    public Status status = Status.waiting;

    // 玩家总数
    public int totalPlayer;
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
        //读取地图json文件
        string filename = "MapSample";
        BoardEntity boardEntity = loadMapFromJson(filename);

        //初始化玩家信息
        totalPlayer = boardEntity.players.number;
        nowPlayer = 0;

        //初始化board
        board = GameObject.Find("/ScriptObjects/Board").GetComponent<Board>();
        board.init(boardEntity.map, boardEntity.special, boardEntity.portal);

        //初始化tokenSet
        tokenSet = GameObject.Find("/ScriptObjects/TokenSet").GetComponent<TokenSet>();
        tokenSet.init(boardEntity.tokens);

        //初始化选中信息
        reachablePos = new List<(Vector2Int pos, List<Vector2Int> route)>();

        //初始化SpecialEffectDisplay
        specialEffectDisplay = GameObject.Find("/Grid/TilemapReachableHighlight").GetComponent<SpecialEffectDisplay>();

        //初始化HUD
        hud = GameObject.Find("/HUD").GetComponent<HUD> ();
        hud.setRule(this);
    }

    // Update is called once per frame
    void Update()
    {
        //获取鼠标所在点的点在tilemap上的坐标
        Vector3 loc = Input.mousePosition;
        Vector2Int pos = specialEffectDisplay.worldToCell(loc);
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);

        //若鼠标所点坐标为非法坐标（在地图之外），则不进行操作
        if(!board.isInBoard(pos)) {
            return;
        }

        //判断鼠标是否在可走的格子上，若不在，取消路径高亮
        if(pos3 != specialEffectDisplay.highlightRouteEnd) {
            specialEffectDisplay.CancelRouteHightlight();
        }
        if(isTokenChoosed) {
            for(int i=0; i<reachablePos.Count; i++) {
                Vector2Int grid = reachablePos[i].pos;
                List<Vector2Int> route = reachablePos[i].route;
                if(grid == pos) {
                    specialEffectDisplay.HighlightRoute(route);
                }
            }
        }
    }

    //查询是否有获胜的玩家，若没有，返回-1
    //若只有一个玩家棋子数不为0，则该玩家获胜
    public int FindWinner() {
        int winnerCandidate = -1; //可能赢（棋子数）的玩家序号
        for(int i=0; i<totalPlayer; i++) {
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
    public void rollDice() {
        //生成随机数
        step = new System.Random().Next(6)+1;
        Debug.Log(step);

        //隐藏按钮
        hud.showRollStep(step);
    }


    //移动棋子
    public void move(Vector2Int from, Vector2Int to, List<Vector2Int> route) {
        Debug.Log("move: ("+ from.x + "." + from.y + ") -> (" + to.x + "." + to.y + ") ");

        //由tokenSet进行操作
        tokenSet.moveToken(from, to);

        //检测危桥
        board.detectBrokenBridge(route);

        //若目的点是传送门，将其传送
        if(board.GetEffect(to) == SingleGrid.Effect.portal) {
            Vector2Int target = board.GetPortalTarget(to);
            tokenSet.moveToken(to, target);
        }

        //修改状态为moved
        status = Status.moved;

        //显示roll点按钮，隐藏步数按钮
        hud.showRollButton();
    }

    // 选中格子
    //
    // 1. 判定是否是走子（已有棋子被选中，且此次点击的是可到达的格子）
    //      是：移动棋子，return
    //      否：清空棋子选中状态
    // 2. 判定是否可走的格子
    //      是：选中该格子（将其高亮，取消先前的高亮）
    //      否：取消选中（取消先前的高亮）
    // 3. 判定该格是否有己方棋子
    //      是：预览可走位置（高亮它可以到达的所有格子）

    public void chooseGrid(Vector3 loc) {
        //获取点击的点在tilemap上的坐标
        //若鼠标所点坐标为非法坐标（在地图之外），则不进行操作
        Vector2Int pos = specialEffectDisplay.worldToCell(loc);
        if(!board.isInBoard(pos)) {
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
                    move(choosedTokenPos, pos, route);

                    //取消所有选中
                    ClearChoose();
                    return;
                }
            }
        }

        //取消所有选中
        ClearChoose();

        //显示选中效果
        if(board.isWalkable(pos)) {
            specialEffectDisplay.highlightGrid(pos, SpecialEffectDisplay.Color.blue);
        }

        //检测格子上是否有己方棋子，有则选中它
        List<Token> tokens = tokenSet.find(pos);
        if(tokens.Count != 0 && tokens[0].player == nowPlayer) {
            AddChoose(pos);
        }
    }

    // 选中棋子，并显示它能到达的所有位置
    void AddChoose(Vector2Int pos) {
        //获取该棋子它所有可达位置
        List<(Vector2Int now, List<Vector2Int> pre)> reachableGrids = board.getReachableGrids(pos, step);

        //维护选中信息
        isTokenChoosed = true;
        choosedTokenPos = pos;
        this.reachablePos = reachableGrids;

        //显示高亮
        for(int i=0; i<reachablePos.Count; i++) {
            Vector2Int grid = reachablePos[i].pos;
            specialEffectDisplay.highlightGrid(grid, SpecialEffectDisplay.Color.yellow);
        }
    }

    // 取消选中棋子，显示效果
    void ClearChoose() {
        //维护选中信息
        isTokenChoosed = false;
        reachablePos.Clear();

        //取消所有高光
        specialEffectDisplay.cancelHighlight();
    }

    //从json文件中读取地图
    public BoardEntity loadMapFromJson(string filename) {
        Debug.Log("加载地图：" + filename);

        //读取json字符串
        string json = "";
        TextAsset text = Resources.Load<TextAsset>(filename);
        json = text.text;
        Debug.Assert(!string.IsNullOrEmpty(json));

        //将json字符串转换为BoardEntity类
        BoardEntity boardEntity = JsonUtility.FromJson<BoardEntity>(json);
        //boardEntity.toConsole();
        return boardEntity;
    }

}
