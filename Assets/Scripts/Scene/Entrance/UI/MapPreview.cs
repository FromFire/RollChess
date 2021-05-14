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
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateSelf);
    }

    /// <summary>
    ///   <para> 按model更新显示 </para>
    /// </summary>
    public void UpdateSelf() {
        // 预览地图
        Sprite image = SaveResource.saveManager.LoadThumb(EntranceResource.mapChooseState.MapFileName);
        thumbnail.sprite = image;
        thumbnail.color = Color.white;
        // 预览地图名
        mapName.text = EntranceResource.mapChooseState.MapFileName;
    }
}
