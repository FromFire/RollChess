using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using PlayerColor = Structure.PlayerColor;

public class HUD : MonoBehaviour
{
    public Rule rule;

    //roll点按钮
    public Button rollButton;
    //roll出的数值
    public Text rollNumber;

    //获胜界面
    public Image gameReview;

    //回合数
    public Text turnCount;

    //正在行动提示
    public Text actionPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //初始化roll点相关
		rollButton.onClick.AddListener(RollDice);

        //初始化获胜界面
        gameReview.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //掷骰子
    //是RollButton的OnClick函数
    public void RollDice() {
        rule.RollDice();
    }

    //显示roll点按钮，隐藏步数
    public void ShowRollButton() {
        rollButton.gameObject.SetActive(true);
        rollNumber.text = "";
    }

    //隐藏roll点按钮，显示步数
    public void ShowRollStep(int step) {
        rollButton.gameObject.SetActive(false);
        rollNumber.text = step + "步";
    }

    //更新回合数
    public void UpdateTurn(int turn) {
        turnCount.text = "第 " + turn + " 回合";
    }

    //更新正在行动提示
    public void UpdateActionPlayer(int player) {
        string str = "";
        switch((PlayerColor)player) {
            case PlayerColor.Red:
                str += "<color=#DD0000FF>红棋</color>";
                break;
            case PlayerColor.Blue:
                str += "<color=#0000DDFF>蓝棋</color>";
                break;
            case PlayerColor.Yellow:
                str += "<color=#FFDD00FF>黄棋</color>";
                break;
            case PlayerColor.Green:
                str += "<color=#00DD00FF>绿棋</color>";
                break;
        }
        str += "行动中";
        actionPlayer.text = str;
    }

    //弹出获胜消息
    public void ShowGameReview(int winner) {
        gameReview.gameObject.SetActive(true);
        Text winnerInfo = GameObject.Find("/HUD/GameReview/WinnerInfo").GetComponent<Text> ();
        winnerInfo.text = "玩家 " + winner + " 获胜了！";
    }
}
