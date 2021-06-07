using System;
using Mirror;
using UnityEngine;

public class MultiPlay : NetworkBehaviour {
    // 单例
    static MultiPlay singleton;
    // 获取单例
    public static MultiPlay Get() { return singleton; }

    private void Awake() {
        singleton = this;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    [ClientRpc]
    public void RpcStartGame() {
        EntranceController.Get().StartSingleGame();    
    }
}