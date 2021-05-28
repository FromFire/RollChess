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
    [SerializeField] private NetworkIdentity identity;
    
    /// <summary>
    ///   <para> 唯一id </para>
    /// </summary>
    [SyncVar] public uint id;
    
    /// <summary>
    ///   <para> 显示在UI上的玩家名字 </para>
    /// </summary>
    [SyncVar] public string name;

    /// <summary>
    ///   <para> 连接上时自动生成id和name </para>
    /// </summary>
    private void Start() {
        SetId();
        MakeName();
        Debug.Log("新Player:" + id);
    }
    
    /// <summary>
    ///   <para> 获取id </para>
    /// </summary>
    public void SetId()
    {
        // 系统自带标识
        id = identity.netId;
    }

    /// <summary>
    ///   <para> 生成name </para>
    /// </summary>
    public void MakeName()
    {
        name = id + "";
    }

}