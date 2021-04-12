using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structure;

/// <summary>
///   <para> 所有棋子 </para>
/// </summary>
public class TokenSet {

    /// <summary>
    ///   <para> 数据存储 </para>
    /// </summary>
    private List<Token> tokenList;

    // todo: subject 更新推送

    /// <summary>
    ///   <para> 增加棋子 </para>
    /// </summary>
    public void Add(Token token) {

    }

    /// <summary>
    ///   <para> 移除棋子 </para>
    /// </summary>
    public void Remove(int id) {

    }

    /// <summary>
    ///   <para> 移动棋子位置 </para>
    /// </summary>
    public void Move(int id, Vector2Int target) {

    }

    /// <summary>
    ///   <para> 查询棋子 </para>
    /// </summary>
    public List<int> Query(Dictionary<QueryParam, Object> param) {
        return null;
    }

    /// <summary>
    ///   <para> 棋子查询参数 </para>
    /// </summary>
    public enum QueryParam {
        Player,         //玩家
        Player_Ignore,  //排除该玩家
        Position        //棋子位置
    }
}