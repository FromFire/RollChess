using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 玩家列表中的一项 </para>
/// </summary>
public class PlayerItem : MonoBehaviour {
    private uint id;
    // 昵称
    [SerializeField] private Text playerName;
    // 房主图标
    [SerializeField] private Image crown;
    // 踢人按钮
    [SerializeField] private Button kickOut;

    /// <summary>
    ///   <para> 把玩家踢出房间 </para>
    /// </summary>
    public void KickOut()
    {
        EntranceResource.entranceController.KickOut(id);
    }

    public uint Id {
        get { return id; }
        set {
            id = value;
            Player player = NetworkResource.networkInfo.players[id];
            playerName.text = player.Name;
            // 高亮自己
            if(player.isLocalPlayer)
                playerName.color = Color.red;
            // 显示房主图标
            crown.gameObject.SetActive(player.isHost);
            // 踢人按钮禁用
            kickOut.interactable = (NetworkResource.networkInfo.isServer && !player.isHost);
        }
    }
}