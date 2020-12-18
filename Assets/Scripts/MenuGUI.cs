using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour
{
    //所有模块都位于此路径中
    string path = "Canvas/ScrollView/Viewport/Content";

    //作为根目录的ScrollView
    ScrollRect scrollRect;

    //切换到选关页面按钮
    Button chooseLevelButton;
    //返回主菜单按钮
    Button returnMainMenuButton;
    
    //打开关卡编辑器按钮
    Button startMapEditButton;

    //是否需要滑动页面
    bool needSlide = false;
    //每次滑动的距离
    float slideDistanceEachFrame;
    //滑动所需帧数
    int slideRestFrames;

    // Start is called before the first frame update
    void Start()
    {
        //初始化scrollView
        scrollRect = GameObject.Find("Canvas/ScrollView").GetComponent<ScrollRect> ();

        //初始化界面切换按钮
        chooseLevelButton = GameObject.Find(path+"/MainMenu/ChooseLevelButton").GetComponent<Button> ();
		chooseLevelButton.onClick.AddListener(ToChooseLevel);
        returnMainMenuButton = GameObject.Find(path+"/LevelChoose/ReturnMainMenuButton").GetComponent<Button> ();
        returnMainMenuButton.onClick.AddListener(ToMainMenu);

        //初始化打开关卡编辑器按钮
        startMapEditButton = GameObject.Find(path+"/MainMenu/StartMapEditButton").GetComponent<Button> ();
		startMapEditButton.onClick.AddListener(StartMapEdit);

        //初始化
    }

    // Update is called once per frame
    void Update()
    {
        //滑动效果
        if(needSlide) {
            SlidePerFrame();
        }
    }

    //滑动到选关界面，是chooseLevelButton的点击响应函数
    public void ToChooseLevel() {
        SlideTo(1f);
    }

    //滑动到主菜单，是returnMainMenuButton的点击响应函数
    public void ToMainMenu() {
        SlideTo(0f);
    }

    //开始关卡编辑器，是startMapEditButton的点击响应函数
    public void StartMapEdit() {
        SceneManager.LoadScene("MapEdit");
    }

    //ScrollView滑动到指定位置
    public void SlideTo(float position) {
        needSlide = true;
        slideRestFrames = 60;
        slideDistanceEachFrame = (position - scrollRect.horizontalNormalizedPosition) / slideRestFrames;
    }

    //滑动时每帧的处理函数
    public void SlidePerFrame() {
        if(slideRestFrames >= 1) {
            slideRestFrames -= 1;
            scrollRect.horizontalNormalizedPosition += slideDistanceEachFrame;
        } else {
            needSlide = false;
        }
    }

}
