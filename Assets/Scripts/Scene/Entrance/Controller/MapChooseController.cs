using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> 地图选择管理 </para>
/// </summary>
public class MapOperationController : MonoBehaviour
{
    /// <summary>
    ///   <para> 判断地图是否合法 </para>
    /// </summary>
    public bool IsMapValid() {
        //获取maps
        string mapName = EntranceResource.mapChooseState.MapFileName;
        //地图不能为空
        if(mapName.Length <= 1) {
            WarningManager.errors.Add(new WarningModel("请选择地图！"));
            return false;
        }

        return false;
    }
    
    /// <summary>
    ///   <para> 选择地图 </para>
    /// </summary>
    public void Choose(string filename) {
        EntranceResource.mapChooseState.MapFileName = filename;
    }
    
    /// <summary>
    ///   <para> 复制地图 </para>
    /// </summary>
    public string CopyMap() {
        // 复制
        string origin = EntranceResource.mapChooseState.MapFileName;
        string copy = SaveResource.saveManager.Duplicate(origin);
        // 地图名称改为<地图名>-副本
        SaveEntity saveEntity = SaveResource.saveManager.LoadMap(copy);
        saveEntity.mapName = EntranceResource.mapChooseState.MapName + "-副本";
        SaveResource.saveManager.SaveMap(saveEntity, copy);
        return copy;
    }

    /// <summary>
    ///   <para> 编辑地图 </para>
    /// </summary>
    public void EditMap() {
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("MapEditor");
    }

    /// <summary>
    ///   <para> 删除地图 </para>
    /// </summary>
    public void DeleteMap() {
        // 地图为空的情况
        string toDel = EntranceResource.mapChooseState.MapFileName;
        SaveResource.saveManager.Delete(EntranceResource.mapChooseState.MapFileName);
    }

    /// <summary>
    ///   <para> 准备新建地图 </para>
    /// </summary>
    public void PrepareNewMap() {
        EntranceResource.mapChooseState.MapFileName = "";
    }

    /// <summary>
    ///   <para> 新建地图 </para>
    /// </summary>
    public void NewMap(string mapName, int min, int max) {
        // 合法性检查
        if(mapName.Length == 0 || min <= 0  || max < min || max > 4)
            return;
        
        // 创建新地图
        SaveEntity saveEntity = new SaveEntity();
        string filename = SaveResource.saveManager.NewMap(saveEntity, mapName);

        // 修改基本属性
        saveEntity = SaveResource.saveManager.LoadMap(filename);
        saveEntity.mapName = mapName;
        saveEntity.player.min = min;
        saveEntity.player.max = max;
        SaveResource.saveManager.SaveMap(saveEntity, filename);

        // 修改MapChooseState
        EntranceResource.mapChooseState.MapFileName = filename;

        // 进入地图编辑器
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("MapEditor");
    }

    /// <summary>
    ///   <para> 修改地图基本信息 </para>
    /// </summary>
    public void ModifySetting(string mapName, int min, int max) {
        // 合法性检查
        if(mapName.Length == 0 || min <= 0  || max < min || max > 4)
            return;

        // 修改
        string filename = EntranceResource.mapChooseState.MapFileName;
        SaveEntity saveEntity = SaveResource.saveManager.LoadMap(filename);
        saveEntity.mapName = mapName;
        saveEntity.player.min = min;
        saveEntity.player.max = max;

        // 修改状态
        EntranceResource.mapChooseState.MapName = mapName;
        EntranceResource.mapChooseState.PlayerLimit = (min, max);

        // 保存
        SaveResource.saveManager.SaveMap(saveEntity, filename);
    }
}