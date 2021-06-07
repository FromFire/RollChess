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
        StartGame();
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
        
        // 玩家人数必须达标
        if (Players.Get().players.Count != MapChooseState.Get().AvaliablePlayerNumber())
            return false;

        // 玩家必须全部选完角色
        foreach (KeyValuePair<uint,Player> kvp in Players.Get().players)
            if (kvp.Value.playerID == PlayerID.None)
                return false;

        return true;
    }

    /// <summary>
    ///   <para> 判断是否可以开始游戏 </para>
    /// </summary>
    public bool isGameAvalible() {
        Debug.Log(PlayerOperationController.Get().IsPlayerValid());
        Debug.Log(MapOperationController.Get().IsMapValid());
        if (!PlayerOperationController.Get().IsPlayerValid() ||
            !MapOperationController.Get().IsMapValid())
            return false;
        return true;
    }
}