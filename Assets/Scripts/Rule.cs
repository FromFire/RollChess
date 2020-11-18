using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    // 负责棋盘的显示
    MapDisplay mapDisplay;

    // 存储棋盘信息
    Board board;
    // 存储棋子信息[玩家][棋子]
    List<List<Token>> tokens;

    // 当前状态
    // waiting: 等待玩家操作
    // moved: 玩家操作完成，等待处理
    enum Status{waiting, moved};
    Status status = Status.waiting;

    // 棋子选中情况
    // 是否已有棋子被选中
    bool isTokenChoosed = false;
    // 当前选中的棋子坐标
    Vector2Int choosedTokenPos;
    // 当前选中棋子可到达的位置
    List<Vector2Int> reachablePos;

    // 初始化棋盘
    void Start()
    {
        //读取地图json文件
        string filename = "MapSample";
        BoardEntity boardEntity = loadMapFromJson(filename);

        //初始化board
        board = GameObject.Find("/Board").GetComponent<Board>();
        board.init(boardEntity.map);

        //初始化tokens
        tokens = new List<List<Token>>();
        for(int i=0; i<boardEntity.players.number; i++) {
            tokens.Add(new List<Token>());
            for(int j=0; j<boardEntity.tokens[i].number; j++) {
                SingleTokenEntity tmpTokenEntity = boardEntity.tokens[i].singleTokens[j];
                tokens[i].Add(new Token(tmpTokenEntity.x, tmpTokenEntity.y));
            }
        }

        //初始化选中信息
        reachablePos = new List<Vector2Int>();

        //显示游戏初始状态
        mapDisplay = GameObject.Find("/Grid").GetComponent<MapDisplay>();
        mapDisplay.display(boardEntity);
    }

    // Update is called once per frame
    void Update()
    {
        // //玩家走子结束，开始处理结果
        // if(status == Status.moved) {

        // }
        if(status == Status.waiting) {
            //move(new Vector2Int(0, 1), new Vector2Int(1, 0));
            status = Status.moved;
        }
            
    
    }

    //移动棋子
    public void move(Vector2Int from, Vector2Int to) {
        //查找棋子
        Debug.Log("from: ("+ from.x + "." + from.y + ") ");
        foreach(List<Token> tokenlist in tokens) {
            foreach(Token token in tokenlist) {
                Debug.Log("("+ token.getXY().x + "." + token.getXY().y + ") ");
                if(token.getXY() == from) {
                    token.setXY(to);
                    break;
                }
            }
        }
        
        //显示
        mapDisplay.moveToken(from, to);
    }

    // 选中格子
    //
    // 1. 判定是否是走子（已有棋子被选中，且此次点击的是可到达的格子）
    //      是：移动棋子，return
    //      否：清空棋子选中状态
    // 2. 判定是否可走的格子
    //      是：选中该格子（将其高亮，取消先前的高亮）
    //      否：取消选中（取消先前的高亮）
    // 3. 判定该格是否有棋子
    //      是：预览可走位置（高亮它可以到达的所有格子）

    public void chooseGrid(Vector3 loc) {
        Vector2Int pos = mapDisplay.worldToCell(loc);

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
            mapDisplay.highlightGrid(pos, MapDisplay.Color.blue);
        }

        //检测格子上是否有棋子，有则选中它
        for(int player=0; player<tokens.Count; player++) {
            for(int i=0; i<tokens[player].ToArray().Length; i++) {
                if(tokens[player][i].getXY() == pos) {
                    AddChoose(pos);
                    break;
                }
            }
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
            mapDisplay.highlightGrid(grid, MapDisplay.Color.yellow);
        }
    }

    // 取消选中棋子，显示效果
    void ClearChoose() {
        //维护选中信息
        isTokenChoosed = false;
        reachablePos.Clear();

        //取消所有高光
        mapDisplay.cancelHighlight();
    }

    //从json文件中读取地图
    BoardEntity loadMapFromJson(string filename) {
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
