﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 游戏进行时的抬头显示 </para>
/// </summary>
public class HUD : MonoBehaviour {

    // roll点按钮
    [SerializeField] private Button rollButton;

    // roll出的数值
    [SerializeField] private Text rollNumber;

    // 回合数
    [SerializeField] private Text turnCount;

    // 正在行动提示
    [SerializeField] private Text actionPlayer;

    void Start() {
        // 注册响应更新
        GameResource.gameStateSubject.Attach(ModelModifyEvent.Stage, ShowRollButton);
        GameResource.gameStateSubject.Attach(ModelModifyEvent.Roll_Result, ShowRollStep);
        GameResource.gameStateSubject.Attach(ModelModifyEvent.Turn, UpdateTurn);
        GameResource.gameStateSubject.Attach(ModelModifyEvent.Now_Player, PlayerOperating);
        ShowRollButton();
    }

    /// <summary>
    ///   <para> 显示roll点按钮，隐藏步数 </para>
    ///   <para> 是gameStage修改的响应函数，仅当该本机操作时生效 </para>
    /// </summary>
   void ShowRollButton() {
        if (GameState.Get().Stage != GameStage.Self_Operating) {
            rollButton.gameObject.SetActive(false);
            return;
        }
        rollButton.gameObject.SetActive(true);
        rollNumber.text = "";
    }

    /// <summary>
    ///   <para> 隐藏roll点按钮，显示步数 </para>
    ///   <para> 是RollResult修改的响应函数，仅当RollResult不为0时生效 </para>
    /// </summary>
    void ShowRollStep() {
        if (GameState.Get().RollResult == 0) {
            rollNumber.text = "";
            return;
        }
        rollButton.gameObject.SetActive(false);
        rollNumber.text = GameState.Get().RollResult + "步";
    }

    /// <summary>
    ///   <para> 更新回合数 </para>
    ///   <para> 是Turn修改的响应函数 </para>
    /// </summary>
    void UpdateTurn() {
        turnCount.text = "第 " + GameState.Get().Turn + " 回合";
    }

    /// <summary>
    ///   <para> 更新正在行动提示 </para>
    ///   <para> 是nowPlayer修改的响应函数，仅当该某玩家操作时生效 </para>
    /// </summary>
    void PlayerOperating() {
        // 排除游戏结束
        if(GameState.Get().Stage == GameStage.Game_Over)
            return;
        // 显示正在行动的玩家
        PlayerID nowPlayer = GameState.Get().NowPlayer;
        string str = Transform.ColorString(nowPlayer, Transform.PlayerNameOfID[nowPlayer]) + " 行动中";
        actionPlayer.text = str;
    }
}