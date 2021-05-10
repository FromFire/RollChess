using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorUI : MonoBehaviour
{
    // Start is called before the first frame update

    // 显示/隐藏使用说明页面
    public Button showIntroduction;
    public Button hideIntroduction;

    // 说明页面
    public Image introductionPanel;

    // 退出地图编辑器
    public Button backMainMenu;

    void Start()
    {
        HideIntroduction();
        showIntroduction.onClick.AddListener(ShowIntroduction);
        hideIntroduction.onClick.AddListener(HideIntroduction);
        backMainMenu.onClick.AddListener(BackMainMenu);
    }

    // 显示使用说明页面
    public void ShowIntroduction() {
        introductionPanel.gameObject.SetActive(true);
    }

    // 隐藏使用说明页面
    public void HideIntroduction() {
        introductionPanel.gameObject.SetActive(false);
    }

    // 退出地图编辑器
    public void BackMainMenu() {
        SceneManager.LoadScene("Entrance");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
