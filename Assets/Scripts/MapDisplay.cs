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
        //不同阵营棋子外观不同
        List<Tile> tokenTiles = new List<Tile>();
        tokenTiles.Add(Resources.Load<Tile>("Tiles/hex-sliced_140"));
        tokenTiles.Add(Resources.Load<Tile>("Tiles/hex-sliced_145"));
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

    // 移动棋子
    public void moveToken(Vector3Int from, Vector3Int to) {
        //暂时不考虑某个格有多个棋子的情况
        Tile tile = (Tile)tilemapToken.GetTile(from);
        tilemapToken.SetTile(from, null);
        tilemapToken.SetTile(to, tile);
    }
}
