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
    enum tileKeys{greenFloor, blueFloor, redTank, blueTank};

    //正在高亮的格子坐标
    List<Vector3Int> highlightGridPosition;

    //正在高亮的格子Tile
    List<Tile> highlightGridTile;

    // Start is called before the first frame update
    void Start()
    {
        // tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/hex-sliced_114", //greenFloor
            "Tiles/hex-sliced_111", //blueFloor

            "Tiles/hex-sliced_145", //redTank
            "Tiles/hex-sliced_140"  //blueTank
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
            tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tileList[(int)tileKeys.greenFloor]);
        }

        //显示TileMapToken层，棋子
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        //不同阵营棋子外观不同
        List<Tile> tokenTiles = new List<Tile>();
        tokenTiles.Add(tileList[(int)tileKeys.redTank]);
        tokenTiles.Add(tileList[(int)tileKeys.blueTank]);
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
    public void highlightGrid(Vector2Int pos) {
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);

        highlightGridPosition.Add(pos3);
        highlightGridTile.Add((Tile)tilemapBoard.GetTile(pos3));

        tilemapBoard.SetTile(pos3, tileList[(int)tileKeys.blueFloor]);
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
    public void moveToken(Vector3Int from, Vector3Int to) {
        //暂时不考虑某个格有多个棋子的情况
        Tile tile = (Tile)tilemapToken.GetTile(from);
        tilemapToken.SetTile(from, null);
        tilemapToken.SetTile(to, tile);
    }

    // 输入坐标，输出格子的坐标
    public Vector2Int worldToCell(Vector3 loc) {
        Vector3Int vector = tilemapBoard.WorldToCell(loc);
        return new Vector2Int(vector.x, vector.y);
    }
}
