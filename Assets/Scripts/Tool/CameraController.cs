using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 摄像头相关功能 </para>
/// </summary>
public class CameraController : MonoBehaviour {
    //本摄像头
    private Camera mcamera;

    /// <summary>
    ///   <para> 视角缩进灵敏度 </para>
    /// </summary>
    public const int zoomSensitivity = 1;

    /// <summary>
    ///   <para> 视角缩进最远距离 </para>
    /// </summary>
    public const float zoomMax = 10;

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
        // Input.GetAxis("Mouse ScrollWheel") < 0 判断滚轮
        if (mcamera.orthographicSize <= zoomMax) {
            mcamera.orthographicSize += zoomSensitivity;
        }
    }

    /// <summary>
    ///   <para> 视角拉远 </para>
    /// </summary>
    public void ZoomOut() {
        // Input.GetAxis("Mouse ScrollWheel") > 0 判断滚轮
        if (mcamera.orthographicSize >= zoomMin) {
            mcamera.orthographicSize += zoomSensitivity;
        }
    }

    /// <summary>
    ///   <para> 摄像头平移（z轴不变） </para>
    /// </summary>
    public void Translate(Vector3 translation) {
        transform.Translate(translation, Space.Self);
    }
}