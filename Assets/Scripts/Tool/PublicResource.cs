using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class PublicResource : MonoBehaviour  {
    // Model
    static public Board board;
    static public TokenSet tokenSet;
    static public GameState gameState;
    static public SpecialIntroduction specialIntroduction;
    static public MapChooseState mapChooseState;

    // Tool
    static public SaveManager saveManager;
    static public CameraController cameraController;
    static public CursorMonitor cursorMonitor;
    static public VisionController visionController;

    // Controller
    static public BoardAssistant boardAssistant;
    static public BoardController boardController;
    static public TokenController tokenController;
    static public GameController gameController;

    /// <summary>
    ///   <para> 棋盘更新的推送 </para>
    /// </summary>
    static public PositionSubject boardSubject = new PositionSubject();

    /// <summary>
    ///   <para> 棋子更新的推送 </para>
    /// </summary>
    static public PositionSubject tokenSubject = new PositionSubject();

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
    [SerializeField] private Board _board;
    [SerializeField] private TokenSet _tokenSet;
    [SerializeField] private GameState _gameState;
    [SerializeField] private SpecialIntroduction _specialIntroduction;

    // Tool
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CursorMonitor _cursorMonitor;
    [SerializeField] private VisionController _visionController;

    // Controller
    [SerializeField] private BoardAssistant _boardAssistant;
    [SerializeField] private BoardController _boardController;
    [SerializeField] private TokenController _tokenController;
    [SerializeField] private GameController _gameController;

    // tilemap
    [SerializeField] private TilemapManager _tilemapManager;

    // 实例化
    void Start() {
        // Model
        board = _board;
        tokenSet = _tokenSet;
        gameState = _gameState;
        specialIntroduction = _specialIntroduction;

        // Tool
        saveManager = _saveManager;
        cameraController = _cameraController;
        cursorMonitor = _cursorMonitor;
        visionController = _visionController;

        // Controller
        boardAssistant = _boardAssistant;
        boardController = _boardController;
        tokenController = _tokenController;
        gameController = _gameController;

        // tilemap
        tilemapManager = _tilemapManager;

        // 此类来自Entrance类
        GameObject mapChooseObject = GameObject.Find("MapChooseState");
        if(mapChooseObject)
            mapChooseState = mapChooseObject.GetComponent<MapChooseState>();
    }
}