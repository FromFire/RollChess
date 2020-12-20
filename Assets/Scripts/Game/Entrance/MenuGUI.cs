using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour
{
    //负责页面左右滚动的控制
    public Scroller scroller;

    //选择玩家
    public ChoosePlayer choosePlayer;

    //切换到选关页面按钮
    public Button chooseLevelButton;
    //返回主菜单按钮
    public Button returnMainMenuButton;
    
    //打开关卡编辑器按钮
    public Button startMapEditButton;

    // Start is called before the first frame update
    void Start()
    {
        //初始化界面切换按钮
		chooseLevelButton.onClick.AddListener(ToChooseLevel);
        returnMainMenuButton.onClick.AddListener(ToMainMenu);

        //初始化打开关卡编辑器按钮
		startMapEditButton.onClick.AddListener(StartMapEdit);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //滑动到选关界面，是chooseLevelButton的点击响应函数
    public void ToChooseLevel() {
        scroller.SlideTo(1f);
    }

    //滑动到主菜单，是returnMainMenuButton的点击响应函数
    public void ToMainMenu() {
        scroller.SlideTo(0f);
    }

    //开始关卡编辑器，是startMapEditButton的点击响应函数
    public void StartMapEdit() {
        SceneManager.LoadScene("MapEditor");
    }
}
