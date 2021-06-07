using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> Entrance整体管理 </para>
/// </summary>
public class EntranceController : MonoBehaviour {
    // 单例
    static EntranceController singleton;
    // 获取单例
    public static EntranceController Get() { return singleton; }

    void Start() {
        singleton = this;
        SaveResource.saveManager.LoadAllSave();
    }

    /// <summary>
    /// 开始多人游戏
    /// </summary>
    public void StartMultipleGame() {
        if (!isMutipleGameAvalible()) return;
        MultiPlay.singleton.StartGame();
    }

    /// <summary>
    ///   <para> 开始单人游戏 </para>
    /// </summary>
    public void StartSingleGame() {
        if (!isGameAvalible()) return;
        StartGame();
    }

    /// <summary>
    ///   <para> 开始游戏 </para>
    /// </summary>
    public void StartGame() {
        DontDestroyOnLoad(MapChooseState.Get().gameObject);
        SceneManager.LoadScene("Game");
    }
    
    /// <summary>
    ///   <para> 判断是否可以开始多人游戏 </para>
    /// </summary>
    bool isMutipleGameAvalible() {
        if (!isGameAvalible())
            return false;
        Debug.Log("单人游戏条件满足");

        // // 玩家人数必须达标
        // if (Players.Get().players.Count != MapChooseState.Get().AvaliablePlayerNumber())
        //     return false;
        // Debug.Log("玩家人数满足");
        
        // 玩家必须全部选完角色
        foreach (KeyValuePair<uint,Player> kvp in Players.Get().players)
            if (kvp.Value.playerID == PlayerID.None)
                return false;
        Debug.Log("角色选择满足");

        return true;
    }

    /// <summary>
    ///   <para> 判断是否可以开始游戏 </para>
    /// </summary>
    public bool isGameAvalible() {
        if (!PlayerOperationController.Get().IsPlayerValid() ||
            !MapOperationController.Get().IsMapValid())
            return false;
        return true;
    }
}