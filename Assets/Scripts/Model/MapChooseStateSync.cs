using System;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>
/// 用于同步Model
/// </summary>
// 同步步骤：
// 1. Server修改Model，通过Subject推送给本类
// 2. Subject响应函数UpdateSelf被调用，使本类成员变量与Model同步
// 3. 本类成员变量被修改，通过SyncVar同步到Client
// 4. 同步后，在Client上触发SyncVar的响应函数，修改对应flag
// 5. flag满足条件后，触发SyncMapChooseState，修改Model，使Client上的Model与本类成员变量同步
public class MapChooseStateSync : NetworkBehaviour {
    /// <summary>
    /// 选择地图，务必先同步本变量，再同步thumb和map
    /// </summary>
    [SyncVar(hook = "SyncMapFileName")] public string mapFileName;
    // 同步flag
    private bool mapFileNameFlag = false;
    
    /// <summary>
    /// 地图略缩图
    /// </summary>
    public SyncList<byte> thumb = new SyncList<byte>();
    // 同步flag
    private bool thumbFlag = false;
    
    /// <summary>
    /// 地图文件
    /// </summary>
    [SyncVar(hook = "SyncMap")] public string map;
    // 同步flag
    private bool mapFlag = false;
    
    private void Start() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateSelf);
        thumb.Callback += SyncThumb;
        UpdateSelf();
    }

    // mapChooseState修改时更新自身
    [Server]
    void UpdateSelf() {
        if (MapChooseState.Get().MapFileName != mapFileName) {
            mapFileName = MapChooseState.Get().MapFileName;
            thumb = new SyncList<byte>(SaveResource.saveManager.LoadThumb(mapFileName).texture.EncodeToPNG());
            map = SaveResource.saveManager.LoadMap(mapFileName).ToJson();
        }
    }

    // 将MapChooseState所需的信息同步。需要mapFileName，thumb和map都同步完成后再调用
    void SyncMapChooseState() {
        Debug.Log("尝试同步");
        Debug.Log(mapFileNameFlag);
        Debug.Log(thumbFlag);
        Debug.Log(thumb.Count);
        Debug.Log(mapFlag);
        if (mapFileNameFlag && thumbFlag && mapFlag) {
            MapChooseState.Get().MapFileName = mapFileName;
            mapFileNameFlag = thumbFlag = mapFlag = false;
            Debug.Log("同步地图：" + mapFileName);
        }
    }

    // 同步地图略缩图
    void SyncMapFileName(string oldValue, string newValue) {
        mapFileNameFlag = true;
        SyncMapChooseState();
    }
    
    // 同步地图略缩图
    void SyncThumb(SyncList<byte>.Operation op, int i, byte b1, byte b2) {
        if (mapFileName is null) return;
        SaveResource.saveManager.SaveThumb(thumb.ToArray(), mapFileName);
        thumbFlag = true;
        SyncMapChooseState();
    }
    
    // 同步地图
    void SyncMap(string oldValue, string newValue) {
        if (mapFileName is null) return;
        SaveResource.saveManager.SaveMap(SaveEntity.FromJson(newValue), mapFileName);
        mapFlag = true;
        SyncMapChooseState();
    }
}