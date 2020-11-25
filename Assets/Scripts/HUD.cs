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

    // Start is called before the first frame update
    void Start()
    {
        //绑定roll点按钮和OnClick函数
        rollButton = GameObject.Find("/HUD/RollButton").GetComponent<Button> ();
		rollButton.onClick.AddListener(rollDice);
        rollNumber = GameObject.Find("/HUD/RollNumber").GetComponent<Text> ();
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
}
