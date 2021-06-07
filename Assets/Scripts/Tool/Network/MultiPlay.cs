using System;
using Mirror;
using UnityEngine;

public class MultiPlay : NetworkBehaviour {
    // 单例
    static public MultiPlay singleton;

    private void Awake() {
        singleton = this;
    }
    
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame() {
        RpcStartGame();
    }

    // 开始游戏
    [ClientRpc]
    void RpcStartGame() {
        EntranceController.Get().StartGame();
    }
}