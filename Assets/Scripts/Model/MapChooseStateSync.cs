using System;
using System.Linq;
using Mirror;
using UnityEngine;

/// <summary>
/// 用于同步Model
/// </summary>
public class MapChooseStateSync : NetworkBehaviour {
    private void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_File_Name, Sync);
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Client_Success, Sync);
    }

    // 将数据同步到Client
    void Sync() {
        Debug.Log("尝试同步");
        MapChooseState model = MapChooseState.Get();
        string mapFileName = model.MapFileName;
        RpcSyncFileName(mapFileName);
        // string map = SaveResource.saveManager.LoadMap(mapFileName).ToJson();
        // byte[] thumb = SaveResource.saveManager.LoadThumb(mapFileName).texture.EncodeToPNG();
        // RpcSetModel(mapFileName, map, thumb);
    }

    // mapChooseState修改时更新自身
    [ClientRpc]
    void RpcSetModel(string mapFileName, string map, byte[] thumb) {
        MapChooseState model = MapChooseState.Get();
        SaveResource.saveManager.SaveMap(SaveEntity.FromJson(map), mapFileName);
        SaveResource.saveManager.SaveThumb(thumb.ToArray(), mapFileName);
        
        // 最后同步mapFileName，因为更新时需要读取thumb和map
        // 如果已经相等就不同步了，以防Host死循环
        // 不在开始判断相等，因为可能出现两端文件名相等，而内容不同的情况
        if (model.MapFileName != mapFileName) 
            model.MapFileName = mapFileName;
        
        Debug.Log("地图同步成功：" + mapFileName);
    }

    [ClientRpc]
    void RpcSyncFileName(string mapFileName) {
        MapChooseState model = MapChooseState.Get();
        if (model.MapFileName != mapFileName) 
            model.MapFileName = mapFileName;
    }
}