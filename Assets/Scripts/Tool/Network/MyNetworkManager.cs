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
        Debug.Log("服务器：客户端已连接");
        
        // 生成player对象
        GameObject playerObject = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.transform.SetParent(transform);
        Player player = playerObject.GetComponent<Player>();
        
        // 添加
        NetworkResource.networkInfo.CmdAddPlayer(player);
    }

    /// <summary>
    ///   <para> 连接成功后，在Client上回调 </para>
    /// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("客户端：客户端已连接");
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Success);
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