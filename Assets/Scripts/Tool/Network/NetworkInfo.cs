using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class NetworkInfo : NetworkBehaviour {
    // 所有id
    public SyncList<uint> ids = new SyncList<uint>();
    // 所有name
    public Dictionary<uint, string> names = new Dictionary<uint, string>();

    private List<string> allName = new List<string>() {
        "小明", "小霞", "小刚", "杰哥", "阿伟", "斌斌"
    };

    [ClientRpc]
    public void RpcAddId() {
        AddId();
    }
    
    [Command]
    public void CmdAddId() {
        AddId();
    }

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    public void AddId() {
        // 初始化所有
        uint id = (uint)new Random().Next(int.MaxValue);
        ids.Add(id);
        names.Add(id, allName[ids.Count]);
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}