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

    /// <summary>
    ///   <para> 玩家操作形式，默认全部是玩家，默认4人 </para>
    /// </summary>
    public Dictionary<PlayerID, PlayerForm> playerForm = new Dictionary<PlayerID, PlayerForm> {
        {PlayerID.Red, PlayerForm.Player},
        {PlayerID.Blue, PlayerForm.Player},
        {PlayerID.Green, PlayerForm.Player},
        {PlayerID.Yellow, PlayerForm.Player},
    };

    /// <summary>
    ///   <para> 最大玩家数量 </para>
    /// </summary>
    public int MaxPlayer;

    /// <summary>
    ///   <para> 最小玩家数量 </para>
    /// </summary>
    public int MinPlayer;

    /// <summary>
    ///   <para> 当前选中的地图的文件名 </para>
    /// </summary>
    public string mapName;

    /// <summary>
    ///   <para> 地图略缩图 </para>
    /// </summary>
    public byte[] mapThumb;

    /// <summary>
    ///   <para> 当前选中的地图 </para>
    /// </summary>
    public SaveEntity currentMap {get;}

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
        mapChooseState.mapName = "MapSample";

        // 使用样例地图：Texts/MapSample.json，将其复制到要读取的文件中
        string filepath = SaveManager.MapNameToPath(mapChooseState.mapName);
        TextAsset t = Resources.Load<TextAsset>("Texts/MapSample");
        byte[] bytes = t.bytes;
        SaveManager.WriteFile(filepath, bytes);

        return mapChooseState;
    }
}