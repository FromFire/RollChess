using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    BoardEntity boardEntity;
    Tilemap tilemapBoard;
    Tilemap tilemapToken;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
