using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    //负责棋盘的显示
    MapDisplay mapDisplay;

    //存储棋盘信息
    private Board board;
    //存储棋子信息[玩家][棋子]
    private List<List<Token>> tokens;

    // 当前状态
    // waiting: 等待玩家操作
    // moved: 玩家操作完成，等待处理
    enum Status{waiting, moved};
    Status status = Status.waiting;

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
        foreach(List<Token> tokenlist in tokens) {
            foreach(Token token in tokenlist) {
                if(token.getXY() == from) {
                    token.setXY(to);
                    break;
                }
            }
        }
        
        //显示
        mapDisplay.moveToken(new Vector3Int(from.x, from.y, 0), new Vector3Int(to.x, to.y, 0));
    }

    // 选中格子
    // 如果是可走的格子，将其高亮，同时将当前已高亮的格子取消
    // 如果是不可走的格子，则只取消原本高亮的格子
    public void chooseGrid(Vector3 loc) {
        Vector2Int pos = mapDisplay.worldToCell(loc);

        //显示选中效果
        mapDisplay.cancelHighlight();
        if(board.isWalkable(pos)) {
            mapDisplay.highlightGrid(pos, MapDisplay.Color.blue);
        }

        //检测格子上是否有棋子
        bool hasToken = false;
        for(int player=0; player<tokens.Count; player++) {
            for(int i=0; i<tokens[player].ToArray().Length; i++) {
                if(tokens[player][i].getXY() == pos) {
                    hasToken = true;
                    break;
                }
            }
        }

        //如果格子上有棋子，显示它所有可达位置（目前显示3步的情况）
        if(hasToken) {
            Debug.Log("yes");
            List<Vector2Int> reachableGrids = board.getReachableGrids(pos, 2);
            //List<Vector2Int> reachableGrids = getNeighbors(pos);
            foreach(Vector2Int grid in reachableGrids) {
                mapDisplay.highlightGrid(grid, MapDisplay.Color.yellow);
            }
        }
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
