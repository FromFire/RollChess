using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

/// <summary>
///   <para> 鼠标和键盘监控 </para>
/// </summary>
public class InputMonitor : MonoBehaviour{

    /// <summary>
    ///   <para> 每个键按住的时间 </para>
    /// </summary>
    static public readonly Dictionary<KeyCode, float> keyDownDuration;

    /// <summary>
    ///   <para> 鼠标上帧的位置 </para>
    /// </summary>
    static public readonly Vector3 mousePositionLastFrame;

    /// <summary>
    ///   <para> 鼠标是否在拖拽状态 </para>
    /// </summary>
    static public readonly Vector3 isMouseDrag;

    // 更新所有状态
    void Update() {
        
    }

    /// <summary>
    ///   <para> 获取当前鼠标的绝对坐标 </para>
    /// </summary>
    static public Vector3 CursorPosition() {
        return Input.mousePosition;
    }

    /// <summary>
    ///   <para> 鼠标是否在拖拽状态 </para>
    /// </summary>
    static public bool CursorIsOverUI() {
        return ( EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null );
    }
}