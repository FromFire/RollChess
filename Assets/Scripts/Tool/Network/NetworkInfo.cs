using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Mirror;
using UnityEngine;
using Random = System.Random;

public class NetworkInfo : NetworkBehaviour {
    // 所有player
    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    public void AddPlayer(GameObject playerObject) {
        Player player = playerObject.GetComponent<Player>();
        players.Add(player.Id, player);
        Debug.Log("新player：(id:" + player.Id + ", name:" + player.name + ")");
        
        // 同步所有player
        NetworkResource.networkInfo.SyncPlayers();
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
    
    /// <summary>
    ///   <para> 同步players </para>
    /// </summary>
    public void SyncPlayers() {
        Player[] _players = FindObjectsOfType<Player>();
        foreach (Player player in _players) {
            if(!players.ContainsKey(player.Id))
                players.Add(player.Id, player);
        }
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}