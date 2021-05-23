using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 编辑地图选项 </para>
/// </summary>
public class MapEditPage : MonoBehaviour {

    // 地图选择列表
    [SerializeField] MapMenu mapMenu;

    [SerializeField] Button copyButton;
    [SerializeField] Button editButton;
    [SerializeField] Button deleteButton;

    void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateSelf);
        UpdateSelf();
    }

    /// <summary>
    ///   <para> 更新自身 </para>
    /// </summary>
    public void UpdateSelf() {
        string filename = EntranceResource.mapChooseState.MapFileName;
        bool valid = (filename.Length != 0);
        copyButton.interactable = valid;
        editButton.interactable = valid;
        deleteButton.interactable = valid;
    }

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
        // 删除文件
        string filename = EntranceResource.mapChooseState.MapFileName;
        EntranceResource.entranceController.DeleteMap();
        mapMenu.RemoveItem(filename);

        // 维护mapChooseState
        EntranceResource.mapChooseState.MapFileName = "";
    }
}