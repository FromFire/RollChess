using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class ModelResource : MonoBehaviour  {
    // Model
    static public Board board;
    static public TokenSet tokenSet;

    /// <summary>
    ///   <para> 棋盘更新的推送 </para>
    /// </summary>
    static public PositionSubject boardSubject = new PositionSubject();

    /// <summary>
    ///   <para> 棋子更新的推送 </para>
    /// </summary>
    static public PositionSubject tokenSubject = new PositionSubject();

    /// <summary>
    ///   <para> 地图选择的推送 </para>
    /// </summary>
    static public Subject mapChooseSubject = new Subject();

    // 下面是用来实例化的
    // Model
    [SerializeField] private Board _board;
    [SerializeField] private TokenSet _tokenSet;

    // 实例化
    void Start() {
        // Model
        board = _board;
        tokenSet = _tokenSet;
    }
}