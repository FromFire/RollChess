using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//描述所有玩家的信息

[System.Serializable]
public class PlayersEntity
{
    public int number;
}


// 描述单个棋子的信息

[System.Serializable]
public class SingleTokenEntity
{
    public int x;
    public int y;
}


// 描述所有棋子的信息

[System.Serializable]
public class TokensEntity
{
    public int number;
    public List<SingleTokenEntity> singleTokens;
}


// 描述单个地图格子的信息

[System.Serializable]
public class SingleMapGridEntity
{
    public int x;
    public int y;
}


// 地图类，对应json的根

[System.Serializable]
public class BoardEntity
{
    public string mapName;

    public PlayersEntity players;

    public List<TokensEntity> tokens;

    public List<SingleMapGridEntity> map;

    //控制台输出信息
    public void toConsole() {
        Debug.Log("名称："+mapName);
        Debug.Log("玩家数目："+players.number);
        for(int i=0; i<players.number;i++) {
            Debug.Log("棋子数目"+i+": "+tokens[i].number);
        }
        for(int i=0; i<map.ToArray().Length;i++) {
            Debug.Log("x: "+map[i].x + "\t y: " +map[i].y);
        }
        
    }
}