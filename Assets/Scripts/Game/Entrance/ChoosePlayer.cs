using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayerChoices = Structure.PlayerChoices;

public class ChoosePlayer : MonoBehaviour
{
    //切换玩家的按钮
    public List<Button> playerButtons;

    // 选择栏的三个图标
    Sprite[] playerSprites;

    // 显示本地图的玩家数量限制
    public Text playerNumLimitLabel;

    // 当前的玩家操纵方式
    PlayerChoices[] nowPlayerChoices;

    // 玩家数量限制-下限
    int minPlayer = 1;
    int maxPlayer = 4;

    // 玩家当前选择的地图
    public BoardEntity currentMap;

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

    // 显示玩家数量限制
    void ShowPlayerNumLimit(int min, int max) {
        if(min == max) {
            playerNumLimitLabel.text = min + "人";
        } else {
            playerNumLimitLabel.text = min + "-" + max + "人";
        }
    }

    // 锁定玩家（强制为Banned）
    void LockPlayer(int player) {
        while(nowPlayerChoices[player] != PlayerChoices.Banned) {
            changePlayer(player);
        }
        playerButtons[player].interactable = false;
    }

    // 解锁玩家
    void UnlockPlayer(int player) {
        while(nowPlayerChoices[player] != PlayerChoices.Player) {
            changePlayer(player);
        }
        playerButtons[player].interactable = true;
    }

    // 切换第index位玩家的操作方式并显示
    public void changePlayer(int index) {
        Image playerImage = playerButtons[index].gameObject.transform.GetChild(0).GetComponent<Image>();
        PlayerChoices nextChoice = nextPlayerChoice(nowPlayerChoices[index]);
        playerImage.sprite = playerSprites[(int)nextChoice];
        nowPlayerChoices[index] = nextChoice;
    }

    /// <summary>
    ///   <para> 获取所有角色的操控方式 </para>
    /// </summary>
    public List<PlayerChoices> GetPlayers() {
        return new List<PlayerChoices> (nowPlayerChoices);
    }

    // 获取下一个玩家操作选项
    PlayerChoices nextPlayerChoice(PlayerChoices choice) {
        return (PlayerChoices)(((int)choice + 1) % 3);
    }

    // 玩家数量限制-下限
    public int MinPlayer{
        get { return minPlayer; }
        set { 
            minPlayer = value; 
            ShowPlayerNumLimit(minPlayer, maxPlayer);
        }
    }

    // 玩家数量限制-上限
    public int MaxPlayer{
        get { return maxPlayer; }
        set {
            // 锁定更多玩家
            if(value < maxPlayer) {
                for(int i=value; i<maxPlayer; i++) {
                    LockPlayer(i);
                }
            }
            // 解锁部分玩家
            else if(value > maxPlayer) {
                for(int i=maxPlayer; i<value; i++) {
                    UnlockPlayer(i);
                }
            }
            maxPlayer = value;
            ShowPlayerNumLimit(minPlayer, maxPlayer);
        }
    }

    // 设置地图，
    // 更新玩家数量显示
    public BoardEntity CurrentMap {
        set {
            currentMap = value;
            MaxPlayer = currentMap.player.max;
            MinPlayer = currentMap.player.min;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
