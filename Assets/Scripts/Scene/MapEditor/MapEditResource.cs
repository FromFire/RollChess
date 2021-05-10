using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class MapEditResource : MonoBehaviour  {
    // Model
    static public GameState gameState;

    // 绘制器
    static public LandEditor landEditor;
    static public TokenEditor tokenEditor;
    static public SpecialEditor specialEditor;
    static public PortalEditor portalEditor;

    // Display
    static public BoardDisplay boardDisplay;
    static public TokenDisplay tokenDisplay;

    /// <summary>
    ///   <para> 游戏状态的推送 </para>
    /// </summary>
    static public Subject gameStateSubject = new Subject();

    /// <summary>
    ///   <para> 不能显示任何Tile，仅用于获取坐标等 </para>
    /// </summary>
    static public TilemapManager tilemapManager;

    // 下面是用来实例化的
    // Model
    [SerializeField] private GameState _gameState;

    // 绘制器
    [SerializeField] private LandEditor _landEditor;
    [SerializeField] private TokenEditor _tokenEditor;
    [SerializeField] private SpecialEditor _specialEditor;
    [SerializeField] private PortalEditor _portalEditor;

    // tilemap
    [SerializeField] private TilemapManager _tilemapManager;

    // 实例化
    void Start() {
        // Model
        gameState = _gameState;

        // 绘制器
        landEditor = _landEditor;
        tokenEditor = _tokenEditor;
        specialEditor = _specialEditor;
        portalEditor = _portalEditor;

        // tilemap
        tilemapManager = _tilemapManager;
    }
}