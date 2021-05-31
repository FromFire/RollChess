using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Random = System.Random;

/// <summary>
///   <para> 玩家 </para>
/// </summary>
public class Player : NetworkBehaviour {
    public NetworkConnection conn;
    private uint id;
    private string name;

    /// <summary>
    ///   <para> 玩家昵称池-没用过的 </para>
    /// </summary>
    public static List<string> namePoolUnused = new List<string>() {
        "暴怒的短吻鳄", "轻快的北极兔", "灵动的藏羚羊", "智慧的灵猴", "燃烧的翼龙",
        "狡诈的变色龙", "矫健的豹猫", "华美的极乐鸟", "剧毒的狼蛛", "轻灵的曙凤蝶",
        "深海的大王乌贼", "潜伏的金环蛇", "美味的秋刀鱼", "掠食的军舰鸟", "洄游的座头鲸"
    };

    /// <summary>
    ///   <para> 玩家昵称池-用过的 </para>
    /// </summary>
    public static List<string> namePoolUsed = new List<string>();

    public uint Id {
        get { return id; }
        set {
            id = value;
            // 随机起名
            if (name is null || name.Length == 0) {
                name = namePoolUnused[new Random().Next() % namePoolUnused.Count];
                namePoolUnused.Remove(name);
                namePoolUsed.Add(name);
            }
        }
    }

    public string Name {
        get { return name; }
    }

    // 回收昵称
    private void OnDestroy() {
        namePoolUnused.Add(name);
        namePoolUsed.Remove(name);
    }
}