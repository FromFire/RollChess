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
    enum TileKeys{floorLawnGreen};

    // Start is called before the first frame update
    void Start()
    {
        // tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/floor-lawnGreen"    //floorLawnGreen
        };

        // 读取所有tile
        tileList = new List<Tile> ();
        foreach(string name in tileNames) {
            tileList.Add(Resources.Load<Tile>(name));
        }
    }

    public void display(List<SingleMapGridEntity> entity) {
        //显示TilemapBoard层，地图格子
        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        foreach(SingleMapGridEntity grid in entity) {
            tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tileList[(int)TileKeys.floorLawnGreen]);
        }
    }

    public void removeGrid(Vector2Int pos) {
        tilemapBoard.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
