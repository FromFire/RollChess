using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 地图选择中的一项 </para>
/// </summary>
public class MapItem : MonoBehaviour {

    // 地图文件名
    private string filename;

    // 略缩图显示
    [SerializeField] private Image image;

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Show() {
        image.sprite = SaveResource.saveManager.LoadThumb(filename);
    }

    /// <summary>
    ///   <para> 选择此项，是本按钮的点击函数 </para>
    /// </summary>
    public void Choose() {
        EntranceResource.mapOperationController.Choose(filename);
    }

    /// <summary>
    ///   <para> 选择此项，是本按钮的点击函数 </para>
    /// </summary>
    public string Filename {
        get {return filename;}
        set {
            filename = value;
            Show();
        }
    }
}