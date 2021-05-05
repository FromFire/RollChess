using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 游戏结束后的结算面板 </para>
/// </summary>
public class ReviewPanel : MonoBehaviour {

    // 结算面板
    [SerializeField] private Image reviewPanel;

    // 获胜文本
    [SerializeField] private Text winnerText;

    void Start() {
        // 默认隐藏面板
        reviewPanel.gameObject.SetActive(false);

        // 注册响应更新
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Stage, Show);
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Winner, Show);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    ///   <para> 是Winner变化时 + GameStage变化时 的响应函数 </para>
    /// </summary>
    public void Show() {
        // 仅当游戏结束时弹出
        PlayerID winner = PublicResource.gameState.Winner;
        if(winner == PlayerID.None || PublicResource.gameState.Stage != GameStage.Game_Over)
            return;
        // 显示自身
        reviewPanel.gameObject.SetActive(true);
        string winnerInfo = Transform.ColorString(winner, Transform.PlayerNameOfID[winner]) + " 获胜了！";
        winnerText.text =  winnerInfo;
    }
}