using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> 一级操作菜单 </para>
/// </summary>
public class OperateMenu : MonoBehaviour {
    private bool toSave = false;

    //网格
    [SerializeField] private HexGridDisplay hexGridDisplay;
    //显示网格开关
    [SerializeField] private Button hexGridVisible;

    void Start() {
        // 默认显示网格
        hexGridDisplay.Visible = false;
        SwitchHexGridVisible();
    }

    void Update() {
        // 若截图好了，保存之
        if(toSave) {
            byte[] capture = SaveResource.saveLoader.ScreenShot;
            if( !(capture is null)) {
                // 获取文件名
                string filename = MapEditResource.mapFilename.Length == 0 ? "save" : MapEditResource.mapFilename;
                // 保存
                SaveResource.saveManager.SaveMap(SaveResource.saveLoader.Save(filename), filename);
                SaveResource.saveManager.SaveThumb(capture, filename);
                toSave = false;
                Debug.Log("保存成功");
            }
        }
    }

    public void Undo() {
        MapEditResource.momentoController.Undo();
    }
    public void Redo() {
        MapEditResource.momentoController.Redo();
    }

    /// <summary>
    ///   <para> 退出地图编辑器 </para>
    /// </summary>
    public void Exit() {
        SceneManager.LoadScene("Entrance");
    }

    /// <summary>
    ///   <para> 显示网格 </para>
    ///   <para> 是switchHexGridVisible的响应函数 </para>
    /// </summary>
    public void SwitchHexGridVisible() {
        hexGridDisplay.Visible = !hexGridDisplay.Visible;
        hexGridVisible.gameObject.GetComponent<Image>().color = 
            hexGridDisplay.Visible ? MapEditResource.highlightColor : MapEditResource.defaultColor;
    }

    /// <summary>
    ///   <para> 存储到文件 </para>
    /// </summary>
    public void Save() {
        toSave = true;
        SaveResource.saveLoader.Capture();
    }
}