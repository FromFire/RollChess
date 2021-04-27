using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 弹窗，需要手动填充内容 </para>
/// </summary>
public class Popup : MonoBehaviour {

    /// <summary>
    ///   <para> 填充的内容 </para>
    /// </summary>
    [SerializeField] private GameObject content;

    // // 无穷远处
    // Vector3 FAR_AWAY = new Vector3(3000, 3000, 3000);

    // 默认隐藏
    private void Start() {
        Hide();
    }

    /// <summary>
    ///   <para> 显示弹窗 </para>
    /// </summary>
    public void Show() {
        //SetPosition(position);
        gameObject.SetActive(true);
    }

    /// <summary>
    ///   <para> 隐藏弹窗 </para>
    /// </summary>
    public void Hide() {
        //SetPosition(FAR_AWAY);
        gameObject.SetActive(false);
    }

    /// <summary>
    ///   <para> 根据世界坐标来修改弹窗位置 </para>
    /// </summary>
    public void SetPosition(Vector3 position) {
        transform.position=position;
    }

    /// <summary>
    ///   <para> 填充的内容 </para>
    /// </summary>
    public GameObject Content {
        get {return content;}
    }
}