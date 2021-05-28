using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NetworkInfo : NetworkBehaviour {
    // 所有player
    public Dictionary<uint, Player> players;
    
    [SerializeField] public NetworkIdentity identity;

    public void Start() {
        players = new Dictionary<uint, Player>();
    }

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    [ClientRpc]
    public void RpcAddPlayer() {
        Debug.Log("rpc" + players.Count);
        
        // 生成player对象
        GameObject playerObject = Instantiate(NetworkResource.networkManager.playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.transform.SetParent(transform);
        Player player = playerObject.GetComponent<Player>();
        players.Add(player.id, player);
        
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }
}