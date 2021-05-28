using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 玩家列表中的一项 </para>
/// </summary>
public class PlayerItem : MonoBehaviour
{
    public Player player;
    [SerializeField] private Text playerName;
    
    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display(Player _player)
    {
        player = _player;
        playerName.text = player.name;
    }
    
    /// <summary>
    ///   <para> 把玩家踢出房间 </para>
    /// </summary>
    public void KickOut()
    {
        
    }
}