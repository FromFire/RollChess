using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structure;

/// <summary>
///   <para> 单个棋子 </para>
/// </summary>
public class Token {
    // 棋子id
    private int id;

    //属于哪个玩家
    private PlayerID player;

    /// <summary>
    ///   <para> 棋子位置 </para>
    /// </summary>
    public Vector2Int position;
}