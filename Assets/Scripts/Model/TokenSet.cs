using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 所有棋子 </para>
/// </summary>
public class TokenSet {

    // 数据存储，id对应棋子
    private Dictionary<int, Token> tokenList;

    // 下一个使用的棋子id
    // 随棋子增加而增加，棋子减少时不影响
    private int nextId = 0;

    /// <summary>
    ///   <para> 更新推送 </para>
    /// </summary>
    public PositionSubject subject;

    // 构造函数
    public TokenSet() {
        tokenList = new Dictionary<int, Token> ();
        subject = new PositionSubject();
    }

    /// <summary>
    ///   <para> 通过id获取棋子 </para>
    /// </summary>
    public Token GetToken(int id) {
        return tokenList[id];
    }

    /// <summary>
    ///   <para> 初始化数据 </para>
    /// </summary>
    public void Load(SaveEntity saveEntity) {
        List<TokenSaveEntity> tokenEntity = saveEntity.token;
        foreach(TokenSaveEntity token in tokenEntity) {
            Token newToken = new Token(new Vector2Int(token.x, token.y), (PlayerID)token.player);
            newToken.subject = this.subject;
            Add(newToken);
        }

        // 初始化时在Add()中推送修改，但此时Subject中无observer，所以推送无效
        // View将统一在初始化时读取和显示数据
    }

    /// <summary>
    ///   <para> 增加棋子 </para>
    /// </summary>
    public void Add(Token token) {
        tokenList[nextId] = token;
        nextId ++;

        // 推送修改
        subject.Notify(ModelModifyEvent.Token, token.Position);
    }

    /// <summary>
    ///   <para> 移除棋子 </para>
    /// </summary>
    public void Remove(List<int> idList) {
        List<Vector2Int> modified = new List<Vector2Int>();
        foreach(int id in idList) {
            // 记录被修改的坐标
            modified.Add(tokenList[id].Position);
            // 移除
            tokenList.Remove(id);
        }

        // 推送修改
        subject.Notify(ModelModifyEvent.Token, modified);
    }

    /// <summary>
    ///   <para> 移动棋子位置，自动吃子 </para>
    /// </summary>
    public void Move(int id, Vector2Int target) {
        // 查询target处其他玩家的棋子，即被吃的棋子
        Token token = tokenList[id];
        Dictionary<QueryParam, int> param = new Dictionary<QueryParam, int>() {
            {QueryParam.Player_Ignore, (int)token.Player},
            {QueryParam.PositionX, target.x},
            {QueryParam.PositionY, target.y}
        };
        List<int> toEat = Query(param);

        // 移除target处其他玩家的棋子
        Remove(toEat);

        // 修改该棋子位置
        Vector2Int from = token.Position;
        token.Position = target;

        // 无须推送修改，因为Token内部修改和Remove已有推送
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
                (playerAvailable && kvp.Value.Player == player)
                || (playerIgnoreAvailable && kvp.Value.Player != playerIgnore)
                || (positionAvailable && kvp.Value.Position == position)
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