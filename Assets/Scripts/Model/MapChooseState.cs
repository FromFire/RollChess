using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
///   <para> 地图选择状态 </para>
///   <para> 只用于Entrance，以及Game初始化，游戏开始后不使用 </para>
/// </summary>
public class MapChooseState : MonoBehaviour {
    // 单例
    static MapChooseState singleton;
    // 获取单例
    public static MapChooseState Get() { return singleton; }

    // 玩家操作形式，默认全部是玩家，默认4人
    // 需要手动初始化
    private Dictionary<PlayerID, PlayerForm> playerForm = new Dictionary<PlayerID, PlayerForm> {
        {PlayerID.Red, PlayerForm.Player},
        {PlayerID.Blue, PlayerForm.Player},
        {PlayerID.Green, PlayerForm.Player},
        {PlayerID.Yellow, PlayerForm.Player},
    };

    // 当前选中的地图的文件名
    private string mapFileName = "";

    // 玩家数量限制
    private (int min, int max) playerLimit;

    // 地图名称
    private string mapName = "";

    private void Start() {
        singleton = this;
    }

    /// <summary>
    ///   <para> 构造一个MapChooseState，仅用于调试时 </para>
    /// </summary>
    static public MapChooseState CreateSample() {
        // 不能用new()创建，因为MapChooseState继承于MonoBehaviour
        GameObject gameObject = new GameObject ("MapChooseState");
        MapChooseState mapChooseState = gameObject.AddComponent<MapChooseState>();

        // 默认2玩家，一定不会出错
        mapChooseState.playerForm[PlayerID.Green] = PlayerForm.Banned;
        mapChooseState.playerForm[PlayerID.Yellow] = PlayerForm.Banned;

        // 设置地图路径
        mapChooseState.mapFileName = "MapSample";

        // 使用样例地图：Texts/MapSample.json，将其复制到要读取的文件中
        string filepath = SaveResource.saveManager.MapNameToPath(mapChooseState.mapFileName);
        TextAsset t = Resources.Load<TextAsset>("Texts/MapSample");
        byte[] bytes = t.bytes;
        SaveResource.saveManager.WriteFile(filepath, bytes);

        return mapChooseState;
    }

    /// <summary>
    ///   <para> 当前选中的地图的文件名 </para>
    /// </summary>
    public string MapFileName {
        get {return mapFileName;}
        set {
            mapFileName = value;
            ModelResource.mapChooseSubject.Notify(ModelModifyEvent.Map_File_Name);
            // FileName是空的情况
            if(mapFileName.Length == 0)
                return;
            // 修改地图名称
            SaveEntity saveEntity = SaveResource.saveManager.LoadMap(mapFileName);
            MapName = saveEntity.mapName;
            // 修改人数限制
            PlayerLimit = (saveEntity.player.min, saveEntity.player.max);
        }
    }

    /// <summary>
    ///   <para> 玩家数量限制 </para>
    /// </summary>
    public (int min, int max) PlayerLimit {
        get {return playerLimit;}
        set {
            playerLimit = value;
            ModelResource.mapChooseSubject.Notify(ModelModifyEvent.Player_Limit);
        }
    }

    /// <summary>
    ///   <para> 地图名称 </para>
    /// </summary>
    public string MapName {
        get {return mapName;}
        set {
            mapName = value;
            ModelResource.mapChooseSubject.Notify(ModelModifyEvent.Map_Name);
        }
    }

    /// <summary>
    ///   <para> 设置玩家操作形式 </para>
    /// </summary>
    public void SetPlayerForm(PlayerID playerID, PlayerForm _playerForm) {
        playerForm[playerID] = _playerForm;
        ModelResource.mapChooseSubject.Notify(ModelModifyEvent.Player_Form);
    }

    /// <summary>
    ///   <para> 获取角色操作形式 </para>
    /// </summary>
    public PlayerForm GetPlayerForm(PlayerID playerID) {
        return playerForm[playerID];
    }

    /// <summary>
    ///   <para> 获取角色操作形式 </para>
    /// </summary>
    public Dictionary<PlayerID, PlayerForm> AllPlayerForm() {
        return new Dictionary<PlayerID, PlayerForm>(playerForm);
    }

    /// <summary>
    ///   <para> 玩家数量 </para>
    /// </summary>
    public int AvaliablePlayerNumber() {
        int ret = 0;
        foreach (KeyValuePair<PlayerID, PlayerForm> kvp in playerForm)
            if (kvp.Value == PlayerForm.Player)
                ret += 1;
        return ret;
    }
}