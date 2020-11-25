using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControl : MonoBehaviour
{
    Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        //获取主摄像机
        mainCamera=GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标滚轮缩小
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(mainCamera.orthographicSize <= 10)
                mainCamera.orthographicSize += 1;
        }
        //鼠标滚轮放大
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(mainCamera.orthographicSize > 1)
                mainCamera.orthographicSize -= 1;
        }
    }

    
}
