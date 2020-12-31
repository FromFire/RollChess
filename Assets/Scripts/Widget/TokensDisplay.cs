using System.Collections.Generic;
using Structure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Widget {
    public class TokensDisplay : MonoBehaviour {
        //Tilemap棋子层
        public Tilemap tilemapToken;

        //存储各类Tile的集合
        //player的值对应tileList的下标
        List<Tile> tileList;

        //Tile在tileList中存储的顺序
        public enum TileKeys {
            tokenRedAlien,
            tokenBlueAlien,
            tokenYellowAlien,
            tokenGreenAlien,
            tokenNeutralAlien
        }; //棋子

        // Start is called before the first frame update
        void Start() {
            // tile顺序按照enum tileKeys中规定的来
            List<string> tileNames = new List<string> {
                "Tiles/token-redAlien", //tokenRedAlien
                "Tiles/token-blueAlien", //tokenBlueAlien
                "Tiles/token-yellowAlien", //tokenYellowAlien
                "Tiles/token-greenAlien", //tokenGreenAlien
                "Tiles/token-neutralAlien" //tokenNeutralAlien
            };

            // 读取所有tile
            tileList = new List<Tile>();
            foreach (string name in tileNames) {
                tileList.Add(Resources.Load<Tile>(name));
            }
        }

        //初始化显示
        public void Display(List<TokenEntity> entity) {
            //不同阵营棋子外观不同
            List<Tile> tokenTiles = new List<Tile>();
            tokenTiles.Add(tileList[(int) TileKeys.tokenRedAlien]);
            tokenTiles.Add(tileList[(int) TileKeys.tokenBlueAlien]);
            tokenTiles.Add(tileList[(int) TileKeys.tokenYellowAlien]);
            tokenTiles.Add(tileList[(int) TileKeys.tokenGreenAlien]);
            tokenTiles.Add(tileList[(int) TileKeys.tokenNeutralAlien]);

            //分阵营显示棋子
            foreach (TokenEntity token in entity) {
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
            if (number == 0) {
                tilemapToken.SetTile(pos3, null);
            }
            else {
                tilemapToken.SetTile(pos3, tile);
            }
        }

        // TODO: 删除，放置
    }
}