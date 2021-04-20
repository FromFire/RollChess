using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 存档数据 </para>
/// </summary>
[System.Serializable]
public class SaveEntity {
    
    /// <summary>
    ///   <para> 地图名称 </para>
    /// </summary>  
    public string mapName;

    /// <summary>
    ///   <para> 玩家信息 </para>
    /// </summary>  
    public PlayerSaveEntity player;

    /// <summary>
    ///   <para> 棋子信息 </para>
    /// </summary>  
    public List<TokenSaveEntity> token;

    /// <summary>
    ///   <para> 可走地图信息 </para>
    /// </summary>  
    public List<LandSaveEntity> map;

    /// <summary>
    ///   <para> 特殊块信息 </para>
    /// </summary>  
    public List<SpecialSaveEntity> special;

    /// <summary>
    ///   <para> 传送门信息 </para>
    /// </summary>  
    public List<PortalSaveEntity> portal;

    /// <summary>
    ///   <para> 转换为json格式文本 </para>
    /// </summary>
    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    /// <summary>
    ///   <para> 从json格式转换 </para>
    /// </summary>
    static public SaveEntity FromJson(string json) {
        return JsonUtility.FromJson<SaveEntity>(json);
    }

    /// <summary>
    ///   <para> 在控制台输出部分信息 </para>
    /// </summary>
    public void ToConsole() {
        string str = "mapName: " + mapName + "\n" +
                     "players - number: " + player.min + "\n" +
                     "tokens - size" + token.Count + "\n";
        Debug.Log(str);
    }
}

/// <summary>
///   <para> 玩家信息 </para>
/// </summary>
[System.Serializable]
public class PlayerSaveEntity {
    
    /// <summary>
    ///   <para> 玩家最小数量 </para>
    /// </summary> 
    public int min;

    /// <summary>
    ///   <para> 玩家最大数量 </para>
    /// </summary> 
    public int max;
}

/// <summary>
///   <para> 单个棋子信息 </para>
/// </summary>
[System.Serializable]
public class TokenSaveEntity {
    
    /// <summary>
    ///   <para> 棋子初始x坐标 </para>
    /// </summary> 
    public int x;

    /// <summary>
    ///   <para> 棋子初始y坐标 </para>
    /// </summary> 
    public int y;

    /// <summary>
    ///   <para> 棋子所属玩家 </para>
    /// </summary> 
    public int player;
}

/// <summary>
///   <para> 单个可走格子信息 </para>
/// </summary>
[System.Serializable]
public class LandSaveEntity {
    
    /// <summary>
    ///   <para> 格子x坐标 </para>
    /// </summary> 
    public int x;

    /// <summary>
    ///   <para> 格子y坐标 </para>
    /// </summary> 
    public int y;
}

/// <summary>
///   <para> 单个特殊块信息 </para>
/// </summary>
[System.Serializable]
public class SpecialSaveEntity {
    
    /// <summary>
    ///   <para> 格子x坐标 </para>
    /// </summary> 
    public int x;

    /// <summary>
    ///   <para> 格子y坐标 </para>
    /// </summary> 
    public int y;

    /// <summary>
    ///   <para> 格子效果 </para>
    /// </summary> 
    public string effect;
}

/// <summary>
///   <para> 单个传送门信息 </para>
/// </summary>
[System.Serializable]
public class PortalSaveEntity {
    
    /// <summary>
    ///   <para> 传送门所在x坐标 </para>
    /// </summary> 
    public int fromX;

    /// <summary>
    ///   <para> 传送门所在y坐标 </para>
    /// </summary> 
    public int fromY;

    /// <summary>
    ///   <para> 传送门目标x坐标 </para>
    /// </summary> 
    public int toX;

    /// <summary>
    ///   <para> 传送门目标y坐标 </para>
    /// </summary> 
    public int toY;
}