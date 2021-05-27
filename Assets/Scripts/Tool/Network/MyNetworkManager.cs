using System;
using Mirror;
using UnityEngine;

/// <summary>
///   <para> 继承NetworkManager </para>
/// </summary>
public class MyNetworkManager : NetworkManager
{
    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Success);
    }

    public override void OnStartHost()
    {
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Server_Success);
    }
}