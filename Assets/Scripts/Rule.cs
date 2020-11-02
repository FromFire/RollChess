using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {

    //存储棋盘初始化信息的实体，也就是从json中读取的信息
    BoardEntity boardEntity;

    //负责棋盘的显示
    MapDisplay mapDisplay;

    private Board board;
    private List<Token> tokens;

    // 初始化棋盘
    void Start()
    {
        //读取地图json文件
        string filename = "MapSample";
        loadMapFromJson(filename);

        //显示游戏初始状态
        mapDisplay = GameObject.Find("/Grid").GetComponent<MapDisplay>();
        mapDisplay.display(boardEntity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //从json文件中读取地图
    void loadMapFromJson(string filename) {
        Debug.Log("从json中加载地图;" + filename);

        //读取文件
        string json = "";
        TextAsset text = Resources.Load<TextAsset>(filename);
        json = text.text;
        Debug.Log(json);
        Debug.Assert(!string.IsNullOrEmpty(json));

        //转换json
        boardEntity = JsonUtility.FromJson<BoardEntity>(json);

        //控制台输出boardEntity
        boardEntity.toConsole();
    }

}
