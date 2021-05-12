using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换地图笔刷 </para>
/// </summary>
public class LandMenu : MonoBehaviour {
    void Start() {}

    /// <summary>
    ///   <para> 切换地块和擦除 </para>
    /// </summary>
    public void SwitchLand(bool isPainting) {
        MapEditResource.landEditor.IsPainting = isPainting;
        MapEditResource.paintController.EditObject = MapEditObject.Land;
    }
}