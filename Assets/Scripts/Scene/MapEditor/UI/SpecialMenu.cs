using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换特殊块和传送门笔刷 </para>
/// </summary>
public class SpecialMenu : MonoBehaviour {

    // 切换玩家
    [SerializeField] private Button brokenBridge;
    [SerializeField] private Button doubleStep;
    [SerializeField] private Button erase;

    void Start() {}

    /// <summary>
    ///   <para> 切换特殊块 </para>
    /// </summary>
    public void SwitchToBrokenBridge() {
        MapEditResource.specialEditor.Effect = SpecialEffect.Broken_Bridge;
        MapEditResource.paintController.EditObject = MapEditObject.Special;
    }
    public void SwitchToDoubleStep() {
        MapEditResource.specialEditor.Effect = SpecialEffect.Double_Step;
        MapEditResource.paintController.EditObject = MapEditObject.Special;
    }
    public void SwitchToErase() {
        MapEditResource.specialEditor.Effect = SpecialEffect.None;
        MapEditResource.paintController.EditObject = MapEditObject.Special;
    }
}