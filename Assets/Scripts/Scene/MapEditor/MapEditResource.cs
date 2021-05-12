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

    // Controller
    static public MomentoController momentoController;
    static public PaintController paintController;

    // 高亮颜色
    static public Color highlightColor;
    // 默认颜色
    static public Color defaultColor;

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

    // Controller
    [SerializeField] private MomentoController _momentoController;
    [SerializeField] private PaintController _paintController;

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

        // Controller
        momentoController = _momentoController;
        paintController = _paintController;

        // tilemap
        tilemapManager = _tilemapManager;

        // 高亮颜色是浅绿色
        ColorUtility.TryParseHtmlString("#66FF55", out highlightColor);
        // 默认颜色是白色
        ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor);

        // 初始化Dictionary
        EditObjectToPaint = new Dictionary<MapEditObject, Paint>() {
            {MapEditObject.Land, landEditor},
            {MapEditObject.Token, tokenEditor},
            {MapEditObject.Special, specialEditor},
            {MapEditObject.Portal, portalEditor}
        };
    }

    /// <summary>
    ///   <para> 绘制类型转换为Painter </para>
    /// </summary>
    static public Dictionary<MapEditObject, Paint> EditObjectToPaint;
}