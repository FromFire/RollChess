using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//管理所有棋子的类
public class TokenSet : MonoBehaviour {

    List<Token> tokenList;

    // 使用TokensEntity初始化
    public void init(List<TokenEntity> entity) {
        tokenList = new List<Token>();
        foreach(TokenEntity token in entity) {
            tokenList.Add(new Token(token.x, token.y, token.player));
        }
    }

    // 查找棋子
    // player未指定或为-1时，不查找player
    public List<Token> find(Vector2Int pos, int player=-1) {
        List<Token> ret = new List<Token>();
        foreach(Token token in tokenList) {
            if(token.getXY() == pos && (player == -1 || player == token.player) ) {
                ret.Add(token);
            }
        }
        return ret;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//棋子类：单个棋子
public class Token{

    int x;
    int y;
    public int player;

    public Token(int x, int y, int player) {
        this.x = x;
        this.y = y;
    }

    public Vector2Int getXY() {
        return new Vector2Int(x,y);
    }

    public void setXY(Vector2Int xy) {
        x = xy.x;
        y = xy.y;
    }
}
