using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 所有棋子 </para>
/// </summary>
public class TokenSet {

    /// <summary>
    ///   <para> 数据存储，id对应棋子 </para>
    /// </summary>
    private Dictionary<int, Token> tokenList;

    /// <summary>
    ///   <para> 下一个使用的棋子id </para>
    ///   <para> 随棋子增加而增加，棋子减少时不影响 </para>
    /// </summary>
    private int nextId = 0;

    // todo: subject 更新推送

    public TokenSet() {
        tokenList = new Dictionary<int, Token> ();
    }

    /// <summary>
    ///   <para> 增加棋子 </para>
    /// </summary>
    public void Add(Token token) {
        tokenList[nextId] = token;
        nextId ++;
    }

    /// <summary>
    ///   <para> 移除棋子 </para>
    /// </summary>
    public void Remove(List<int> idList) {
        foreach(int id in idList) {
            tokenList.Remove(id);
        }
    }

    /// <summary>
    ///   <para> 移动棋子位置，自动吃子 </para>
    /// </summary>
    public void Move(int id, Vector2Int target) {
        // 修改该棋子位置
        Token token = tokenList[id];
        Vector2Int from = token.position;
        token.position = target;

        // 查询target处其他玩家的棋子
        Dictionary<QueryParam, int> param = new Dictionary<QueryParam, int>();
        param[QueryParam.Player_Ignore] = (int)token.player;
        param[QueryParam.PositionX] = target.x;
        param[QueryParam.PositionY] = target.y;
        List<int> toEat = Query(param);

        // 移除target处其他玩家的棋子
        Remove(toEat);
    }

    /// <summary>
    ///   <para> 查询棋子 </para>
    /// </summary>
    public List<int> Query(Dictionary<QueryParam, int> param) {
        List<int> ret = new List<int>();

        // 特殊情况：无参数，返回所有id
        if(param == null || param.Count == 0) {
            foreach(KeyValuePair<int, Token> kvp in tokenList) {
                ret.Add(kvp.Key);
            }
            return ret;
        }

        // 解析参数：Player
        bool playerAvailable = param.ContainsKey(QueryParam.Player) ? true : false;
        PlayerID player = playerAvailable ? (PlayerID)param[QueryParam.Player] : 0;

        // 解析参数：IgnorePlayer
        bool playerIgnoreAvailable = param.ContainsKey(QueryParam.Player_Ignore) ? true : false;
        PlayerID playerIgnore = playerIgnoreAvailable ? (PlayerID)param[QueryParam.Player_Ignore] : 0;

        // 解析参数：PositionX, PositionY
        bool positionAvailable = param.ContainsKey(QueryParam.PositionX) ? true : false;
        Vector2Int position = positionAvailable ? new Vector2Int(param[QueryParam.PositionX], param[QueryParam.PositionY]): Vector2Int.zero;

        // 查询
        foreach(KeyValuePair<int, Token> kvp in tokenList) {
            if(
                (playerAvailable && kvp.Value.player == player)
                || (playerIgnoreAvailable && kvp.Value.player != playerIgnore)
                || (positionAvailable && kvp.Value.position == position)
            )
            ret.Add(kvp.Key);
        }
        return ret;
    }

    /// <summary>
    ///   <para> 棋子查询参数 </para>
    /// </summary>
    public enum QueryParam {
        Player,         //玩家
        Player_Ignore,  //排除该玩家
        PositionX,      //棋子位置
        PositionY       //棋子位置
    }
}