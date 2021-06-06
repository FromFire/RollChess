using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 玩家列表中的一项 </para>
/// </summary>
public class PlayerItem : MonoBehaviour {
    // 表示的玩家的id
    private uint id;
    // 昵称
    [SerializeField] private Text playerName;
    // 房主图标
    [SerializeField] private Image crown;
    // 踢人按钮
    [SerializeField] private Button kickOut;
    // 棋子图标
    [SerializeField] private Image token;
    // 棋子图标素材
    [SerializeField] private List<Sprite> tokenSprites;

    private void Start() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Client_Player_ID, UpdatePlayerToken);
    }

    // 用来防止触发prefab的响应函数
    private void OnDisable() {
        NetworkResource.networkSubject.Detach(ModelModifyEvent.Client_Player_ID, UpdatePlayerToken);
    }

    /// <summary>
    ///   <para> 把玩家踢出房间 </para>
    /// </summary>
    public void KickOut() {
        EntranceController.Get().KickOut(id);
    }
    
    // 设置选择的角色
    void UpdatePlayerToken() {
        Debug.Log(id);
        foreach (KeyValuePair<uint,Player> kvp in Players.Get().players) {
            Debug.Log(kvp.Key);
        }
        PlayerID playerID = Players.Get().players[id].playerID;
        if(playerID == PlayerID.None)
            token.gameObject.SetActive(false);
        else {
            token.gameObject.SetActive(true);
            token.sprite = tokenSprites[(int)playerID];
        }
    }
    
    // 更新自身显示
    private void UpdateSelf() {
        Player player = Players.Get().players[id];
        playerName.text = player.Name;
        // 高亮自己
        if(player.isLocalPlayer)
            playerName.color = Color.red;
        // 显示房主图标
        crown.gameObject.SetActive(player.isHost);
        // 踢人按钮禁用
        kickOut.interactable = (Players.Get().isServer && !player.isHost);
    }

    public uint Id {
        get { return id; }
        set {
            id = value;
            Debug.Log(id);
            UpdateSelf();
        }
    }
}