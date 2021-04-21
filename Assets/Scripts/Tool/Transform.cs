using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Transform {
    /// <summary>
    ///   <para>给定地图json文件中的特殊块类型名称，得到SpecialEffect。</para>
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
    /// </summary>
    public static Dictionary<TileType, string> resourceOfTileType = new Dictionary<TileType, string>() {
        {TileType.Land, "Tiles/floor-lawnGreen"},
        {TileType.Special_Portal, "Tiles/special-portal"},
        {TileType.Special_Double_Step, "Tiles/special-doubleStep"},
        {TileType.Special_Broken_Bridge, "Tiles/special-brokenBridge"},
        {TileType.Special_Pulse_On, "Tiles/special-pulseOn"},
        {TileType.Special_Pulse_Off, "Tiles/special-pulseOff"},
        {TileType.Special_Stop, "Tiles/special-stop"},
        {TileType.Special_Magical_Circle, "Tiles/special-ritual"},
        {TileType.Special_Roll_Again, "Tiles/special-rollAgain"},
        {TileType.Player_Red, "Tiles/token-blueAlien"},
        {TileType.Player_Blue, "Tiles/token-redAlien"},
        {TileType.Player_Green, "Tiles/token-greenAlien"},
        {TileType.Player_Yellow, "Tiles/token-yellowAlien"},
        {TileType.Player_Neutral, "Tiles/token-neutralAlien"},
        {TileType.HexGrid, "Tiles/token-hexGrid"}
    };
}