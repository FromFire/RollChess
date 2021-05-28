using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
///   <para> 继承NetworkManager </para>
/// </summary>
public class MyNetworkManager : NetworkManager
{
    // 玩家列表
    public List<Player> players;

    public override void Start()
    {
        base.Start();
        players = new List<Player>();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("客户端已连接");
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Success);
    }

    public override void OnStartHost()
    {
        Debug.Log("服务器已连接");
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Server_Success);
    }

    /// <summary>
    ///   <para> 添加Player </para>
    /// </summary>
    public void AddPlayer(Player player)
    {
        players.Add(player);
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Success);
    }
}