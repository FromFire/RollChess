using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 编辑地图选项 </para>
/// </summary>
public class MapEditPage : MonoBehaviour {

    [SerializeField] MapMenu mapMenu;

    /// <summary>
    ///   <para> 复制地图 </para>
    /// </summary>
    public void CopyMap() {
        string filename = EntranceResource.entranceController.CopyMap();
        mapMenu.AddItem(filename);
    }

    /// <summary>
    ///   <para> 编辑地图 </para>
    /// </summary>
    public void EditMap() {
        EntranceResource.entranceController.EditMap();
    }

    /// <summary>
    ///   <para> 删除地图 </para>
    /// </summary>
    public void DeleteMap() {
        EntranceResource.entranceController.DeleteMap();
    }
}