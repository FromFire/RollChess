using System.Collections;
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
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Stage, ShowRollButton);
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Roll_Result, ShowRollStep);
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Turn, UpdateTurn);
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Now_Player, PlayerOperating);
    }

    /// <summary>
    ///   <para> 显示roll点按钮，隐藏步数 </para>
    ///   <para> 是gameStage修改的响应函数，仅当该本机操作时生效 </para>
    /// </summary>
    public void ShowRollButton() {
        if(PublicResource.gameState.Stage != GameStage.Self_Operating)
            return;
        rollButton.gameObject.SetActive(true);
        rollNumber.text = "";
    }

    /// <summary>
    ///   <para> 隐藏roll点按钮，显示步数 </para>
    ///   <para> 是RollResult修改的响应函数，仅当RollResult不为0时生效 </para>
    /// </summary>
    public void ShowRollStep() {
        if(PublicResource.gameState.RollResult == 0)
            return;
        rollButton.gameObject.SetActive(false);
        rollNumber.text = PublicResource.gameState.RollResult + "步";
    }

    /// <summary>
    ///   <para> 更新回合数 </para>
    ///   <para> 是Turn修改的响应函数 </para>
    /// </summary>
    public void UpdateTurn() {
        turnCount.text = "第 " + PublicResource.gameState.Turn + " 回合";
    }

    /// <summary>
    ///   <para> 更新正在行动提示 </para>
    ///   <para> 是nowPlayer修改的响应函数，仅当该某玩家操作时生效 </para>
    /// </summary>
    public void PlayerOperating() {
        // 排除游戏结束
        if(PublicResource.gameState.Stage == GameStage.Game_Over)
            return;
        // 显示正在行动的玩家
        PlayerID nowPlayer = PublicResource.gameState.NowPlayer;
        string str = Transform.ColorString(nowPlayer, Transform.PlayerNameOfID[nowPlayer]) + " 行动中";
        actionPlayer.text = str;
    }
}