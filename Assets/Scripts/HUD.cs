using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    Rule rule;

    //roll点按钮
    Button rollButton;
    //roll出的数值
    Text rollNumber;

    //获胜界面
    Image gameReview;

    //回合数
    Text turnCount;

    // Start is called before the first frame update
    void Start()
    {
        //初始化roll点相关
        rollButton = GameObject.Find("/HUD/RollButton").GetComponent<Button> ();
		rollButton.onClick.AddListener(rollDice);
        rollNumber = GameObject.Find("/HUD/RollNumber").GetComponent<Text> ();

        //初始化回合数
        turnCount = GameObject.Find("/HUD/TurnCount").GetComponent<Text> ();

        //初始化获胜界面
        gameReview = GameObject.Find("/HUD/GameReview").GetComponent<Image> ();
        gameReview.gameObject.SetActive(false);
    }

    //绑定Rule
    public void setRule(Rule rule) {
        this.rule = rule;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //掷骰子
    //是RollButton的OnClick函数
    public void rollDice() {
        rule.rollDice();
    }

    //显示roll点按钮，隐藏步数
    public void showRollButton() {
        rollButton.gameObject.SetActive(true);
        rollNumber.text = "";
    }

    //隐藏roll点按钮，显示步数
    public void showRollStep(int step) {
        rollButton.gameObject.SetActive(false);
        rollNumber.text = step + "步";
    }

    //更新回合数
    public void updateTurn(int turn) {
        turnCount.text = "第 " + turn + " 回合";
    }

    //弹出获胜消息
    public void showGameReview(int winner) {
        gameReview.gameObject.SetActive(true);
        Text winnerInfo = GameObject.Find("/HUD/GameReview/WinnerInfo").GetComponent<Text> ();
        winnerInfo.text = "玩家 " + winner + " 获胜了！";
    }
}
