using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 对棋子数据（TokenSet + Token）的操作和处理 </para>
/// </summary>
public class TokenController : MonoBehaviour  {
    /// <summary>
    ///   <para> 移动棋子，处理吃子 </para>
    /// </summary>
    public void Move(Vector2Int from, Vector2Int to) {
        // 寻找from处的棋子
        Dictionary<TokenSet.QueryParam, int> param = new Dictionary<TokenSet.QueryParam, int> {
            {TokenSet.QueryParam.PositionX, from.x},
            {TokenSet.QueryParam.PositionY, from.y}
        };
        int token = PublicResource.tokenSet.Query(param)[0];

        // 移动
        PublicResource.tokenSet.Move(token, to);
    }
}