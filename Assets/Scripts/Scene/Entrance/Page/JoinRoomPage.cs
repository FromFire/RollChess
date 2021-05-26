using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 创建和加入房间 </para>
/// </summary>
public class JoinRoomPage : MonoBehaviour
{
    // ip输入框
    [SerializeField] private InputField ipInputField;
    // 加入房间按钮的文本
    [SerializeField] private Text joinRoomButtonText;
    // 加入房间按钮
    [SerializeField] private Button joinRoomButton;

    private void Update()
    {
        //if(NetworkResource.networkManager.)
    }

    /// <summary>
    ///   <para> 创建房间，不开服务器，直接跳转至选角色界面 </para>
    /// </summary>
    public void CreateRoom()
    {
        PanelManager.Get().NowPanel = PanelManager.Get().roomOwnerChooseMap;
    }
    
    /// <summary>
    ///   <para> 尝试加入房间 </para>
    /// </summary>
    public void JoinRoom()
    {
        string ip = ipInputField.text;
        EntranceResource.entranceController.JoinRoom(ip);
        
        // 把“加入房间”按钮改为“取消加入”
        joinRoomButtonText.text = "取消加入";
        // 锁定ip输入框
        ipInputField.interactable = false;
        // 按钮响应函数修改为取消加入
        joinRoomButton.onClick.RemoveAllListeners();
        joinRoomButton.onClick.AddListener(CancelJoinRoom);
    }
    
    /// <summary>
    ///   <para> 取消加入 </para>
    /// </summary>
    public void CancelJoinRoom()
    {
        EntranceResource.entranceController.CancelJoinRoom();
        
        // 把“取消房间”按钮改为“加入房间”
        joinRoomButtonText.text = "加入房间";
        // 解锁ip输入框
        ipInputField.interactable = true;
        // 按钮相应函数修改为加入房间
        joinRoomButton.onClick.RemoveAllListeners();
        joinRoomButton.onClick.AddListener(JoinRoom);
    }

    /// <summary>
    ///   <para> 加入房间成功，跳转至选角色界面 </para>
    /// </summary>
    public void JoinRoomSuccess()
    {
        PanelManager.Get().NowPanel = PanelManager.Get().room;
    }
}