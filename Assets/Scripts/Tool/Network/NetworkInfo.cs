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
    public void AddPlayer() {
        // 生成player对象
        GameObject playerObject = Instantiate(NetworkResource.networkManager.playerPrefab, Vector3.zero, Quaternion.identity);
        playerObject.transform.SetParent(transform);
        Player player = playerObject.GetComponent<Player>();
                
        // 初始化player相关
        // Debug.Log(conn.identity is null);
        player.Identity = null;
        NetworkResource.networkInfo.players.Add(player.id, player);
        Debug.Log("当前玩家数：" + NetworkResource.networkInfo.players.Count);
                
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.New_Client);
    }

    [ClientRpc]
    void RpcAddPlayer() {
        AddPlayer();
    }
}