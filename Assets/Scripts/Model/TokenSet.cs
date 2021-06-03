using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 所有棋子 </para>
/// </summary>
public class TokenSet : MonoBehaviour {
    // 单例
    static TokenSet singleton;
    // 获取单例
    public static TokenSet Get() { return singleton; }
    
    // 数据存储，坐标到棋子
    private Dictionary<Vector2Int, Token> tokens;

    // 构造函数
    public TokenSet() {
        singleton = this;
        tokens = new Dictionary<Vector2Int, Token> ();
    }

    /// <summary>
    ///   <para> 初始化数据 </para>
    /// </summary>
    public void Load(SaveEntity saveEntity) {
        List<TokenSaveEntity> tokenEntity = saveEntity.token;
        foreach(TokenSaveEntity token in tokenEntity) {
            Token newToken = new Token(new Vector2Int(token.x, token.y), (PlayerID)token.player);
            Add(newToken);
        }

        // 初始化时在Add()中推送修改，但此时Subject中无observer，所以推送无效
        // View将统一在初始化时读取和显示数据
    }

    /// <summary>
    ///   <para> 包含性检查 </para>
    /// </summary>
    public bool Contains(Vector2Int pos) {
        return tokens.ContainsKey(pos);
    }

    /// <summary>
    ///   <para> 增加和设置棋子 </para>
    /// </summary>
    public void Add(Token token) {
        tokens[token.Position] = token;
        // 推送修改
        ModelResource.tokenSubject.Notify(ModelModifyEvent.Token, token.Position);
    }

    /// <summary>
    ///   <para> 移除棋子 </para>
    /// </summary>
    public void Remove(Vector2Int position) {
        tokens.Remove(position);
        // 推送修改
        ModelResource.tokenSubject.Notify(ModelModifyEvent.Token, position);
    }

    /// <summary>
    ///   <para> 通过id获取棋子 </para>
    /// </summary>
    public Token Get(Vector2Int position) {
        if(!tokens.ContainsKey(position))
            return null;
        return tokens[position];
    }

    /// <summary>
    ///   <para> 移动棋子位置，自动吃子 </para>
    /// </summary>
    public void Move(Vector2Int source, Vector2Int target) {
        // 吃子
        Token sourceToken = Get(source);
        Token targetToken = Get(target);
        if(targetToken != null && targetToken.Player != sourceToken.Player)
            Remove(target);

        // 修改该棋子位置
        Remove(source);
        sourceToken.Position = target;
        Add(sourceToken);

        // 无须推送修改，因为Token内部修改和Remove已有推送
    }

    /// <summary>
    ///   <para> 查询棋子 </para>
    /// </summary>
    public List<Vector2Int> Query(PlayerID player, PlayerID ignore) {
        List<Vector2Int> ret = new List<Vector2Int>();

        // 特殊情况：无参数，返回所有id
        if(player == PlayerID.None && ignore == PlayerID.None) {
            foreach(KeyValuePair<Vector2Int, Token> kvp in tokens)
                ret.Add(kvp.Key);
            return ret;
        }

        // 查询
        foreach(KeyValuePair<Vector2Int, Token> kvp in tokens) {
            if(
                (player == PlayerID.None ? true : kvp.Value.Player == player)
                && (ignore == PlayerID.None ? true : kvp.Value.Player != ignore)
            )
            ret.Add(kvp.Key);
        }
        return ret;
    }
}