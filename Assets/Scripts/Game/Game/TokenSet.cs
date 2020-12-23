using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Widget;

//管理所有棋子的类
public class TokenSet : MonoBehaviour {

    TokensDisplay tokensDisplay;

    List<Token> tokenList;

    // 使用TokensEntity初始化
    public void Init(List<TokenEntity> entity) {
        tokenList = new List<Token>();
        foreach(TokenEntity token in entity) {
            tokenList.Add(new Token(token.x, token.y, token.player));
        }

        tokensDisplay = GameObject.Find("/Grid/TilemapToken").GetComponent<TokensDisplay>();
        tokensDisplay.Display(entity);
    }

    //移动棋子，包括吃子的处理
    public void MoveToken(Vector2Int from, Vector2Int to) {
        //移动棋子
        int player = -1;
        foreach(Token token in tokenList) {
            if(token.GetXY() == from) {
                player = token.player;
                token.SetXY(to);
                break;
            }
        }

        //吃子判定
        RemoveEnemies(to, player);

        //更新from格子的显示
        tokensDisplay.ShowToken(from, Find(from, player).Count, player);

        //更新to格子的显示
        tokensDisplay.ShowToken(to, Find(to, player).Count, player);
    }

    //移除该格子上的所有敌人棋子（pos符合，但阵营不是player的棋子）
    public void RemoveEnemies(Vector2Int pos, int player) {
        for(int i=tokenList.Count-1; i>=0; i--) {
            if(tokenList[i].GetXY() == pos && player != tokenList[i].player) {
                Debug.Log("remove token: (" + pos.x + "." + pos.y + ") player:" + tokenList[i].player);
                tokenList.RemoveAt(i);
            }
        }
    }

    //查询某位玩家的棋子数量
    public int GetTokenNumber(int player) {
        int ret=0;
        foreach(Token token in tokenList) {
            if(player == token.player) {
                ret++;
            }
        }
        return ret;
    }

    // 查找棋子
    // player未指定或为-1时，忽略player
    public List<Token> Find(Vector2Int pos, int player=-1) {
        List<Token> ret = new List<Token>();
        foreach(Token token in tokenList) {
            if(token.GetXY() == pos && (player == -1 || player == token.player)) {
                ret.Add(token);
            }
        }
        return ret;
    }

    // 移除player的全部棋子
    public void removePlayer(int player) {
        for(int i=tokenList.Count-1; i>=0; i--) {
            if(player == tokenList[i].player) {
                Vector2Int pos = tokenList[i].GetXY();
                Debug.Log("remove token: (" + pos.x + "." + pos.y + ") player:" + tokenList[i].player);
                tokensDisplay.ShowToken(pos, 0, player);
                tokenList.RemoveAt(i);
            }
        }
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

    public Vector2Int GetXY() {
        return new Vector2Int(x,y);
    }

    public void SetXY(Vector2Int xy) {
        x = xy.x;
        y = xy.y;
    }
}
