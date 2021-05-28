using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
///   <para> 继承NetworkManager </para>
/// </summary>
public class MyNetworkManager : NetworkManager {
    /// <summary>
    ///   <para> 连接成功后，在Server上回调 </para>
    /// </summary>
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("我是服务器，我完成了一次连接");
        
        // 添加
        NetworkResource.networkInfo.RpcAddPlayer();
        
        // 生成player对象
        GameObject playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.transform.SetParent(transform);
        Player player = playerObject.GetComponent<Player>();
        
        // 初始化player相关
        // Debug.Log(conn.identity is null);
        player.Identity = conn.identity;
        NetworkResource.networkInfo.players.Add(player.id, player);
        Debug.Log("当前玩家数：" + NetworkResource.networkInfo.players.Count);
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }

    /// <summary>
    ///   <para> 连接成功后，在Client上回调 </para>
    /// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("我是客户端，我已连接");
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Success);
        
        // 添加
        NetworkResource.networkInfo.RpcAddPlayer();
    }

    /// <summary>
    ///   <para> Host建立时回调 </para>
    /// </summary>
    public override void OnStartHost()
    {
        Debug.Log("启动Host");
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Server_Success);
    }
}