using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类，对应json的根
// 只用于初始化时作为json导入的载体。游戏过程中不使用此类

[System.Serializable]
public class BoardEntity
{
    public string mapName;

    public PlayersEntity players;

    public List<TokenEntity> tokens;

    public List<SingleMapGridEntity> map;

    public void toConsole() {
        string str = "mapName: " + mapName + "\n" +
            "players - number: " + players.number + "\n" +
            "tokens - size" + tokens.Count + "\n";
        Debug.Log(str);
    }
}

//描述所有玩家的信息

[System.Serializable]
public class PlayersEntity
{
    public int number;
}


// 描述单个棋子的信息

[System.Serializable]
public class TokenEntity
{
    public int x;
    public int y;
    public int player;
}

// 描述单个地图格子的信息

[System.Serializable]
public class SingleMapGridEntity
{
    public int x;
    public int y;
}