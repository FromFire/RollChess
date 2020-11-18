using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 控制棋局的显示，包括地图、棋子、特效等等，不包括UI
public class MapDisplay : MonoBehaviour
{
    //存储棋盘信息的实体
    BoardEntity boardEntity;

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
    }


    //初始化显示

    public void display(BoardEntity entity) {
        boardEntity = entity;

        //显示TilemapBoard层，地图格子
        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        foreach(SingleMapGridEntity grid in boardEntity.map) {
            tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tileList[(int)TileKeys.floorLawnGreen]);
        }

        //显示TileMapToken层，棋子
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        //不同阵营棋子外观不同
        List<Tile> tokenTiles = new List<Tile>();
        tokenTiles.Add(tileList[(int)TileKeys.tokenRedTank]);
        tokenTiles.Add(tileList[(int)TileKeys.tokenBlueTank]);
        //分阵营显示棋子
        for(int player = 0; player < boardEntity.players.number; player++) {
            foreach(SingleTokenEntity grid in boardEntity.tokens[player].singleTokens) {
                tilemapToken.SetTile(new Vector3Int(grid.x, grid.y, 0), tokenTiles[player]);
            }
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
        Vector3Int from3 = new Vector3Int(from.x, from.y, 0);
        Vector3Int to3 = new Vector3Int(to.x, to.y, 0);

        //暂时不考虑某个格有多个棋子的情况
        Tile tile = (Tile)tilemapToken.GetTile(from3);
        tilemapToken.SetTile(from3, null);
        tilemapToken.SetTile(to3, tile);
        Debug.Log("move: ("+ from.x + "." + from.y + ") -> (" + to.x + "." + to.y + ") ");
    }

    // 输入坐标，输出格子的坐标
    public Vector2Int worldToCell(Vector3 loc) {
        Vector3Int vector = tilemapBoard.WorldToCell(loc);
        return new Vector2Int(vector.x, vector.y);
    }
}
