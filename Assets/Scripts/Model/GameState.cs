using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 游戏状态 </para>
/// </summary>
public class GameState {

    /// <summary>
    ///   <para> 回合数 </para>
    /// </summary>
    public int turn {get;}

    /// <summary>
    ///   <para> 掷骰子结果 </para>
    /// </summary>
    public int rollResult {get;}

    /// <summary>
    ///   <para> 玩家操作形式 </para>
    /// </summary>
    public PlayerForm playerForm;

    /// <summary>
    ///   <para> 当前玩家 </para>
    /// </summary>
    public PlayerID nowPlayer {get;}

    /// <summary>
    ///   <para> 当前操作状态 </para>
    /// </summary>
    public OperationState operationState;

    /// <summary>
    ///   <para> 游戏是否结束 </para>
    /// </summary>
    public bool isGameOver;

    /// <summary>
    ///   <para> 赢家 </para>
    /// </summary>
    public PlayerID winner;

    /// <summary>
    ///   <para> 更新推送 </para>
    /// </summary>
    public PositionSubject subject;

    // todo: subject 更新推送

    /// <summary>
    ///   <para> 玩家操作状态 </para>
    /// </summary>
    public enum OperationState {
        Waiting,    //等待玩家操作
        Moved       //玩家操作完毕，等待处理
    }
}