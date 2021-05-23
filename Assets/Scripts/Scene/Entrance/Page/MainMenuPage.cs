using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 单人玩家选地图 </para>
/// </summary>
public class MainMenuPage : MonoBehaviour {

    /// <summary>
    ///   <para> 选择单人游戏，去选地图界面 </para>
    /// </summary>
    public void ToSinglePage() {
        PanelManager.Get().NowPanel = PanelManager.Get().single;
    }

    /// <summary>
    ///   <para> 选择多人游戏，去房间大厅界面 </para>
    /// </summary>
    public void ToRoomPage() {
        
    }

    /// <summary>
    ///   <para> 选择地图编辑器，去地图编辑选地图界面 </para>
    /// </summary>
    public void ToChooseEditMapPage() {
        
    }
}