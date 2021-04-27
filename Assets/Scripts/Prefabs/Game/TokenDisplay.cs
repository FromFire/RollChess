using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 显示棋子 </para>
/// </summary>
public class TokenDisplay : MonoBehaviour {
    // Tilemap棋子层
    [SerializeField] private TilemapManager tilemap;

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display() {
        // 获取所有棋子
        List<int> tokenId = PublicResource.tokenSet.Query(null);
        // 按阵营显示棋子
        foreach (int id in tokenId) {
            Token token = PublicResource.tokenSet.GetToken(id);
            tilemap.SetTile(token.Position, Transform.tileTypeOfPlayerId[token.Player]);
        }
    }

    /// <summary>
    ///   <para> 响应TokenSet更新 </para>
    /// </summary>
    public void Update(Vector2Int pos) {
        // 获取被修改处的所有棋子
        Dictionary<TokenSet.QueryParam, int> param = new Dictionary<TokenSet.QueryParam, int> {
            {TokenSet.QueryParam.PositionX, pos.x},
            {TokenSet.QueryParam.PositionY, pos.y}
        };
        List<int> tokenId = PublicResource.tokenSet.Query(param);

        // 若此处已无棋子，则清空此格
        if(tokenId is null || tokenId.Count == 0)
            tilemap.RemoveTile(pos);

        // 若此处有棋子，更新阵营
        else {
            // 由于一个位置上的棋子都是同一阵营的，所以用第一个为代表
            Token token = PublicResource.tokenSet.GetToken(tokenId[0]);
            // 重新显示此格
            tilemap.SetTile(token.Position, Transform.tileTypeOfPlayerId[token.Player]);
        }

        // 若此处棋子数多于1个，在其上方显示数字
        if(tokenId.Count > 1) {
            // 暂空
        }
    }
}