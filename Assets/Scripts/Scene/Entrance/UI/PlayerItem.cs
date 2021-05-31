using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 玩家列表中的一项 </para>
/// </summary>
public class PlayerItem : MonoBehaviour {
    private uint id;
    [SerializeField] private Text playerName;

    /// <summary>
    ///   <para> 把玩家踢出房间 </para>
    /// </summary>
    public void KickOut()
    {
        
    }

    public uint Id {
        get { return id; }
        set {
            id = value;
            playerName.text = NetworkResource.networkInfo.players[id].Name;
        }
    }
}