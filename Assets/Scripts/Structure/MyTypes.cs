using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Structure {
    /// <summary>
    ///   <para> 角色对应颜色 </para>
    /// </summary>
    public enum PlayerColor {
        Red,
        Blue,
        Yellow,
        Green
    }

    /// <summary>
    ///   <para> 角色的操控方式 </para>
    /// </summary>
    public enum PlayerChoices {
        Player, // 玩家控制
        Comuputer, // AI控制
        Banned // 此角色不参与游戏
    };

    /// <summary>
    ///   <para>提供了全局静态常量。</para>
    /// </summary>
    public static class Constants {
        /// <summary>
        ///   <para>玩家数量是4</para>
        /// </summary>
        public static int PLAYERNUMBER = 4;
    }
}