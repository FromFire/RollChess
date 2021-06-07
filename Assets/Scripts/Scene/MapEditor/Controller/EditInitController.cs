using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
///   <para> 对地图的初始化 </para>
/// </summary>
public class EditInitController : MonoBehaviour {
    void Start() {
        Init();
    }

    /// <summary>
    ///   <para> 初始化 </para>
    /// </summary>
    void Init() {
        // 不是从Entrance过来就不初始化了
        if(MapEditResource.mapChooseState is null)
            return;

        // 读取存档
        string filename = MapEditResource.mapChooseState.MapFileName;
        Debug.Log("加载地图：" + filename);
        MapEditResource.mapFilename = filename;
        SaveEntity saveEntity = SaveResource.saveManager.LoadMap(filename);
        SaveResource.saveLoader.Load(saveEntity);

        // 销毁MapChooseState（若存在）
        if(MapEditResource.mapChooseState.gameObject)
            Destroy(MapEditResource.mapChooseState.gameObject);
        MapEditResource.mapChooseState = null;
    }
}