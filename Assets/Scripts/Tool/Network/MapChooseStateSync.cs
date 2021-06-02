using System;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>
/// 用于同步Model
/// </summary>
public class MapChooseStateSync : NetworkBehaviour {
    private void Start() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateSelf);
        UpdateSelf();
    }

    /// <summary>
    /// 选择地图，务必先同步本变量，再同步thumb和map
    /// </summary>
    [SyncVar(hook = "SyncMapFileName")] public string mapFileName;
    // 同步flag
    private bool mapFileNameFlag = false;
    
    /// <summary>
    /// 地图略缩图
    /// </summary>
    [SyncVar(hook = "SyncThumb")] public SyncList<byte> thumb;
    // 同步flag
    private bool thumbFlag = false;
    
    /// <summary>
    /// 地图文件
    /// </summary>
    [SyncVar(hook = "SyncMap")] public string map;
    // 同步flag
    private bool mapFlag = false;
    
    // mapChooseState修改时更新自身
    [Server]
    void UpdateSelf() {
        if (EntranceResource.mapChooseState.MapFileName != mapFileName) {
            mapFileName = EntranceResource.mapChooseState.MapFileName;
            thumb = new SyncList<byte>(SaveResource.saveManager.LoadThumb(mapFileName).texture.EncodeToPNG());
            map = SaveResource.saveManager.LoadMap(mapFileName).ToJson();
        }
    }

    // 将MapChooseState所需的信息同步。需要mapFileName，thumb和map都同步完成后再调用
    void SyncMapChooseState() {
        if (mapFileNameFlag && thumbFlag && mapFlag) {
            EntranceResource.mapChooseState.MapFileName = mapFileName;
            mapFileNameFlag = thumbFlag = mapFlag = false;
        }
        Debug.Log("同步地图：" + mapFileName);
    }
    
    // 同步地图略缩图
    void SyncMapFileName(string oldValue, string newValue) {
        mapFileNameFlag = true;
        SyncMapChooseState();
    }
    
    // 同步地图略缩图
    void SyncThumb(SyncList<byte> oldValue, SyncList<byte> newValue) {
        if (mapFileName is null) return;
        SaveResource.saveManager.SaveThumb(newValue.ToArray(), mapFileName);
        thumbFlag = true;
        SyncMapChooseState();
    }
    
    // 同步地图
    void SyncMap(string oldValue, string newValue) {
        if (mapFileName is null) return;
        SaveResource.saveManager.SaveMap(SaveEntity.FromJson(newValue), mapFileName);
        mapFileNameFlag = true;
        SyncMapChooseState();
    }
}