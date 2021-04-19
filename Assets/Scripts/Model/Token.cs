using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structure;

/// <summary>
///   <para> 单个棋子 </para>
/// </summary>
public class Token {

    //属于哪个玩家
    public PlayerID player;

    /// <summary>
    ///   <para> 棋子位置 </para>
    /// </summary>
    public Vector2Int position;

    public Token(Vector2Int position, PlayerID player) {
        this.position = position;
        this.player = player;
    }

    public Token() {

    }
}