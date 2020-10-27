using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类
// 加载地图、管理地图状态
public class Board : MonoBehaviour{

    BoardEntity boardEntity;

    // Start is called before the first frame update
    void Start() {
        //读取地图json文件
        string filename = "MapSample";
        loadMapFromJson(filename);
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

[System.Serializable]
public class PlayersEntity
{
    public int number;
}

[System.Serializable]
public class TokensEntity
{
    public int number;
}

[System.Serializable]
public class MapEntity
{
    public int x;
    public int y;
}

[System.Serializable]
public class BoardEntity
{
    public string mapName;

    public PlayersEntity players;

    public List<TokensEntity> tokens;

    public List<MapEntity> map;

    //控制台输出信息
    public void toConsole() {
        Debug.Log("名称："+mapName);
        Debug.Log("玩家数目："+players.number);
        for(int i=0; i<players.number;i++) {
            Debug.Log("棋子数目"+i+": "+tokens[i].number);
        }
        for(int i=0; i<map.ToArray().Length;i++) {
            Debug.Log("x: "+map[i].x + "\t y: " +map[i].y);
        }
        
    }
}
