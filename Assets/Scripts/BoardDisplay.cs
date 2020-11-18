using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardDisplay : MonoBehaviour
{
    //Tilemap
    Tilemap tilemapBoard;

    //存储各类Tile的集合
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{floorLawnGreen, //普通地板
    floorSteelBlue, floorYellow}; //高亮色

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
            "Tiles/floor-yellow"        //floorYellow
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

    public void display(BoardEntity entity) {
        //显示TilemapBoard层，地图格子
        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        foreach(SingleMapGridEntity grid in entity.map) {
            tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tileList[(int)TileKeys.floorLawnGreen]);
        }
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

    // 输入坐标，输出格子的坐标
    public Vector2Int worldToCell(Vector3 loc) {
        Vector3Int vector = tilemapBoard.WorldToCell(loc);
        return new Vector2Int(vector.x, vector.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
