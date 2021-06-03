using System.Collections.Generic;
using UnityEngine;

public class PlayerOperationController : MonoBehaviour {
    // 单例
    static PlayerOperationController singleton;
    // 获取单例
    public static PlayerOperationController Get() { return singleton; }

    void Start() {
        singleton = this;
    }

    /// <summary>
    ///   <para> 判断玩家是否合法 </para>
    /// </summary>
    public bool IsPlayerValid() {
        // 获取players
        List<PlayerForm> player = new List<PlayerForm> {
            MapChooseState.Get().GetPlayerForm(PlayerID.Red), 
            MapChooseState.Get().GetPlayerForm(PlayerID.Blue), 
            MapChooseState.Get().GetPlayerForm(PlayerID.Yellow), 
            MapChooseState.Get().GetPlayerForm(PlayerID.Green)
        };

        // 检查players的合法性
        // 4个player至少有1个由玩家操控，且player最少为2人
        int totalPlayers = 0;
        bool haveHuman = false;
        foreach(PlayerForm choice in player) {
            if(choice != PlayerForm.Banned) {
                totalPlayers ++;
            }
            if(choice == PlayerForm.Player) {
                haveHuman = true;
            }
        }
        // 错误：若players人数低于最小人数限制
        int minPlayer = MapChooseState.Get().PlayerLimit.min;
        if(totalPlayers < minPlayer) {
            WarningManager.errors.Add(new WarningModel("角色数量最少为" + minPlayer + "人！"));
            return false;
        }
        // 错误：若玩家数为0
        if(!haveHuman) {
            WarningManager.errors.Add(new WarningModel("至少有一个玩家参与游戏！"));
            return false;
        }

        return true;
    }
    
    /// <summary>
    ///   <para> 修改操作方式 </para>
    /// </summary>
    public void ShiftPlayerFrom(PlayerID id) {
        // 轮换到下一个
        PlayerForm form = MapChooseState.Get().GetPlayerForm(id);
        int count = System.Enum.GetNames(typeof(PlayerForm)).Length;
        form = (PlayerForm)( ((int)form+1) % count );
        MapChooseState.Get().SetPlayerForm(id, form);
        Debug.Log(id + ": " + form);
    }
}