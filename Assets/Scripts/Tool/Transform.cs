using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 类型转换 </para>
/// </summary>
public class Transform {
    /// <summary>
    ///   <para>给定地图json文件中的特殊块类型名称，得到SpecialEffect。</para>
    ///   <para>用于从文件中读取地图到内存。</para>
    /// </summary>
    public static Dictionary<string, SpecialEffect> specialEffectOfName = new Dictionary<string, SpecialEffect>() {
        {"portal", SpecialEffect.Portal},
        {"brokenBridge", SpecialEffect.Broken_Bridge},
        {"doubleStep", SpecialEffect.Double_Step},
        {"pulse", SpecialEffect.Pulse},
        {"stop", SpecialEffect.Stop},
        {"magicalCircle", SpecialEffect.Magical_Circle},
        {"rollAgain", SpecialEffect.Roll_Again},
    };

    /// <summary>
    ///   <para>给定SpecialEffect，得到地图json文件中的特殊块类型名称。</para>
    ///   <para>用于将内存中的地图导出至文件。</para>
    /// </summary>
    public static Dictionary<SpecialEffect, string> specialNameOfEffect = new Dictionary<SpecialEffect, string>() {
        {SpecialEffect.Portal, "portal"},
        {SpecialEffect.Broken_Bridge, "brokenBridge"},
        {SpecialEffect.Double_Step, "doubleStep"},
        {SpecialEffect.Pulse, "pulse"},
        {SpecialEffect.Stop, "stop"},
        {SpecialEffect.Magical_Circle, "magicalCircle"},
        {SpecialEffect.Roll_Again, "rollAgain"},
    };

    /// <summary>
    ///   <para>给定TileType，得到资源路径。</para>
    ///   <para>用于TilemapManager内部绘制Tile。</para>
    /// </summary>
    public static Dictionary<TileType, string> resourceOfTileType = new Dictionary<TileType, string>() {
        {TileType.Land, "Tiles/floor-lawnGreen"},
        {TileType.Ocean, "Tiles/floor-ocean"},
        {TileType.Special_Portal, "Tiles/special-portal"},
        {TileType.Special_Double_Step, "Tiles/special-doubleStep"},
        {TileType.Special_Broken_Bridge, "Tiles/special-brokenBridge"},
        {TileType.Special_Pulse_On, "Tiles/special-pulseOn"},
        {TileType.Special_Pulse_Off, "Tiles/special-pulseOff"},
        {TileType.Special_Stop, "Tiles/special-stop"},
        {TileType.Special_Magical_Circle, "Tiles/special-ritual"},
        {TileType.Special_Roll_Again, "Tiles/special-rollAgain"},
        {TileType.Special_Neutral_Player, "Tiles/token-neutralAlien"},
        {TileType.Player_Red, "Tiles/token-blueAlien"},
        {TileType.Player_Blue, "Tiles/token-redAlien"},
        {TileType.Player_Green, "Tiles/token-greenAlien"},
        {TileType.Player_Yellow, "Tiles/token-yellowAlien"},
        {TileType.Highlight_Blue, "Tiles/highlight-blue"},
        {TileType.Highlight_Yellow, "Tiles/highlight-yellow"},
        {TileType.HexGrid, "Tiles/hex-grid-2x"}
    };

    /// <summary>
    ///   <para>给定TileType，得到资源路径。</para>
    ///   <para>用于BoardDisplay显示特殊块</para>
    /// </summary>
    public static Dictionary<SpecialEffect, TileType> tileTypeOfSpecialEffect = new Dictionary<SpecialEffect, TileType>() {
        {SpecialEffect.None, TileType.Land},
        {SpecialEffect.Portal, TileType.Special_Portal},
        {SpecialEffect.Broken_Bridge, TileType.Special_Broken_Bridge},
        {SpecialEffect.Double_Step, TileType.Special_Double_Step},
        {SpecialEffect.Pulse, TileType.Special_Pulse_On},
        {SpecialEffect.Stop, TileType.Special_Stop},
        {SpecialEffect.Magical_Circle, TileType.Special_Magical_Circle},
        {SpecialEffect.Roll_Again, TileType.Special_Roll_Again}
    };

    /// <summary>
    ///   <para>给定PlayerId，得到TileType。</para>
    ///   <para>用于TokenDisplay显示玩家棋子</para>
    /// </summary>
    public static Dictionary<PlayerID, TileType> tileTypeOfPlayerId = new Dictionary<PlayerID, TileType>() {
        {PlayerID.Red, TileType.Player_Red},
        {PlayerID.Blue, TileType.Player_Blue},
        {PlayerID.Yellow, TileType.Player_Yellow},
        {PlayerID.Green, TileType.Player_Green}
    };

    /// <summary>
    ///   <para> 根据棋子颜色着色文本 </para>
    /// </summary>
    public static Dictionary<PlayerID, string> PlayerNameOfID = new Dictionary<PlayerID, string>() {
        {PlayerID.Red, "红棋"},
        {PlayerID.Blue, "蓝棋"},
        {PlayerID.Yellow, "黄棋"},
        {PlayerID.Green, "绿棋"}
    };

    /// <summary>
    ///   <para> 根据棋子颜色着色文本 </para>
    /// </summary>
    static public string ColorString(PlayerID color, string content) {
        switch (color) {
            case PlayerID.Red:
                return "<color=#DD0000FF>" + content + "</color>";
            case PlayerID.Blue:
                return "<color=#0000DDFF>" + content + "</color>";
            case PlayerID.Yellow:
                return "<color=#FFDD00FF>" + content + "</color>";
            case PlayerID.Green:
                return "<color=#00DD00FF>" + content + "</color>";
        }
        return content;
    }
}