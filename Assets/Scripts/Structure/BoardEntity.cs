using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类，对应json的根
// 只用于初始化时作为json导入的载体。游戏过程中不使用此类

[System.Serializable]
public class BoardEntity {
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

    public void ToConsole() {
        string str = "mapName: " + mapName + "\n" +
                     "players - number: " + player.min + "\n" +
                     "tokens - size" + tokens.Count + "\n";
        Debug.Log(str);
    }

    /// <summary>
    ///   <para> 转换为json格式文本 </para>
    /// </summary>
    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    /// <summary>
    ///   <para> 从json格式转换 </para>
    /// </summary>
    static public BoardEntity FromJson(string json) {
        return JsonUtility.FromJson<BoardEntity>(json);
    }
}

// 描述所有玩家的信息
[System.Serializable]
public class PlayersEntity {
    public PlayersEntity(int min, int max) {
        this.min = min;
        this.max = max;
    }

    public int min;
    public int max;
}

// 描述单个棋子的信息
[System.Serializable]
public class TokenEntity {
    public TokenEntity(int x, int y, int player) {
        this.x = x;
        this.y = y;
        this.player = player;
    }

    public int x;
    public int y;
    public int player;
}

// 描述单个地图格子的信息
[System.Serializable]
public class SingleMapGridEntity {
    public SingleMapGridEntity(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public int x;
    public int y;
}

// 描述单个特殊格子的信息
[System.Serializable]
public class SingleSpecialEntity {
    public SingleSpecialEntity(int x, int y, string effect) {
        this.x = x;
        this.y = y;
        this.effect = effect;
    }

    public int x;
    public int y;
    public string effect;
}

// 描述单个传送门的信息
[System.Serializable]
public class SinglePortalEntity {
    public SinglePortalEntity(int fromX, int fromY, int toX, int toY) {
        this.fromX = fromX;
        this.fromY = fromY;
        this.toX = toX;
        this.toY = toY;
    }

    public int fromX;
    public int fromY;
    public int toX;
    public int toY;
}