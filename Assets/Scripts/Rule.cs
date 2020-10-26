using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    private Board board;
    private List<Token> tokens;

    // Start is called before the first frame update
    void Start()
    {
        //board = GameObject.Find("Board").GetComponent<Board>();
        //int playerNumber = board.playerNumbers;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
