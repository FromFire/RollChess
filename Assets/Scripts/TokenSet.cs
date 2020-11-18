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

    //移动棋子，包括吃子的处理
    public void moveToken(Vector2Int from, Vector2Int to) {
        //移动棋子
        int player = -1;
        foreach(Token token in tokenList) {
            if(token.getXY() == from) {
                player = token.player;
                token.setXY(to);
                break;
            }
        }

        //吃子判定
        removeEnemies(to, player);
    }

    //移除该格子上的所有敌人棋子（pos符合，但阵营不是player的棋子）
    public void removeEnemies(Vector2Int pos, int player) {
        for(int i=tokenList.Count-1; i>=0; i--) {
            if(tokenList[i].getXY() == pos && player != tokenList[i].player) {
                Debug.Log("remove token: (" + pos.x + "." + pos.y + ") player:" + tokenList[i].player);
                tokenList.RemoveAt(i);
            }
        }
    }

    // 查找棋子
    // player未指定或为-1时，忽略player
    public List<Token> find(Vector2Int pos, int player=-1) {
        List<Token> ret = new List<Token>();
        foreach(Token token in tokenList) {
            if(token.getXY() == pos && (player == -1 || player == token.player)) {
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
        this.player = player;
    }

    public Vector2Int getXY() {
        return new Vector2Int(x,y);
    }

    public void setXY(Vector2Int xy) {
        x = xy.x;
        y = xy.y;
    }
}
