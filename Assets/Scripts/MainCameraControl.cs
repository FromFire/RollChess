using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
    //本摄像头
    Camera mainCamera;

    //鼠标拖拽时的原本位置
    Vector3 mousePosPre;
    //上一帧时是否按住鼠标
    bool isMousePressedLastTime = false;


    // Start is called before the first frame update
    void Start()
    {
        //获取主摄像机
        mainCamera=GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标滚轮缩小（最远为10）
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize <= 10) {
            mainCamera.orthographicSize += 1;
        }

        //鼠标滚轮放大（最近为1）
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && mainCamera.orthographicSize > 1) {
            mainCamera.orthographicSize -= 1;
        }

        //鼠标左键按住不放，拖拽视角
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosNow = Input.mousePosition;
            //摄像头平移
            if(isMousePressedLastTime) {
                //摄像头离地图越近，移动幅度越小
                //200是试出来的参数，防止拖动速度太快，但鼠标和地图移动速度仍有一点差异
                Vector3 trans = (mousePosPre-mousePosNow)*mainCamera.orthographicSize/200;
                transform.Translate(trans, Space.Self);
            }
            //维护数据
            mousePosPre = mousePosNow;
            isMousePressedLastTime = true;
        } else {
            isMousePressedLastTime = false;
        }
    }

    
}
