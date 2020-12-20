using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour
{
    //作为根目录的ScrollView
    public ScrollRect scrollRect;

    //切换到选关页面按钮
    public Button chooseLevelButton;
    //返回主菜单按钮
    public Button returnMainMenuButton;
    
    //打开关卡编辑器按钮
    public Button startMapEditButton;

    //是否需要滑动页面
    bool needSlide = false;
    //每次滑动的距离
    float slideDistanceEachFrame;
    //滑动所需帧数
    int slideRestFrames;

    // 四个玩家选择栏
    public Button player1, player2, player3, player4;
    Button[] playerButtons;
    // 选择栏的三个图标
    Sprite[] playerSprites;
    // 玩家操控方式选项
    enum PlayerChoices{Player, Comuputer, Banned};
    // 当前的玩家操纵方式
    PlayerChoices[] nowPlayerChoices;

    // Start is called before the first frame update
    void Start()
    {
        //初始化界面切换按钮
		chooseLevelButton.onClick.AddListener(ToChooseLevel);
        returnMainMenuButton.onClick.AddListener(ToMainMenu);

        //初始化打开关卡编辑器按钮
		startMapEditButton.onClick.AddListener(StartMapEdit);

        //初始化玩家选择相关
        playerButtons = new Button[4] {player1, player2, player3, player4};
        for(int i=0; i<playerButtons.Length; i++) {
            int arg = i;
            playerButtons[i].onClick.AddListener(()=> {changePlayer(arg);} );
        }
        playerSprites = new Sprite[3] {
            Resources.Load<Sprite>("Sprites/Player"),
            Resources.Load<Sprite>("Sprites/Computer"),
            Resources.Load<Sprite>("Sprites/Banned"),
        };
        nowPlayerChoices = new PlayerChoices[4]{PlayerChoices.Player, PlayerChoices.Player, PlayerChoices.Player, PlayerChoices.Player};
    }

    // Update is called once per frame
    void Update()
    {
        //滑动效果
        if(needSlide) {
            SlidePerFrame();
        }
    }

    // 切换第index位玩家的操作方式并显示
    public void changePlayer(int index) {
        Image playerImage = playerButtons[index].gameObject.transform.GetChild(0).GetComponent<Image>();
        PlayerChoices nextChoice = nextPlayerChoice(nowPlayerChoices[index]);
        playerImage.sprite = playerSprites[(int)nextChoice];
        nowPlayerChoices[index] = nextChoice;
    }

    // 获取下一个玩家操作选项
    PlayerChoices nextPlayerChoice(PlayerChoices choice) {
        return (PlayerChoices)(((int)choice + 1) % 3);
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
        SceneManager.LoadScene("MapEditor");
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
