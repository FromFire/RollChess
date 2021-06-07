using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 单人玩家选地图 </para>
/// </summary>
public class SinglePage : MonoBehaviour {
    // 开始游戏
    public void StartGame() {
        EntranceController.Get().StartSingleGame();
    }
}