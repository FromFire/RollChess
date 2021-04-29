using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class PublicResource {
    static public Board board;
    static public TokenSet tokenSet;
    static public GameState gameState;
    static public SaveManager saveManager;
    static public CameraController cameraController;
    static public CursorMonitor cursorMonitor;
    static public VisionController visionController;
    static public SpecialIntroduction specialIntroduction;
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
}