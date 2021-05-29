using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 预览地图信息，目前包括：图片、地图名称 </para>
/// </summary>
public class MapPreview : MonoBehaviour
{
    // 预览图片
    [SerializeField] private Image thumbnail;

    // 地图名称显示
    [SerializeField] private Text mapName;

    void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateThumb);
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_Name, UpdateName);
        UpdateThumb();
        UpdateName();
    }

    /// <summary>
    ///   <para> 更新略缩图的显示 </para>
    /// </summary>
    public void UpdateThumb() {
        string mapFileName = EntranceResource.mapChooseState.MapFileName;
        // 空地图情况
        if(mapFileName.Length == 0)
            return;
        // 预览地图
        Sprite image = SaveResource.saveManager.LoadThumb(mapFileName);
        // 略缩图为空的情况
        if(image == null)
            return;
        thumbnail.sprite = image;
        thumbnail.color = Color.white;
    }

    /// <summary>
    ///   <para> 更新地图名的显示 </para>
    /// </summary>
    public void UpdateName() {
        // 预览地图名
        mapName.text = EntranceResource.mapChooseState.MapName;
    }
}
