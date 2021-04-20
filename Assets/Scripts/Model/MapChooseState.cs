using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 地图选择状态 </para>
/// </summary>
public class MapChooseState {

    /// <summary>
    ///   <para> 回合数 </para>
    /// </summary>
    public List<PlayerForm> nowPlayerForm {get;}

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