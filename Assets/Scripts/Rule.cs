using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    SpecialEffectDisplay specialEffectDisplay;

    // 存储棋盘信息
    Board board;
    // 存储棋子信息
    TokenSet tokenSet;

    // 当前状态
    // waiting: 等待玩家操作
    // moved: 玩家操作完成，等待处理
    public enum Status{waiting, moved};
    public Status status = Status.waiting;

    // 目前正在行动的玩家，他只能移动己方的棋子
    public int nowPlayer = 0;

    // 棋子选中情况
    // 是否已有棋子被选中
    bool isTokenChoosed = false;
    // 当前选中的棋子坐标
    Vector2Int choosedTokenPos;
    // 当前选中棋子可到达的位置
    List<Vector2Int> reachablePos;

    // 初始化全局
    void Start()
    {
        //读取地图json文件
        string filename = "MapSample";
        BoardEntity boardEntity = loadMapFromJson(filename);

        //初始化board
        board = GameObject.Find("/Board").GetComponent<Board>();
        board.init(boardEntity.map);

        //初始化tokenSet
        tokenSet = GameObject.Find("/TokenSet").GetComponent<TokenSet>();
        tokenSet.init(boardEntity.tokens);

        //初始化选中信息
        reachablePos = new List<Vector2Int>();

        //初始化SpecialEffectDisplay
        specialEffectDisplay = GameObject.Find("/Grid/TilemapSpecialEffect").GetComponent<SpecialEffectDisplay>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //移动棋子
    public void move(Vector2Int from, Vector2Int to) {
        Debug.Log("move: ("+ from.x + "." + from.y + ") -> (" + to.x + "." + to.y + ") ");

        //由tokenSet进行操作
        tokenSet.moveToken(from, to);

        //修改状态为moved
        status = Status.moved;
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
        Vector2Int pos = specialEffectDisplay.worldToCell(loc);

        //检测是否是走子
        if(isTokenChoosed) {
            foreach(Vector2Int grid in reachablePos) {
                if(grid == pos) {
                    //显示走子效果
                    move(choosedTokenPos, pos);

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
        //获取该棋子它所有可达位置（目前只能显示2步）
        List<Vector2Int> reachableGrids = board.getReachableGrids(pos, 2);

        //维护选中信息
        isTokenChoosed = true;
        choosedTokenPos = pos;
        this.reachablePos = reachableGrids;

        //显示高亮
        foreach(Vector2Int grid in reachableGrids) {
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
        Debug.Log("从json中加载地图;" + filename);

        //读取json字符串
        string json = "";
        TextAsset text = Resources.Load<TextAsset>(filename);
        json = text.text;
        Debug.Log(json);
        Debug.Assert(!string.IsNullOrEmpty(json));

        //将json字符串转换为BoardEntity类
        BoardEntity boardEntity = JsonUtility.FromJson<BoardEntity>(json);
        //boardEntity.toConsole();
        return boardEntity;
    }

}
