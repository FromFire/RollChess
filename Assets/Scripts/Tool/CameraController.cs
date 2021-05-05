using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 摄像头相关功能 </para>
/// </summary>
public class CameraController : MonoBehaviour {
    //本摄像头
    [SerializeField] private Camera mcamera;

    /// <summary>
    ///   <para> 视角缩进灵敏度 </para>
    /// </summary>
    public const int zoomSensitivity = 1;

    /// <summary>
    ///   <para> 视角缩进最远距离 </para>
    /// </summary>
    public const float zoomMax = 8;

    /// <summary>
    ///   <para> 视角缩进最近距离 </para>
    /// </summary>
    public const float zoomMin = 1.1f;

    /// <summary>
    ///   <para> 屏幕坐标到世界坐标 </para>
    /// </summary>
    public Vector3 ScreenToWorld(Vector3 position) {
        return mcamera.ScreenToWorldPoint(position);
    }

    /// <summary>
    ///   <para> 视角拉近 </para>
    /// </summary>
    public void ZoomIn() {
        if (mcamera.orthographicSize >= zoomMin) {
            mcamera.orthographicSize -= zoomSensitivity;
        }Debug.Log(mcamera.orthographicSize);
    }

    /// <summary>
    ///   <para> 视角拉远 </para>
    /// </summary>
    public void ZoomOut() {
        if (mcamera.orthographicSize <= zoomMax) {
            mcamera.orthographicSize += zoomSensitivity;
        }Debug.Log(mcamera.orthographicSize);
    }

    /// <summary>
    ///   <para> 摄像头平移（z轴不变），单位为Unit </para>
    /// </summary>
    public void Translate(Vector3 translation) {
        transform.Translate(translation, Space.Self);
    }

    /// <summary>
    ///   <para> 摄像头平移（z轴不变），单位为像素 </para>
    /// </summary>
    public void TranslateByPixel(Vector3 translation) {
        // Camera.orthographicSize表示摄像机范围高度一半，单位为Unit
        // 计算出Unit/Pixel的比值
        float unitPerPixel = mcamera.orthographicSize * 2 / Screen.height;

        // 以Unit为单位转换
        Vector3 trans = translation * unitPerPixel;
        Translate(trans);

        //旧算法：
        //摄像头离地图越近，移动幅度越小
        //200是试出来的参数，防止拖动速度太快，但鼠标和地图移动速度仍有一点差异
        //Vector3 trans = (mousePosPre - mousePosNow) * mcamera.orthographicSize / 200;
    }
}