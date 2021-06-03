using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 编辑地图选项页 </para>
/// </summary>
public class MapEditPage : MonoBehaviour {

    // 地图选择列表
    [SerializeField] MapMenu mapMenu;

    // 地图操作按钮
    [SerializeField] Button copyButton;
    [SerializeField] Button editButton;
    [SerializeField] Button deleteButton;

    // 地图设置面板
    [SerializeField] MapInfoSetting mapInfoSetting;

    void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateSelf);
        UpdateSelf();
    }

    /// <summary>
    ///   <para> 更新自身 </para>
    /// </summary>
    public void UpdateSelf() {
        string filename = MapChooseState.Get().MapFileName;
        bool valid = (filename.Length != 0);
        copyButton.interactable = valid;
        editButton.interactable = valid;
        deleteButton.interactable = valid;
    }

    /// <summary>
    ///   <para> 复制地图 </para>
    /// </summary>
    public void CopyMap() {
        string filename = MapOperationController.Get().CopyMap();
        mapMenu.AddItem(filename);
    }

    /// <summary>
    ///   <para> 编辑地图 </para>
    /// </summary>
    public void EditMap() {
        MapOperationController.Get().EditMap();
    }

    /// <summary>
    ///   <para> 删除地图 </para>
    /// </summary>
    public void DeleteMap() {
        // 删除文件
        string filename = MapChooseState.Get().MapFileName;
        MapOperationController.Get().DeleteMap();
        mapMenu.RemoveItem(filename);

        // 维护mapChooseState
        MapChooseState.Get().MapFileName = "";
    }

    /// <summary>
    ///   <para> 准备新建地图，点击“新建地图”的响应函数 </para>
    /// </summary>
    public void PrepareNewMap() {
        // 清空地图信息，解锁InfoSetting用来输入新地图的信息
        MapOperationController.Get().PrepareNewMap();
        mapInfoSetting.SetLock(false);
    }
}