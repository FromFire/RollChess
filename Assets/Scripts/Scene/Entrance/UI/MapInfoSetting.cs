using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 设置地图的基本信息：地图名称 + 人数限制 </para>
/// </summary>
public class MapInfoSetting : MonoBehaviour
{
    // 遮挡
    [SerializeField] Image mask;

    // 地图名
    [SerializeField] InputField mapName;
    // 人数下限
    [SerializeField] InputField limitMin;
    // 人数上限
    [SerializeField] InputField limitMax;

    // 初始化
    void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_File_Name, UpdateSelf);
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Map_Name, UpdateSelf);
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Player_Limit, UpdateSelf);
        SetLock(true);
    }

    /// <summary>
    ///   <para> 根据Model显示 </para>
    /// </summary>
    public void UpdateSelf() {
        // 若没有地图，锁定
        if(EntranceResource.mapChooseState.MapFileName.Length == 0) {
            SetLock(true);
            return;
        }

        // 地图合法，显示其信息
        SetLock(false);
        mapName.text = EntranceResource.mapChooseState.MapName;
        limitMin.text = EntranceResource.mapChooseState.PlayerLimit.min + "";
        limitMax.text = EntranceResource.mapChooseState.PlayerLimit.max + "";
    }

    /// <summary>
    ///   <para> 锁定或解锁 </para>
    /// </summary>
    public void SetLock(bool tolock) {
        // 锁定
        if(tolock) {
            mask.gameObject.SetActive(true);
            mapName.text = "";
            limitMin.text = "";
            limitMax.text = "";
        } 
        
        // 解锁
        else {
            mask.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///   <para> 确定修改 </para>
    /// </summary>
    public void Confirm() {
        // 获取人数限制
        int min, max;
        int.TryParse(limitMin.text, out min);
        int.TryParse(limitMax.text, out max);

        EntranceResource.entranceController.ModifySetting(mapName.text, min, max);
    }
}
