using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> 地图选择中的一项 </para>
/// </summary>
public class EntranceController : MonoBehaviour {

    void Start() {
        SaveResource.saveManager.LoadAllSave();
    }

    /// <summary>
    ///   <para> 选择地图 </para>
    /// </summary>
    public void Choose(string filename) {
        EntranceResource.mapChooseState.MapFileName = filename;
    }

    /// <summary>
    ///   <para> 修改操作方式 </para>
    /// </summary>
    public void ShiftPlayerFrom(PlayerID id) {
        // 轮换到下一个
        PlayerForm form = EntranceResource.mapChooseState.GetPlayerForm(id);
        int count = System.Enum.GetNames(typeof(PlayerForm)).Length;
        form = (PlayerForm)( ((int)form+1) % count );
        EntranceResource.mapChooseState.SetPlayerForm(id, form);
        Debug.Log(id + ": " + form);
    }

    /// <summary>
    ///   <para> 选择地图 </para>
    /// </summary>
    public void StartGame() {
        // 获取players
        List<PlayerForm> player = new List<PlayerForm> {
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Red), 
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Blue), 
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Yellow), 
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Green)
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
        int minPlayer = EntranceResource.mapChooseState.PlayerLimit.min;
        if(totalPlayers < minPlayer) {
            WarningManager.errors.Add(new WarningModel("角色数量最少为" + minPlayer + "人！"));
            return;
        }
        // 错误：若玩家数为0
        if(!haveHuman) {
            WarningManager.errors.Add(new WarningModel("至少有一个玩家参与游戏！"));
            return;
        }

        //获取maps
        string mapName = EntranceResource.mapChooseState.MapFileName;

        //地图不能为空
        if(mapName.Length <= 1) {
            WarningManager.errors.Add(new WarningModel("请选择地图！"));
            return;
        }

        //符合要求，进入游戏
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("Game");
    }
}