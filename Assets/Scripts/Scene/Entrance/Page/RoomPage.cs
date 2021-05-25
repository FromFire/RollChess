using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 单人玩家选地图 </para>
/// </summary>
public class RoomPage : MonoBehaviour {
    /// <summary>
    ///   <para> 创建房间 </para>
    /// </summary>
    public void CreateRoom()
    {
        EntranceResource.entranceController.CreateR0oom();
    }
    
    /// <summary>
    ///   <para> 加入房间 </para>
    /// </summary>
    public void JoinRoom()
    {
        EntranceResource.entranceController.JoinRoom();
    }
}