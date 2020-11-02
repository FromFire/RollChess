using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类
// 加载地图、管理地图状态
public class Board : MonoBehaviour{

    BoardEntity boardEntity;
    MapDisplay mapDisplay;

    // Start is called before the first frame update
    void Start() {
        //读取地图json文件
        string filename = "MapSample";
        loadMapFromJson(filename);

        mapDisplay = GameObject.Find("/Grid").GetComponent<MapDisplay>();
        mapDisplay.display(boardEntity);
    }

    // Update is called once per frame
    void Update() {
        
    }

    //从json文件中读取地图
    void loadMapFromJson(string filename) {
        Debug.Log("从json中加载地图");

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

