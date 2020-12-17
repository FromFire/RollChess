using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuGUI : MonoBehaviour
{

    //开始游戏按钮
    Button startGameButton;
    
    //打开关卡编辑器按钮
    Button startMapEditButton;

    // Start is called before the first frame update
    void Start()
    {
        //初始化开始游戏按钮
        startGameButton = GameObject.Find("/GUI/StartGameButton").GetComponent<Button> ();
		startGameButton.onClick.AddListener(StartGame);

        //初始化打开关卡编辑器按钮
        startMapEditButton = GameObject.Find("/GUI/StartMapEditButton").GetComponent<Button> ();
		startMapEditButton.onClick.AddListener(StartMapEdit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //开始游戏，是startGameButton的点击相应函数
    public void StartGame() {
        SceneManager.LoadScene("ChessPlay");
    }

    //开始关卡编辑器，是startMapEditButton的点击相应函数
    public void StartMapEdit() {
        SceneManager.LoadScene("MapEdit");
    }
}
