using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkInfo : NetworkBehaviour {
    // 所有player
    public Dictionary<uint, Player> players;

    public void Start() {
        players = new Dictionary<uint, Player>();
    }

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    [Command]
    public void CmdAddPlayer(Player player) {
        players.Add(player.id, player);
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}