using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class ToolResource : MonoBehaviour  {

    // Tool
    static public CameraController cameraController;
    static public CursorMonitor cursorMonitor;
    static public VisionController visionController;

    /// <summary>
    ///   <para> 不能显示任何Tile，仅用于获取坐标等 </para>
    /// </summary>
    static public TilemapManager tilemapManager;

    // 下面是用来实例化的
    // Tool
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CursorMonitor _cursorMonitor;
    [SerializeField] private VisionController _visionController;

    // tilemap
    [SerializeField] private TilemapManager _tilemapManager;

    // 实例化
    void Start() {
        // Tool
        cameraController = _cameraController;
        cursorMonitor = _cursorMonitor;
        visionController = _visionController;

        // tilemap
        tilemapManager = _tilemapManager;
    }
}