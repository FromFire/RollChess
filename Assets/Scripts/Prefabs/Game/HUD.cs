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

    /// <summary>
    ///   <para> 显示roll点按钮，隐藏步数 </para>
    /// </summary>
    public void ShowRollButton() {
        rollButton.gameObject.SetActive(true);
        rollNumber.text = "";
    }

    /// <summary>
    ///   <para> 隐藏roll点按钮，显示步数 </para>
    /// </summary>
    public void ShowRollStep(int step) {
        rollButton.gameObject.SetActive(false);
        rollNumber.text = step + "步";
    }

    /// <summary>
    ///   <para> 更新回合数 </para>
    /// </summary>
    public void UpdateTurn(int turn) {
        turnCount.text = "第 " + turn + " 回合";
    }

    /// <summary>
    ///   <para> 更新正在行动提示 </para>
    /// </summary>
    public void UpdateActionPlayer(int player) {
        string str = GetColorfulTokenString((PlayerColor) player);
        str += "行动中";
        actionPlayer.text = str;
    }
}