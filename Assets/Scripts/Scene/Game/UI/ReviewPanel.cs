using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        GameResource.gameStateSubject.Attach(ModelModifyEvent.Stage, Show);
        GameResource.gameStateSubject.Attach(ModelModifyEvent.Winner, Show);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    ///   <para> 是Winner变化时 + GameStage变化时 的响应函数 </para>
    /// </summary>
    public void Show() {
        // 仅当游戏结束时弹出
        PlayerID winner = GameState.Get().Winner;
        if(winner == PlayerID.None || GameState.Get().Stage != GameStage.Game_Over)
            return;
        // 显示自身
        reviewPanel.gameObject.SetActive(true);
        string winnerInfo = Transform.ColorString(winner, Transform.PlayerNameOfID[winner]) + " 获胜了！";
        winnerText.text =  winnerInfo;
    }
    
    /// <summary>
    ///   <para> 返回主菜单 </para>
    /// </summary>
    public void BackMainMenu() {
        SceneManager.LoadScene("Entrance");
    }
}