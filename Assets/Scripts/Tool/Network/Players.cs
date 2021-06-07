using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Players : NetworkBehaviour {
    // 单例
    static Players singleton;
    // 获取单例
    public static Players Get() { return singleton; }
    
    // 所有player
    public Dictionary<uint, Player> players = new Dictionary<uint, Player>();

    private void Awake() {
        singleton = this;
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 获取本地player
    /// </summary>
    public Player LocalPlayer() {
        foreach (KeyValuePair<uint,Player> kvp in players)
            if (kvp.Value.isLocalPlayer)
                return kvp.Value;
        return null;
    }

    /// <summary>
    ///   <para> 添加连接 </para>
    /// </summary>
    public void AddPlayer(GameObject playerObject) {
        Player player = playerObject.GetComponent<Player>();
        players.Add(player.Id, player);
        Debug.Log("新player：(id:" + player.Id + ", name:" + player.name + ")");
                
        // 同步所有player
        SyncPlayers();
    }
            
    /// <summary>
    ///   <para> 同步players </para>
    /// </summary>
    void SyncPlayers() {
        Player[] _players = FindObjectsOfType<Player>();
        foreach (Player player in _players) {
            if(!players.ContainsKey(player.Id))
                players.Add(player.Id, player);
        }
    }
            
    /// <summary>
    ///   <para> 移除Player </para>
    /// </summary>
    [ClientRpc]
    public void RpcRemovePlayer(uint id) {
        RemovePlayer(id);
    }
            
    /// <summary>
    ///   <para> 移除Player </para>
    /// </summary>
    public void RemovePlayer(uint id) {
        players.Remove(id);
                
        // 推送
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Player_Change);
    }
}