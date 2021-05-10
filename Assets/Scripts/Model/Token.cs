using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 单个棋子 </para>
/// </summary>
public class Token {

    // 属于哪个玩家
    private PlayerID player;

    // 棋子位置
    private Vector2Int position;

    // 构造函数
    public Token(Vector2Int position, PlayerID player) {
        this.position = position;
        this.player = player;
    }

    // 不能构造空的Token
    private Token() {}

    /// <summary>
    ///   <para> 属于哪个玩家 </para>
    /// </summary>
    public PlayerID Player {
        get {return player;}
        set {
            player = value;
            //推送修改
            ModelResource.tokenSubject.Notify(ModelModifyEvent.Token, position); 
        }
    }

    /// <summary>
    ///   <para> 棋子位置 </para>
    /// </summary>
    public Vector2Int Position {
        get {return position;}
        set {
            Vector2Int valueOld = position;
            position = value;
            //推送修改
            ModelResource.tokenSubject.Notify(ModelModifyEvent.Token, valueOld);
            ModelResource.tokenSubject.Notify(ModelModifyEvent.Token, position);
        }
    }
}