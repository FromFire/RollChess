using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using Random = System.Random;

/// <summary>
///   <para> 玩家 </para>
/// </summary>
public class Player : NetworkBehaviour {
    public NetworkConnection conn;
    [SyncVar(hook = "AddPlayer")] private uint id;
    [SyncVar(hook = "UpdateName")] private string name;
    [SyncVar(hook = "UpdateIsHost")] public bool isHost = false;
    [SyncVar(hook = "UpdatePlayerID")] private int _playerID = (int)PlayerID.None;

    private void Start() {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    ///   <para> 玩家昵称池-没用过的 </para>
    /// </summary>
    public static List<string> namePoolUnused = new List<string>() {
        "暴怒的短吻鳄", "轻快的北极兔", "灵动的藏羚羊", "智慧的灵猴", "燃烧的翼龙",
        "狡诈的变色龙", "矫健的豹猫", "华美的极乐鸟", "剧毒的狼蛛", "轻灵的曙凤蝶",
        "深海的大王乌贼", "潜伏的金环蛇", "美味的秋刀鱼", "掠食的军舰鸟", "洄游的座头鲸"
    };
    
    /// <summary>
    ///   <para> 玩家昵称池-用过的 </para>
    /// </summary>
    public static List<string> namePoolUsed = new List<string>();

    /// <summary>
    /// 移动棋子
    /// </summary>
    public void Move(Vector2Int from, List<Vector2Int> route) {
        CmdMove(from, route.ToArray());
    }

    [Command]
    void CmdMove(Vector2Int from, Vector2Int[] route) {
        RpcMove(from, route);
    }
    
    [ClientRpc]
    void RpcMove(Vector2Int from, Vector2Int[] route) {
        GameResource.gameController.Move(from, new List<Vector2Int>(route));
    }
    
    /// <summary>
    /// Roll点
    /// </summary>
    public void RollDice(int result) {
        CmdRollDice(result);
    }

    [Command]
    void CmdRollDice(int result) {
        RpcRollDice(result);
    }
    
    [ClientRpc]
    void RpcRollDice(int result) {
        GameState.Get().RollResult = result;
    }

    // 同步id后，将自己加入networkInfo
    // 必须保证先同步id，再AddPlayer
    void AddPlayer(uint oldValue, uint newValue) {
        Players.Get().AddPlayer(gameObject);
    }
    
    // 同步name后，推送提醒
    void UpdateName(string oldValue, string newValue) {
        Debug.Log("昵称同步成功，id: " + id + "，昵称：" + newValue);
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Player_Change);
    }
    
    // 同步name后，推送提醒
    void UpdateIsHost(bool oldValue, bool newValue) {
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Player_Change);
    }

    // 同步PlayerID后，推送提醒
    void UpdatePlayerID(int oldValue, int newValue) {
        NetworkResource.networkSubject.Notify(ModelModifyEvent.Client_Player_ID);
    }

    public void SetPlayerID(PlayerID _id) {
        _id = (playerID == _id) ? PlayerID.None : _id;
        CmdSetPlayerID(_id);
    }

    [Command]
    void CmdSetPlayerID(PlayerID _id) {
        // 检查是否与其他player重复
        if (_id != PlayerID.None) {
            List<Player> players = new List<Player>(Players.Get().players.Values);
            foreach (Player player in players)
                if (player.playerID == _id)
                    return;
        }

        playerID = _id;
    }

    public uint Id {
        get { return id; }
        set {
            id = value;
            // 随机起名
            if (name is null || name.Length == 0) {
                name = namePoolUnused[new Random().Next() % namePoolUnused.Count];
                namePoolUnused.Remove(name);
                namePoolUsed.Add(name);
            }
        }
    }

    public string Name {
        get { return name; }
    }

    public PlayerID playerID {
        get { return (PlayerID)_playerID; }
        set { _playerID = (int)value; }
    }

    // 回收昵称
    // 通知NetworkInfo
    private void OnDestroy() {
        namePoolUnused.Add(name);
        namePoolUsed.Remove(name);
        Players.Get().RemovePlayer(id);
    }
}