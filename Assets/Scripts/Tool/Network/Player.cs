using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

/// <summary>
///   <para> 玩家 </para>
/// </summary>
public class Player : NetworkBehaviour {
    // identity
    private NetworkIdentity identity;
    
    /// <summary>
    ///   <para> 唯一id </para>
    /// </summary>
    [SyncVar] public uint id;
    
    /// <summary>
    ///   <para> 显示在UI上的玩家名字 </para>
    /// </summary>
    [SyncVar] public string name;

    /// <summary>
    ///   <para> 自动生成id和name </para>
    /// </summary>
    public NetworkIdentity Identity {
        get { return identity; }
        set {
            identity = value;
            // id = identity.netId;
            name = "玩家" + id;
            Debug.Log("新Player:" + id);
        }
    }

}