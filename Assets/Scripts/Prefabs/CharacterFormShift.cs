using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换角色操控方式的按钮 </para>
///   <para> 功能1：面对用户。点击时触发响应函数。 </para>
///   <para> 功能2：面对Model。数据修改时，更新自身显示。 </para>
///   <para> 功能3：面对父对象。由上级控制锁定或解锁本按钮。 </para>
///   <para> 需要父对象指定enableLeft/RightClick、onLeft/RightClick。 </para>
/// </summary>
public class CharacterFormShift : MonoBehaviour
{
    // 切换按钮
    public Button button;

    // 选择栏的三个图标
    public Sprite[] playerSprites;

    // 是否允许左右键点击
    public bool enableLeftClick;
    public bool enableRightClick;

    // 点击响应函数
    public delegate void onClick();
    public onClick onLeftClick;
    public onClick onRightClick;

    // 创建时绑定响应函数
    void Start() {
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    ///   <para> 点击响应函数，按左右键进行回调。 </para>
    /// </summary>
    public void OnClick() {
        if(enableLeftClick && onLeftClick != null) {
            onLeftClick();
        }
        if(enableRightClick && onRightClick != null) {
            onRightClick();
        }
    }

    /// <summary>
    ///   <para> 锁定玩家。显示为Banned，按钮变灰。 </para>
    /// </summary>
    public void Lock() {
        button.interactable = false;
        UpdateDisplay();
    }

    /// <summary>
    ///   <para> 解锁玩家。按钮变为可用。 </para>
    /// </summary>
    public void UnlockPlayer(int player) {
        button.interactable = true;
        UpdateDisplay();
    }

    /// <summary>
    ///   <para> 根据Model更新显示。 </para>
    /// </summary>
    void UpdateDisplay() {
        // todo: 填充之
    }
}
