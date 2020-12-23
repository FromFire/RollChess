using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    //本摄像头
    public Camera camera;

    //鼠标拖拽时的原本位置
    Vector3 mousePosPre;

    //上一帧时是否按住鼠标
    bool isMousePressedLastTime = false;
    public int dragKey = 0;


    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        //鼠标滚轮缩小（最远为10）
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize <= 10) {
            camera.orthographicSize += 1;
        }

        //鼠标滚轮放大（最近为1）
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && camera.orthographicSize > 1) {
            camera.orthographicSize -= 1;
        }

        //鼠标左键按住不放，拖拽视角
        if (Input.GetMouseButton(dragKey)) {
            Debug.Log("Pressed");
            Vector3 mousePosNow = Input.mousePosition;
            //摄像头平移
            if (isMousePressedLastTime) {
                //摄像头离地图越近，移动幅度越小
                //200是试出来的参数，防止拖动速度太快，但鼠标和地图移动速度仍有一点差异
                Vector3 trans = (mousePosPre - mousePosNow) * camera.orthographicSize / 200;
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