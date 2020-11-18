using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 控制棋局的显示，包括地图、棋子、特效等等，不包括UI
public class MapDisplay : MonoBehaviour
{
    //Tilemap地图层
    Tilemap tilemapBoard;
    //Tilemap棋子层
    Tilemap tilemapToken;


    //存储各类Tile的集合
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{floorLawnGreen, //普通地板
    floorSteelBlue, floorYellow,  //高亮色
    tokenRedTank, tokenBlueTank}; //棋子

    //供外界传入的颜色参数类
    public enum Color{blue, yellow};


    //正在高亮的格子坐标
    List<Vector3Int> highlightGridPosition;
    //正在高亮的格子Tile
    List<Tile> highlightGridTile;

    //记录每个坐标上有几个棋子
    Dictionary<Vector2Int, int> pileInfo;

    // Start is called before the first frame update
    void Start()
    {
        // tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/floor-lawnGreen",    //floorLawnGreen
            "Tiles/floor-steelBlue",    //floorSteelBlue
            "Tiles/floor-yellow",       //floorYellow

            "Tiles/token-redTank",      //tokenRedTank
            "Tiles/token-blueTank"      //tokenBlueTank
        };

        // 读取所有tile
        tileList = new List<Tile> ();
        foreach(string name in tileNames) {
            tileList.Add(Resources.Load<Tile>(name));
        }

        //初始化空的高亮列表
        highlightGridPosition = new List<Vector3Int>();
        highlightGridTile = new List<Tile>();

        //初始化叠子信息
        pileInfo = new Dictionary<Vector2Int, int>();
    }


    //初始化显示
    public void display(BoardEntity entity) {
        //显示TilemapBoard层，地图格子
        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        foreach(SingleMapGridEntity grid in entity.map) {
            tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tileList[(int)TileKeys.floorLawnGreen]);
        }

        //显示TileMapToken层，棋子
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        //不同阵营棋子外观不同
        List<Tile> tokenTiles = new List<Tile>();
        tokenTiles.Add(tileList[(int)TileKeys.tokenRedTank]);
        tokenTiles.Add(tileList[(int)TileKeys.tokenBlueTank]);

        //分阵营显示棋子
        foreach(TokenEntity token in entity.tokens) {
            tilemapToken.SetTile(new Vector3Int(token.x, token.y, 0), tokenTiles[token.player]);
            //维护pileInfo
            Vector2Int key = new Vector2Int(token.x, token.y);
            pileInfo[key] = pileInfo.ContainsKey(key)? pileInfo[key]+1 : 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 高亮格子
    public void highlightGrid(Vector2Int pos, Color color) {
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);

        //记录高亮的位置的原本Tile
        //如果该格子已被高亮，则不记录它
        if(!highlightGridPosition.Contains(pos3)) {
            highlightGridPosition.Add(pos3);
            highlightGridTile.Add((Tile)tilemapBoard.GetTile(pos3));
        }
        
        //将格子显示为指定的高亮色
        Tile highlightTile = tileList[(int)TileKeys.floorSteelBlue];
        switch(color) {
            case Color.blue:
                highlightTile = tileList[(int)TileKeys.floorSteelBlue];
                break;
            case Color.yellow:
                highlightTile = tileList[(int)TileKeys.floorYellow];
                break;
        }
        tilemapBoard.SetTile(pos3, highlightTile);
    }

    //取消所有高亮
    public void cancelHighlight() {
        for(int i=0; i<highlightGridPosition.ToArray().Length; i++) {
            tilemapBoard.SetTile(highlightGridPosition[i], highlightGridTile[i]);
        }
        highlightGridPosition.Clear();
        highlightGridTile.Clear();
    }

    // 移动棋子
    public void moveToken(Vector2Int from, Vector2Int to) {
        //转换格式
        Vector3Int from3 = new Vector3Int(from.x, from.y, 0);
        Vector3Int to3 = new Vector3Int(to.x, to.y, 0);
        //要移动的棋子的Tile
        Tile tile = (Tile)tilemapToken.GetTile(from3);

        //更新pileInfo
        if(pileInfo[from] == 1) {
            pileInfo.Remove(from);
        } else {
            pileInfo[from] = pileInfo[from]-1;
        }
        pileInfo[to] = pileInfo.ContainsKey(to)? pileInfo[to]+1 : 1;

        //如果from上原本只有一个子（即要移走的子），则将原本的tile清空
        if(!pileInfo.ContainsKey(from)) {
            tilemapToken.SetTile(from3, null);
        }

        //在to上显示tile
        tilemapToken.SetTile(to3, tile);
    }

    // 输入坐标，输出格子的坐标
    public Vector2Int worldToCell(Vector3 loc) {
        Vector3Int vector = tilemapBoard.WorldToCell(loc);
        return new Vector2Int(vector.x, vector.y);
    }
}
