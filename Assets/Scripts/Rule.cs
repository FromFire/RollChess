using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    //存储棋盘初始化信息的实体，也就是从json中读取的信息
    BoardEntity boardEntity;

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
        loadMapFromJson(filename);

        // //初始化board
         board = GameObject.Find("/Board").GetComponent<Board>();
         board.init(boardEntity.map);

        //初始化tokens
        tokens = new List<List<Token>>(boardEntity.players.number);
        for(int i=0; i<tokens.ToArray().Length; i++) {
            tokens[i] = new List<Token>(boardEntity.tokens[i].number);
            for(int j=0; j<tokens[i].ToArray().Length; j++) {
                SingleTokenEntity tmpTokenEntity = boardEntity.tokens[i].singleTokens[j];
                tokens[i][j] = new Token(tmpTokenEntity.x, tmpTokenEntity.y);
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


    //从json文件中读取地图
    void loadMapFromJson(string filename) {
        Debug.Log("从json中加载地图;" + filename);

        //读取文件
        string json = "";
        TextAsset text = Resources.Load<TextAsset>(filename);
        json = text.text;
        Debug.Log(json);
        Debug.Assert(!string.IsNullOrEmpty(json));

        //转换json
        boardEntity = JsonUtility.FromJson<BoardEntity>(json);

        //控制台输出boardEntity
        //boardEntity.toConsole();
    }

}
