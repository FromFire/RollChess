using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 视角控制 </para>
/// </summary>
public class VisionController : MonoBehaviour {

    /// <summary>
    ///   <para> 允许鼠标滚轮放大缩小 </para>
    /// </summary>
    public bool allowZoom;

    // 允许拖拽方向，allowMoveUp表示允许摄像机向上移动
    public bool allowMoveUp = true;
    public bool allowMoveDown = true;
    public bool allowMoveLeft = true;
    public bool allowMoveRight = true;

    /// <summary>
    ///   <para> 拖拽按键，默认左键 </para>
    /// </summary>
    public int dragKey = 0;

    void Update() {
        // 避免与UI按键冲突
        if (CursorMonitor.CursorIsOverUI()) {
            return;
        }

        //鼠标滚轮放大缩小
        if(allowZoom)
            Zoom();

        //鼠标拖动地图
        Drag();
    }

    // 鼠标滚轮放大缩小
    void Zoom() {
        //鼠标滚轮向下，视角拉远
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            GameResource.cameraController.ZoomOut();
        }

        //鼠标滚轮向上，视角拉近
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            GameResource.cameraController.ZoomIn();
        }
    }

    // 鼠标拖动地图
    void Drag() {
        // 判断是否存在拖拽
        if(!CursorMonitor.IsMouseDrag(dragKey))
            return;

        // 获取鼠标拖动距离
        Vector3 dragDistance = CursorMonitor.MouseScreenPositionNow - CursorMonitor.MouseScreenPositionLastFrame;

        // 根据allowMove四方向，修正移动距离
        if( (!allowMoveUp && dragDistance.y < 0) || (!allowMoveDown && dragDistance.y > 0) ) {
            dragDistance.y = 0;
        }
        if( (!allowMoveLeft && dragDistance.x > 0) || (!allowMoveRight && dragDistance.x < 0) ) {
            dragDistance.x = 0;
        }
        
        // 以像素为单位做转换
        GameResource.cameraController.TranslateByPixel(-dragDistance);
    }
}