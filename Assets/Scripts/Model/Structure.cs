using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Structure
{
    /// <summary>
    ///   <para> 角色操控方式 </para>
    /// </summary>
    public enum PlayerForm {
        Player,     //玩家操控
        Computer,   //AI操控
        Banned      //禁用角色
    }

    /// <summary>
    ///   <para> 格子的特殊效果 </para>
    /// </summary>
    public enum SpecialEffect {
        None,           //无效果
        Double_Step,    //倍速
        Broken_Bridge,  //危桥
        Portal          //传送门
    }

    /// <summary>
    ///   <para> 玩家，用棋子颜色唯一标识 </para>
    /// </summary>
    public enum PlayerID {
        Red,
        Blue,
        Yellow,
        Green
    }
}


