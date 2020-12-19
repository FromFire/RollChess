using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TokensDisplay : MonoBehaviour
{
    //Tilemap棋子层
    Tilemap tilemapToken;

    //存储各类Tile的集合
    //player的值对应tileList的下标
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{
    tokenRedTank, tokenBlueTank}; //棋子

    // Start is called before the first frame update
    void Start()
    {
        // tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/token-redTank",      //tokenRedTank
            "Tiles/token-blueTank"      //tokenBlueTank
        };

        // 读取所有tile
        tileList = new List<Tile> ();
        foreach(string name in tileNames) {
            tileList.Add(Resources.Load<Tile>(name));
        }
    }


    //初始化显示
    public void Display(List<TokenEntity> entity) {
    
        //显示TileMapToken层，棋子
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        //不同阵营棋子外观不同
        List<Tile> tokenTiles = new List<Tile>();
        tokenTiles.Add(tileList[(int)TileKeys.tokenRedTank]);
        tokenTiles.Add(tileList[(int)TileKeys.tokenBlueTank]);

        //分阵营显示棋子
        foreach(TokenEntity token in entity) {
            tilemapToken.SetTile(new Vector3Int(token.x, token.y, 0), tokenTiles[token.player]);
        }
    }

    //在pos处显示number个player方的棋子
    //认定player为TileList的下标
    public void ShowToken(Vector2Int pos, int number, int player) {
        //转换格式
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);
        //根据player获取棋子外观
        Tile tile = tileList[player];

        //显示（目前不显示number）
        if(number==0) {
            tilemapToken.SetTile(pos3, null);
        } else {
            tilemapToken.SetTile(pos3, tile);
        }
    }
    
    // TODO: 删除，放置

    // Update is called once per frame
    void Update()
    {
        
    }
}
