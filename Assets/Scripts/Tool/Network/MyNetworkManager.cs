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
        NetworkResource.networkInfo.AddPlayer();
    }

    /// <summary>
    ///   <para> 连接成功后，在Client上回调 </para>
    /// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("我是客户端，我已连接");
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