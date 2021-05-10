using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 游戏状态 </para>
///   <para> 只用于Game场景 </para>
/// </summary>
public class GameState : MonoBehaviour {

    // 回合数
    // 游戏开始时是第1回合
    private int turn = 1;

    // 掷骰子结果
    // 每回合结束需要将rollResult还原为-1
    private int rollResult = -1;

    // 玩家操作形式，默认全部是玩家，默认4人
    // 需要手动初始化
    private Dictionary<PlayerID, PlayerForm> playerForm = new Dictionary<PlayerID, PlayerForm> {
        {PlayerID.Red, PlayerForm.Player},
        {PlayerID.Blue, PlayerForm.Player},
        {PlayerID.Green, PlayerForm.Player},
        {PlayerID.Yellow, PlayerForm.Player},
    };

    // 当前玩家
    // 需要手动初始化
    private PlayerID nowPlayer = PlayerID.None;

    // 本机ID，仅适用于多人联机模式
    // 本地模式下此值为PlayerID.None
    // 需要手动初始化
    private PlayerID myID = PlayerID.None;

    // 当前游戏阶段
    // 默认是Processing，初始化完成后切换为Operating
    private GameStage gameStage = GameStage.Self_Operation_Processing;

    // 赢家
    private PlayerID winner = PlayerID.None;

    // 输家
    private HashSet<PlayerID> losers = new HashSet<PlayerID>();

    /// <summary>
    ///   <para> 角色总数 </para>
    /// </summary>
    public int CharacterNumber() {
        return playerForm.Count;
    }

    /// <summary>
    ///   <para> 回合数 </para>
    /// </summary>
    public int Turn {
        get{return turn;}
        set{
            turn = value;
            GameResource.gameStateSubject.Notify(ModelModifyEvent.Turn);
        }
    }

    /// <summary>
    ///   <para> 掷骰子结果 </para>
    /// </summary>
    public int RollResult {
        get{return rollResult;}
        set{
            rollResult = value;
            GameResource.gameStateSubject.Notify(ModelModifyEvent.Roll_Result);
        }
    }

    /// <summary>
    ///   <para> 设置玩家操作形式 </para>
    /// </summary>
    public void SetPlayerForm(PlayerID playerID, PlayerForm _playerForm) {
        playerForm[playerID] = _playerForm;
    }

    /// <summary>
    ///   <para> 设置玩家操作形式 </para>
    /// </summary>
    public PlayerForm GetPlayerForm(PlayerID playerID) {
        return playerForm[playerID];
    }

    /// <summary>
    ///   <para> 当前玩家 </para>
    /// </summary>
    public PlayerID NowPlayer {
        get {return nowPlayer;}
        set {
            nowPlayer = value;
            GameResource.gameStateSubject.Notify(ModelModifyEvent.Now_Player);
            Debug.Log("开始行动：" + nowPlayer);
        }
    }

    /// <summary>
    ///   <para> 本机ID，仅适用于多人联机模式 </para>
    /// </summary>
    public PlayerID MyID {
        get {return myID;}
        set {
            myID = value;
        }
    }

    /// <summary>
    ///   <para> 当前操作状态 </para>
    /// </summary>
    public GameStage Stage {
        get {return gameStage;}
        set {
            gameStage = value;
            GameResource.gameStateSubject.Notify(ModelModifyEvent.Stage);
        }
    }

    /// <summary>
    ///   <para> 赢家 </para>
    /// </summary>
    public PlayerID Winner {
        get {return winner;}
        set {
            winner = value;
            GameResource.gameStateSubject.Notify(ModelModifyEvent.Winner);
        }
    }

    /// <summary>
    ///   <para> 添加输家 </para>
    /// </summary>
    public void AddLoser(PlayerID loser) {
        losers.Add(loser);
        GameResource.gameStateSubject.Notify(ModelModifyEvent.Loser);
    }

    /// <summary>
    ///   <para> 查询输家 </para>
    /// </summary>
    public bool IsLoser(PlayerID loser) {
        return losers.Contains(loser);
    }
}