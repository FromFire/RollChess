using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    ///   <para> 当前选中的地图的路径 </para>
    /// </summary>
    public string mapPath;

    /// <summary>
    ///   <para> 地图略缩图的路径 </para>
    /// </summary>
    public string mapThumbPath;

    /// <summary>
    ///   <para> 地图略缩图 </para>
    /// </summary>
    public byte[] mapThumb;

    /// <summary>
    ///   <para> 当前选中的地图 </para>
    /// </summary>
    public SaveEntity currentMap {get;}
}