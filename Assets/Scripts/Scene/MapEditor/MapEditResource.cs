using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class MapEditResource : MonoBehaviour  {

    // 绘制器
    static public LandEditor landEditor;
    static public TokenEditor tokenEditor;
    static public SpecialEditor specialEditor;
    static public PortalEditor portalEditor;

    // Display
    static public BoardDisplay boardDisplay;
    static public TokenDisplay tokenDisplay;

    // Momento
    static public MomentoController momentoController;

    /// <summary>
    ///   <para> 游戏状态的推送 </para>
    /// </summary>
    static public Subject gameStateSubject = new Subject();

    /// <summary>
    ///   <para> 不能显示任何Tile，仅用于获取坐标等 </para>
    /// </summary>
    static public TilemapManager tilemapManager;

    // 下面是用来实例化的

    // 绘制器
    [SerializeField] private LandEditor _landEditor;
    [SerializeField] private TokenEditor _tokenEditor;
    [SerializeField] private SpecialEditor _specialEditor;
    [SerializeField] private PortalEditor _portalEditor;

    // Display
    [SerializeField] private BoardDisplay _boardDisplay;
    [SerializeField] private TokenDisplay _tokenDisplay;

    // Momento
    [SerializeField] private MomentoController _momentoController;

    // tilemap
    [SerializeField] private TilemapManager _tilemapManager;

    // 实例化
    void Start() {

        // 绘制器
        landEditor = _landEditor;
        tokenEditor = _tokenEditor;
        specialEditor = _specialEditor;
        portalEditor = _portalEditor;

        // Display
        boardDisplay = _boardDisplay;
        tokenDisplay = _tokenDisplay;

        // Momento
        momentoController = _momentoController;

        // tilemap
        tilemapManager = _tilemapManager;

        // 初始化Dictionary
        EditObjectToPaint = new Dictionary<MapEditObject, Paint>() {
            {MapEditObject.Land, landEditor},
            {MapEditObject.Token, tokenEditor},
            {MapEditObject.Special, specialEditor},
            {MapEditObject.Portal, portalEditor}
        };

        EditObjectToBlockPaint = new Dictionary<MapEditObject, BlockPaint>() {
            {MapEditObject.Land, landEditor},
            {MapEditObject.Token, tokenEditor},
            {MapEditObject.Special, specialEditor},
            {MapEditObject.Portal, null}
        };
    }

    /// <summary>
    ///   <para> 绘制类型转换为Painter </para>
    /// </summary>
    static public Dictionary<MapEditObject, Paint> EditObjectToPaint;

    /// <summary>
    ///   <para> 绘制类型转换为BlockPainter </para>
    /// </summary>
    static public Dictionary<MapEditObject, BlockPaint> EditObjectToBlockPaint;
}