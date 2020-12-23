using System.Collections.Generic;
using Structure;
using UnityEngine;

namespace Widget {
    public class TokensDisplay : MonoBehaviour {
        //Tilemap棋子层
        public TilemapManager tilemapManager;

        //player的值对应tokenList的下标
        public List<TileType> tokenList = new List<TileType>() {
            TileType.Token_Alien_Red, TileType.Token_Alien_Red //棋子
        };

        //初始化显示
        public void Display(List<TokenEntity> entity) {
            //不同阵营棋子外观不同
            //分阵营显示棋子
            foreach (TokenEntity token in entity)
                tilemapManager.SetTile(new Vector2Int(token.x, token.y), tokenList[token.player]);
        }

        //在pos处显示number个player方的棋子
        //认定player为TileList的下标
        public void ShowToken(Vector2Int pos, int number, int player) {
            //根据player获取棋子外观
            TileType tile = tokenList[player];
            //显示（目前不显示number）
            if (number == 0) {
                tilemapManager.EraseTile(pos);
            }
            else {
                tilemapManager.SetTile(pos, tile);
            }
        }

        // TODO: 删除，放置
    }
}