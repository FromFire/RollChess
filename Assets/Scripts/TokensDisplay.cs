using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TokensDisplay : MonoBehaviour
{
    //Tilemap棋子层
    Tilemap tilemapToken;


    //存储各类Tile的集合
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{
    tokenRedTank, tokenBlueTank}; //棋子

    //记录每个坐标上有几个棋子
    Dictionary<Vector2Int, int> pileInfo;

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

        //初始化叠子信息
        pileInfo = new Dictionary<Vector2Int, int>();
    }


    //初始化显示
    public void display(BoardEntity entity) {
    
        

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
