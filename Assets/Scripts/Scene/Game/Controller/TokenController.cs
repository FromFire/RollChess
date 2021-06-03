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
        // 移动
        TokenSet.Get().Move(from, to);
    }

    /// <summary>
    ///   <para> 杀死棋子 </para>
    /// </summary>
    public void Kill(Vector2Int pos) {
        // 移除
        TokenSet.Get().Remove(pos);
    }
}