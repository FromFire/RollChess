using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChoosePlayer : MonoBehaviour
{
    //切换玩家的按钮
    public List<Button> playerButtons;

    // 选择栏的三个图标
    Sprite[] playerSprites;

    // 玩家操控方式选项
    enum PlayerChoices{Player, Comuputer, Banned};

    // 当前的玩家操纵方式
    PlayerChoices[] nowPlayerChoices;

    // Start is called before the first frame update
    void Start()
    {
        //为切换按钮绑定点击事件
        for(int i=0; i<playerButtons.Count; i++) {
            int arg = i;
            playerButtons[i].onClick.AddListener(()=> {changePlayer(arg);} );
        }
        //加载图标资源
        playerSprites = new Sprite[3] {
            Resources.Load<Sprite>("Sprites/Player"),
            Resources.Load<Sprite>("Sprites/Computer"),
            Resources.Load<Sprite>("Sprites/Banned"),
        };
        //初始化当前选择
        nowPlayerChoices = new PlayerChoices[4]{PlayerChoices.Player, PlayerChoices.Player, PlayerChoices.Player, PlayerChoices.Player};
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
