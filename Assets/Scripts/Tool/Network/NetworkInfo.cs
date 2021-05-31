using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class NetworkInfo : NetworkBehaviour {
    // 所有player
    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();
    
    private List<string> allName = new List<string>() {
        "小明", "小霞", "小刚", "杰哥", "阿伟", "斌斌"
    };

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    [ClientRpc]
    public void RpcAddPlayer(Player player) {
        players.Add(player.id, player);
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}