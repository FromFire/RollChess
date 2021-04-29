using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 游戏状态 </para>
/// </summary>
public class GameState {

    // 回合数
    private int turn;

    // 掷骰子结果
    private int rollResult;

    // 玩家操作形式
    private List<PlayerForm> playerForm;

    // 当前玩家
    private PlayerID nowPlayer;

    // 当前操作状态
    private OperationState opState;

    // 游戏是否结束
    private bool isGameOver;

    // 赢家
    private PlayerID winner;

    /// <summary>
    ///   <para> 玩家操作状态，用处是限制玩家操作 </para>
    /// </summary>
    public enum OperationState {
        Waiting,    //等待玩家操作
        Moved       //玩家操作完毕，等待处理
    }

    /// <summary>
    ///   <para> 回合数 </para>
    /// </summary>
    public int Turn {
        get{return turn;}
        set{
            turn = value;
            PublicResource.gameStateSubject.Notify(ModelModifyEvent.Turn);
        }
    }

    /// <summary>
    ///   <para> 掷骰子结果 </para>
    /// </summary>
    public int RollResult {
        get{return rollResult;}
        set{
            rollResult = value;
            PublicResource.gameStateSubject.Notify(ModelModifyEvent.Roll_Result);
        }
    }

    /// <summary>
    ///   <para> 设置玩家操作形式 </para>
    /// </summary>
    public void SetPlayerForm(PlayerID playerID, PlayerForm _playerForm) {
        playerForm[(int)playerID] = _playerForm;
    }

    /// <summary>
    ///   <para> 设置玩家操作形式 </para>
    /// </summary>
    public PlayerForm GetPlayerForm(PlayerID playerID) {
        return playerForm[(int)playerID];
    }

    /// <summary>
    ///   <para> 当前玩家 </para>
    /// </summary>
    public PlayerID NowPlayer {
        get {return nowPlayer;}
        set {
            nowPlayer = value;
            PublicResource.gameStateSubject.Notify(ModelModifyEvent.Now_Player);
        }
    }

    /// <summary>
    ///   <para> 当前操作状态 </para>
    /// </summary>
    public OperationState OpState {
        get {return opState;}
        set {
            opState = value;
            PublicResource.gameStateSubject.Notify(ModelModifyEvent.Operation_State);
        }
    }

    /// <summary>
    ///   <para> 游戏是否结束 </para>
    /// </summary>
    public bool IsGameOver {
        get {return isGameOver;}
        set {
            isGameOver = value;
            PublicResource.gameStateSubject.Notify(ModelModifyEvent.Is_Game_Over);
        }
    }

    /// <summary>
    ///   <para> 赢家 </para>
    /// </summary>
    public PlayerID Winner {
        get {return winner;}
        set {
            winner = value;
            PublicResource.gameStateSubject.Notify(ModelModifyEvent.Winner);
        }
    }
}