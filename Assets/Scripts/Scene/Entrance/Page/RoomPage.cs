using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 单人玩家选地图 </para>
/// </summary>
public class RoomPage : MonoBehaviour
{
    [SerializeField] private InputField ipInputField;
    
    /// <summary>
    ///   <para> 创建房间，不开服务器，直接跳转至选角色界面 </para>
    /// </summary>
    public void CreateRoom()
    {
        EntranceResource.entranceController.CreateRoom();
    }
    
    /// <summary>
    ///   <para> 加入房间 </para>
    /// </summary>
    public void JoinRoom()
    {
        string ip = ipInputField.text;
        EntranceResource.entranceController.JoinRoom(ip);
    }
    
    /// <summary>
    ///   <para> 取消加入 </para>
    /// </summary>
    public void CancelJoinRoom()
    {
        string ip = ipInputField.text;
        EntranceResource.entranceController.JoinRoom(ip);
    }

    /// <summary>
    ///   <para> 加入房间成功，跳转至选角色界面 </para>
    /// </summary>
    public void JoinRoomSuccess()
    {
        string ip = ipInputField.text;
        EntranceResource.entranceController.JoinRoom(ip);
    }
}