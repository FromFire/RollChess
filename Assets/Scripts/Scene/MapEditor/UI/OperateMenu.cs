using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 一级操作菜单 </para>
/// </summary>
public class OperateMenu : MonoBehaviour {

    //网格
    [SerializeField] private HexGridDisplay hexGridDisplay;
    //显示网格开关
    [SerializeField] private Button hexGridVisible;

    void Start() {
        // 默认显示网格
        hexGridDisplay.Visible = false;
        SwitchHexGridVisible();
    }

    public void Undo() {
        MapEditResource.momentoController.Undo();
    }
    public void Redo() {
        MapEditResource.momentoController.Redo();
    }

    /// <summary>
    ///   <para> 显示网格 </para>
    ///   <para> 是switchHexGridVisible的响应函数 </para>
    /// </summary>
    public void SwitchHexGridVisible() {
        hexGridDisplay.Visible = !hexGridDisplay.Visible;
        hexGridVisible.gameObject.GetComponent<Image>().color = 
            hexGridDisplay.Visible ? MapEditResource.highlightColor : MapEditResource.defaultColor;
        Debug.Log(hexGridDisplay.Visible);
    }
}