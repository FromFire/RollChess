using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour
{
    //负责页面左右滚动的控制
    public Scroller scroller;

    // //选择玩家
    // public ChooseCharacter chooseCharacter;

    // //选择地图
    // public ChooseMap chooseMap;

    // //切换到选关页面按钮
    // public Button chooseLevelButton;
    // //返回主菜单按钮
    // public Button returnMainMenuButton;
    
    // //打开关卡编辑器按钮
    // public Button startMapEditButton;

    // //开始游戏按钮
    // public Button startGameButton;

    // // 传递给Game的消息
    // public Message message;

    // // 玩家当前选择的地图
    // public BoardEntity currentMap;
    // // 玩家当前选择的地图的文件名
    // string mapFilename;

    // Start is called before the first frame update
    void Start()
    {
        // //初始化界面切换按钮
		// chooseLevelButton.onClick.AddListener(ToChooseLevel);
        // returnMainMenuButton.onClick.AddListener(ToMainMenu);

        // //初始化打开关卡编辑器按钮
		// startMapEditButton.onClick.AddListener(StartMapEdit);

        // //初始化打开游戏按钮
        // startGameButton.onClick.AddListener(StartGame);
        ToChooseLevel();
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    // /// <summary>
    // /// <para> 启动游戏，是startGameButton的点击响应函数 </para>
    // /// </summary>
    // public void StartGame() {
    //     // 获取players
    //     List<PlayerChoices> player = choosePlayer.GetPlayers();

    //     // 检查players的合法性
    //     // 4个player至少有1个由玩家操控，且player最少为2人
    //     int totalPlayers = 0;
    //     bool haveHuman = false;
    //     foreach(PlayerChoices choice in player) {
    //         if(choice != PlayerChoices.Banned) {
    //             totalPlayers ++;
    //         }
    //         if(choice == PlayerChoices.Player) {
    //             haveHuman = true;
    //         }
    //     }
    //     // 错误：若players人数低于最小人数限制
    //     if(totalPlayers < choosePlayer.MinPlayer) {
    //         WarningManager.errors.Add(new WarningModel("角色数量最少为" + choosePlayer.MinPlayer + "人！"));
    //         return;
    //     }
    //     // 错误：若玩家数为0
    //     if(!haveHuman) {
    //         WarningManager.errors.Add(new WarningModel("至少有一个玩家参与游戏！"));
    //         return;
    //     }

    //     //获取maps
    //     string mapName = chooseMap.mapName.text;

    //     //地图不能为空
    //     if(mapName.Length <= 1) {
    //         WarningManager.errors.Add(new WarningModel("请选择地图！"));
    //         return;
    //     }

    //     //符合要求，进入游戏
    //     GameObject.DontDestroyOnLoad(message.gameObject);
    //     message.SetMessage<string>("mapFilename", mapFilename);
    //     message.SetMessage<List<PlayerChoices>>("playerChoice", player);
    //     SceneManager.LoadScene("Game");
    // }

    // /// <summary>
    // /// <para> 设置当前地图 </para>
    // /// </summary>
    // public void SetCurrentMap(string filename) {
    //     Debug.Log("预览："+filename);
    //     //读取地图
    //     TextAsset text = Resources.Load<TextAsset>("Maps/"+filename);
    //     string json = text.text;
    //     currentMap = BoardEntity.FromJson(json);
    //     mapFilename = filename;
    //     //通知map和player
    //     chooseMap.CurrentMap = currentMap;
    // }

    /// <summary>
    /// <para> 滑动到选关界面，是chooseLevelButton的点击响应函数 </para>
    /// </summary>
    public void ToChooseLevel() {
        scroller.SlideTo(1f);
    }

    /// <summary>
    /// <para> 滑动到主菜单，是returnMainMenuButton的点击响应函数 </para>
    /// </summary>
    public void ToMainMenu() {
        scroller.SlideTo(0f);
    }

    /// <summary>
    /// <para> 开始关卡编辑器，是startMapEditButton的点击响应函数 </para>
    /// </summary>
    void StartMapEdit() {
        SceneManager.LoadScene("MapEditor");
    }
}
