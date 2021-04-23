using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

/// <summary>
///   <para> 鼠标监控 </para>
/// </summary>
public class CursorMonitor : MonoBehaviour{

    // 鼠标每个键按住的时间，0=左键，1=右键，2=中键
    static private Dictionary<int, float> mouseDownDuration;

    // 鼠标上帧的屏幕坐标
    static private Vector3 mouseScreenPositionLastFrame;

    // 鼠标此帧的位置，屏幕坐标
    static private Vector3 mouseScreenPositionNow;

    // 初始化
    static CursorMonitor() {
        mouseDownDuration = new Dictionary<int, float>();
    }

    // 更新所有状态
    void Update() {
        // 更新鼠标上帧位置
        mouseScreenPositionLastFrame = mouseScreenPositionNow;
        // 更新鼠标本帧位置
        mouseScreenPositionNow = Input.mousePosition;

        // 更新keyDownDuration
        // 若没有按下，则清空在字典中的记录。
        for(int i=0; i<=2; i++) {
            if(Input.GetMouseButton(i))
                mouseDownDuration[i] += Time.deltaTime;
            else
                mouseDownDuration.Remove(i);
        }
    }

    /// <summary>
    ///   <para> 鼠标是否在UI上 </para>
    /// </summary>
    static public bool CursorIsOverUI() {
        return ( EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null );
    }

    /// <summary>
    ///   <para> 每个键按住的时间，0=左键，1=右键，2=中键 </para>
    ///   <para> 若没有按住，返回一个负数 </para>
    /// </summary>
    static public float MouseDownDuration(int button) {
        if(!mouseDownDuration.ContainsKey(button)) 
            return -1;
        return mouseDownDuration[button];
    }

    /// <summary>
    ///   <para> 鼠标上帧的屏幕坐标 </para>
    /// </summary>
    static public Vector3 MouseScreenPositionLastFrame {
        get {return mouseScreenPositionLastFrame; }
    }

    /// <summary>
    ///   <para> 鼠标上帧的屏幕坐标 </para>
    /// </summary>
    static public Vector3 MouseScreenPositionNow {
        get {return mouseScreenPositionNow; }
    }
}