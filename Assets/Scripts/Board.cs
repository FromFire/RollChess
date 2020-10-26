using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类
// 加载地图、管理地图状态
public class Board : MonoBehaviour{

    BoardEntity boardEntity;
    
    //地图名称
    string mapName;

    //玩家数目
    public int playerNumbers{get;} 

    //各方棋子的数量
    private List<int> tokenNumbers{get;} 
    
    //地图
    private int[] map; 

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

        //控制台输出信息
        Debug.Log("名称："+boardEntity.mapName);
        Debug.Assert(boardEntity.players != null);
        Debug.Log("玩家数目："+boardEntity.players.number);
        for(int i=0; i<boardEntity.players.number;i++) {
            Debug.Log("棋子数目"+i+": "+boardEntity.tokens[i].number);
        }
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
public class BoardEntity
{
    public string mapName;

    public PlayersEntity players;

    public List<TokensEntity> tokens;
}
