using System;
using UnityEngine;
using System.Text.RegularExpressions;

public class ConnectionController : MonoBehaviour {
    // 单例
    static public ConnectionController singleton;
    
    private void Start() {
        singleton = this;
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Disconnect, OnDisconnect);
    }

    /// <summary>
    ///   <para> 创建房间 </para>
    /// </summary>
    public void CreateRoom() {
        if (!EntranceController.Get().isGameAvalible())
            return;
        
        // 创建host
        NetworkResource.networkManager.StartHost();
    }
    
    /// <summary>
    ///   <para> 加入房间 </para>
    /// </summary>
    public void JoinRoom(string ip) {
        // localhost的情况
        if (ip.Equals("localhost") || ip.Length == 0) ;
        // ip不合法
        else if (!IpIsValid(ip))
            return;
        // 合法ip
        else NetworkResource.networkManager.networkAddress = ip;
        
        // 启动客户端
        NetworkResource.networkManager.StartClient();
    }
    
    /// <summary>
    ///   <para> 取消加入房间 </para>
    /// </summary>
    public void CancelJoinRoom() {
        NetworkResource.networkManager.StopClient();
    }
    
    /// <summary>
    ///   <para> 断开网络连接 </para>
    /// </summary>
    public void ExitRoom() {
        NetworkResource.networkManager.StopClient();
        NetworkResource.networkManager.StopServer();
    }
    
    /// <summary>
    ///   <para> 判断ip地址是否合法 </para>
    /// </summary>
    bool IpIsValid(string ip)
    {
        string patternIp = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
        return Regex.IsMatch(ip, patternIp);
    }

    /// <summary>
    ///   <para> 踢人 </para>
    /// </summary>
    public void KickOut(uint id) {
        // 房主才能踢人，而且不能踢自己
        if(Players.Get().isServer && !Players.Get().players[id].isHost)
            Players.Get().players[id].conn.Disconnect();
    }

    /// <summary>
    ///   <para> 断开连接，返回创建房间页面 </para>
    /// </summary>
    public void OnDisconnect() {
        PanelManager.Get().NowPanel = PanelManager.Get().joinRoom;
    }
}