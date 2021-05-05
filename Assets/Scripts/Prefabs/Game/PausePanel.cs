using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> 游戏的暂停面板</para>
/// </summary>
public class PausePanel : MonoBehaviour
{
    //暂停按钮
    [SerializeField] private Button pauseButton;

    //暂停Panel
    [SerializeField] private Image pausePanel;

    //退出游戏按钮
    [SerializeField] private Button exitGameButton;

    //返回游戏按钮
    [SerializeField] private Button backGameButton;

    //显示网格开关
    [SerializeField] private Toggle switchHexGridVisible;

    //网格
    [SerializeField] private HexGridDisplay hexGridDisplay;

    void Start()
    {
        // 默认隐藏
        Hide();
    }

    /// <summary>
    ///   <para> 切换网格显示状态 </para>
    ///   <para> 是switchHexGridVisible的响应函数 </para>
    /// </summary>
    public void SwitchHexGridVisiblity(bool isOn) {
        hexGridDisplay.Visible = isOn;
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    ///   <para> 是pauseButton的响应函数 </para>
    /// </summary>
    public void Show() {
        pausePanel.gameObject.SetActive(true);
    }

    /// <summary>
    ///   <para> 隐藏自身 </para>
    ///   <para> 是backGameButton的响应函数 </para>
    /// </summary>
    public void Hide() {
        pausePanel.gameObject.SetActive(false);
    }

    /// <summary>
    ///   <para> 返回主菜单 </para>
    ///   <para> 是exitGameButton的响应函数 </para>
    /// </summary>
    public void BackMainMenu() {
        SceneManager.LoadScene("Entrance");
    }
}
