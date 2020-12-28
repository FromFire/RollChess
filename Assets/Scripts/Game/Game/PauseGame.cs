using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    //暂停按钮
    public Button pauseButton;

    //暂停Panel
    public Image pausePanel;

    //返回游戏按钮
    public Button exitGameButton;

    //退出游戏按钮
    public Button backGameButton;

    //显示网格按钮
    public Button switchGrid;

    //网格
    public HexGridDisplay hexGridDisplay;

    // Start is called before the first frame update
    void Start()
    {
        HidePausePanel();
        pauseButton.onClick.AddListener(ShowPausePanel);
        backGameButton.onClick.AddListener(HidePausePanel);
        exitGameButton.onClick.AddListener(BackMainMenu);
        switchGrid.onClick.AddListener(ShowGrid);
    }

    //显示网格
    public void ShowGrid() {
        if(hexGridDisplay.ShowHexGrid == true) {
            hexGridDisplay.ShowHexGrid = false;
        } else {
            hexGridDisplay.ShowHexGrid = true;
        }
    }

    //弹出暂停框
    public void ShowPausePanel() {
        pausePanel.gameObject.SetActive(true);
    }

    //隐藏暂停框
    public void HidePausePanel() {
        pausePanel.gameObject.SetActive(false);
    }

    //退出游戏
    public void BackMainMenu() {
        SceneManager.LoadScene("Entrance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
