using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类，对应json的根
// 只用于初始化时作为json导入的载体。游戏过程中不使用此类

[System.Serializable]
public class BoardEntity
{
    //地图名称
    public string mapName;

    //玩家信息
    public PlayersEntity player;

    //棋子信息
    public List<TokenEntity> tokens;

    //可走格子信息
    public List<SingleMapGridEntity> map;

    //特殊格子信息
    public List<SingleSpecialEntity> special;

    //传送门信息
    public List<SinglePortalEntity> portal;

    public void toConsole() {
        string str = "mapName: " + mapName + "\n" +
            "players - number: " + player.number + "\n" +
            "tokens - size" + tokens.Count + "\n";
        Debug.Log(str);
    }
}

// 描述所有玩家的信息
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

// 描述单个特殊格子的信息
[System.Serializable]
public class SingleSpecialEntity 
{
    public int x;
    public int y;
    public string effect;
}

// 描述单个传送门的信息
[System.Serializable]
public class SinglePortalEntity 
{
    public int fromX;
    public int fromY;
    public int toX;
    public int toY;
}