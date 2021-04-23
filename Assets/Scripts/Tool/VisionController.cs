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
    ///   <para> 拖拽按键 </para>
    /// </summary>
    public int dragKey = 0;

    //鼠标拖拽时的原本位置
    Vector3 mousePosPre;

    //上一帧时是否按住鼠标
    bool isMousePressedLastTime = false;
    public int dragKey = 0;


    // Update is called once per frame
    void Update() {
        // 避免与UI按键冲突
        if (CursorMonitor.CursorIsOverUI()) {
            return;
        }

        //鼠标滚轮放大缩小
        if(allowZoom) {
            Zoom();
        }

        //鼠标拖动地图
        Drag();
    }

    // 鼠标滚轮放大缩小
    void Zoom() {
        //鼠标滚轮向下，视角拉远
        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            PublicResource.cameraController.ZoomOut();
        }

        //鼠标滚轮向上，视角拉近
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            PublicResource.cameraController.ZoomIn();
        }
    }

    // 鼠标拖动地图
    void Drag() {
        //鼠标左键按住不放，拖拽视角
        if (Input.GetMouseButton(dragKey)) {
            Vector3 mousePosNow = Input.mousePosition;
            //摄像头平移
            if (isMousePressedLastTime) {
                //摄像头离地图越近，移动幅度越小
                //200是试出来的参数，防止拖动速度太快，但鼠标和地图移动速度仍有一点差异
                Vector3 trans = (mousePosPre - mousePosNow) * mcamera.orthographicSize / 200;

                // 根据allowMove四方向，修正移动距离
                if( (!allowMoveUp && trans.y > 0) || (!allowMoveDown && trans.y < 0) ) {
                    trans.y = 0;
                }
                if( (!allowMoveLeft && trans.x < 0) || (!allowMoveRight && trans.x > 0) ) {
                    trans.x = 0;
                }
                
                transform.Translate(trans, Space.Self);
            }

            //维护数据
            mousePosPre = mousePosNow;
            isMousePressedLastTime = true;
        }
        else {
            isMousePressedLastTime = false;
        }
    }
}