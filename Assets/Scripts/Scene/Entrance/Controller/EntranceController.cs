using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 地图选择中的一项 </para>
/// </summary>
public class EntranceController : MonoBehaviour {

    void Start() {
        SaveResource.saveManager.LoadAllSave();
        Debug.Log(SaveResource.saveManager.saveEntities.Count);
    }

    /// <summary>
    ///   <para> 选择地图 </para>
    /// </summary>
    public void Choose(string filename) {

    }
}