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
}