using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换地图笔刷 </para>
/// </summary>
public class LandMenu : MonoBehaviour {

    // 绘制地块
    [SerializeField] private Button landButton;

    // 擦除
    [SerializeField] private Button eraseButton;

    void Start() {}

    /// <summary>
    ///   <para> 切换地块和擦除 </para>
    /// </summary>
    public void SwitchLand(bool isPainting) {
        MapEditResource.landEditor.IsPainting = isPainting;
    }
}