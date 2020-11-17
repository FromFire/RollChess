using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    //是否已有格子正在高亮
    bool isHighlighted = false;
    //当前高亮的格子坐标
    Vector2Int highlightPosition;

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
            List<Vector2Int> reachableGrids = getReachableGrids(pos, 2);
            //List<Vector2Int> reachableGrids = getNeighbors(pos);
            foreach(Vector2Int grid in reachableGrids) {
                mapDisplay.highlightGrid(grid, MapDisplay.Color.yellow);
            }
        }
    }

    // 显示所有从pos出发前进step步可到达的格子
    public List<Vector2Int> getReachableGrids(Vector2Int pos, int step) {
        List<Vector2Int> ret = new List<Vector2Int>();

        //需要的信息：当前格子的坐标、上一步的坐标，已走步数
        Queue<(Vector2Int now, Vector2Int pre, int step)> queue = new Queue<(Vector2Int now, Vector2Int pre, int step)>();

        //BFS
        queue.Enqueue( (pos, pos, 0) );
        while(queue.Count != 0) {
            var thisTuple = queue.Dequeue();

            //若step足够，不进行操作，直接加入返回列表
            if(thisTuple.step == step) {
                ret.Add(thisTuple.now);
                continue;
            }

            //得到所有合法的下一步
            //如果只有一个合法的下一步，说明now在端点上，不考虑它的上一步是否和下一步重合，直接入队
            List<Vector2Int> nextGrids = getNeighbors(thisTuple.now);
            if(nextGrids.Count == 1) {
                queue.Enqueue( (nextGrids[0], thisTuple.now, thisTuple.step+1) );
                continue;
            }

            //将所有不和上一步重合的下一步入队
            foreach(Vector2Int next in nextGrids) {
                if(next != thisTuple.pre) {
                    queue.Enqueue( (next, thisTuple.now, thisTuple.step+1) );
                }
            }

        }

        //返回列表去重
        ret = ret.Distinct().ToList();

        return ret;
    }

    //获取所有与该格子相邻的可走格子
    public List<Vector2Int> getNeighbors(Vector2Int pos) {
        List<Vector2Int> ret = new List<Vector2Int>();

        //无论奇偶，上下左右都可达
        List<Vector2Int> offsets = new List<Vector2Int> {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };
        //偶数行：斜向左可达(-1,+1)和(-1,-1)
        if(pos.y % 2 == 0) {
            offsets.Add(new Vector2Int(-1,1));
            offsets.Add(new Vector2Int(-1,-1));
        }
        //奇数行：斜向右可达(+1,+1)和(+1,-1)
        else {
            offsets.Add(new Vector2Int(1,1));
            offsets.Add(new Vector2Int(1,-1));
        }

        //筛选出可到达的格子
        foreach(Vector2Int offset in offsets) {
            if(board.isWalkable(pos+offset)) {
                ret.Add(pos+offset);
            }
        }

        return ret;
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
