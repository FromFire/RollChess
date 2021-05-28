using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkInfo : NetworkBehaviour {
    // 玩家列表
    [SyncVar] public List<Player> players;

    public void Start() {
        players = new List<Player>();
    }

    /// <summary>
    ///   <para> 添加Player </para>
    /// </summary>
    [Command]
    public void CmdAddPlayer(Player player) {
        players.Add(player);
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}