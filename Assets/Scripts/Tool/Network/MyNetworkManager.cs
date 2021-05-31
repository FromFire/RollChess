using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
///   <para> 继承NetworkManager </para>
/// </summary>
public class MyNetworkManager : NetworkManager {
    /// <summary>
    ///   <para> 创建新Player </para>
    /// </summary>
    public override void OnServerAddPlayer(NetworkConnection conn) {
        // base的内容
        GameObject playerobject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        NetworkServer.AddPlayerForConnection(conn, playerobject);
        
        // 修改player属性
        Player player = playerobject.GetComponent<Player>();
        player.conn = conn;
        player.Id = conn.identity.netId;
        
        // NetworkInfo添加player
        NetworkResource.networkInfo.RpcAddPlayer(player);
        Debug.Log("新player：(id:" + player.Id + ", name:" + player.name + ")");
    }

    /// <summary>
    ///   <para> 连接成功后，在Server上回调 </para>
    /// </summary>
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log("我是服务器，我完成了一次连接");
    }

    /// <summary>
    ///   <para> 连接成功后，在Client上回调 </para>
    /// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("我是客户端，我已连接");

        // 添加                      
        //NetworkResource.networkInfo.AddId();
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Success);
    }

    /// <summary>
    ///   <para> Host建立时回调 </para>
    /// </summary>
    public override void OnStartHost()
    {
        Debug.Log("启动Host");
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Server_Success);
        base.OnStartHost();
    }
}