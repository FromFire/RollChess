using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 单人玩家选地图 </para>
/// </summary>
public class PanelManager : MonoBehaviour {
    // 单例
    static PanelManager menuManager;

    // 当前显示的页面
    private Image nowPanel;

    // 主菜单
    public Image mainMenu;
    // 单人页面
    public Image single;
    // 地图编辑页面
    public Image mapEdit;
    // 创建和加入房间页面
    public Image joinRoom;
    // 房主选地图页面
    public Image roomOwnerChooseMap;
    // 多人玩家选角色页面
    public Image room;

    // 返回按钮：【不显示】主菜单
    [SerializeField] private Button backButton;
    // 地图预览：【不显示】主菜单、创建房间
    [SerializeField] private MapPreview mapPreview;
    // 地图选择：【显示】单人、地图编辑、房主选地图
    [SerializeField] private MapMenu mapMenu;
    // 角色选择：【显示】单人、房主选地图、多人选角色
    [SerializeField] private ChooseCharacter chooseCharacter;

    // 初始化，页面最开始是主菜单
    void Start() {
        NowPanel = mainMenu;
        menuManager = this;
    }

    /// <summary>
    ///   <para> 获取单例 </para>
    /// </summary>
    public static PanelManager Get() {
        return menuManager;
    }

    /// <summary>
    ///   <para> 点击返回按钮 </para>
    /// </summary>
    public void Back() {
        // 单人页 / 地图编辑页 / 创建房间 -> 主页
        if(nowPanel == single || nowPanel == mapEdit || nowPanel == joinRoom)
            NowPanel = mainMenu;
    }

    /// <summary>
    ///   <para> 当前显示的页面 </para>
    /// </summary>
    public Image NowPanel {
        get {return nowPanel;}
        set {
            if(!(nowPanel is null))
                nowPanel.gameObject.SetActive(false);
            nowPanel = value;
            nowPanel.gameObject.SetActive(true);

            // 返回按钮、地图预览、地图选择
            backButton.gameObject.SetActive(nowPanel != mainMenu);
            mapPreview.gameObject.SetActive(nowPanel != mainMenu && nowPanel != joinRoom);
            mapMenu.gameObject.SetActive(nowPanel == single || nowPanel == mapEdit || nowPanel == roomOwnerChooseMap);
            chooseCharacter.gameObject.SetActive(nowPanel == single || nowPanel == roomOwnerChooseMap || nowPanel == room);
        }
    }
}