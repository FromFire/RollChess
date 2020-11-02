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

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //初始化显示

    public void display(BoardEntity entity) {
        boardEntity = entity;

        //显示TilemapBoard层，地图格子
        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        Tile tile = Resources.Load<Tile>("Tiles/hex-sliced_114");
        foreach(SingleMapGridEntity grid in boardEntity.map) {
            tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tile);
        }

        //显示TileMapToken层，棋子
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        tile = Resources.Load<Tile>("Tiles/hex-sliced_117");
        for(int player = 0; player < boardEntity.players.number; player++) {
            foreach(SingleTokenEntity grid in boardEntity.tokens[player].singleTokens) {
                tilemapBoard.SetTile(new Vector3Int(grid.x, grid.y, 0), tile);
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
