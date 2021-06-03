using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class ModelResource : MonoBehaviour  {
    /// <summary>
    ///   <para> 棋盘更新的推送 </para>
    /// </summary>
    static public PositionSubject boardSubject;

    /// <summary>
    ///   <para> 棋子更新的推送 </para>
    /// </summary>
    static public PositionSubject tokenSubject;

    /// <summary>
    ///   <para> 地图选择的推送 </para>
    /// </summary>
    static public Subject mapChooseSubject;

    // 实例化
    void Start() {

        // 推送要在Start里初始化，不然切换场景会有bug！！
        tokenSubject = new PositionSubject();
        boardSubject = new PositionSubject();
        mapChooseSubject = new Subject();
    }
}