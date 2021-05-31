using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class NetworkInfo : NetworkBehaviour {
    // 所有player
    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    [ClientRpc]
    public void RpcAddPlayer(Player player) {
        players.Add(player.Id, player);
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}