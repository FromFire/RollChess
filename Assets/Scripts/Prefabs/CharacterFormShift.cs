using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换角色操控方式的按钮 </para>
///   <para> 功能1：面对用户。点击时触发响应函数，并切换显示。 </para>
///   <para> 功能2：面对父对象。由上级控制锁定或解锁本按钮。 </para>
///   <para> 需要父对象指定onLeft/RightClick。 </para>
/// </summary>
public class CharacterFormShift : MonoBehaviour
{
    // 当前的操控方式，在锁定状态下存储锁定前的选择
    PlayerForm currentPlayerForm;

    // 是否已锁定
    bool isLocked;

    // 切换按钮
    public Button button;

    // 选择栏的三个图标
    public List<Sprite> playerSprites;

    // 图标
    public Image image;

    // 点击响应函数
    public delegate void onClick();
    public onClick onLeftClick;
    public onClick onRightClick;

    // 创建时绑定响应函数
    void Start() {
        button.onClick.AddListener(OnClick);
    }

    /// <summary>
    ///   <para> 读写currentPlayerForm </para>
    /// </summary>
    public PlayerForm CurrentPlayerForm {
        set {
            currentPlayerForm = value;
            // 更新显示
            image.sprite = playerSprites[(int)currentPlayerForm];
        }
        get {return currentPlayerForm;}
    }

    /// <summary>
    ///   <para> 锁定和解锁按钮 </para>
    /// </summary>
    public bool IsLocked {
        get {return isLocked;}
        set {
            // 锁定时显示为Banned，按钮变灰。
            // 解锁时恢复原本选项，按钮变为可用。
            button.interactable = isLocked = value;
            CurrentPlayerForm = isLocked ? PlayerForm.Banned : currentPlayerForm;
        }
    }

    /// <summary>
    ///   <para> 点击响应函数，按左右键进行回调。 </para>
    /// </summary>
    void OnClick() {
        // 左键切换操控方式（用于单机和host）
        if(onLeftClick != null && Input.GetMouseButtonDown(0)) {
            onLeftClick();
            // 修改显示，轮换至下一PlayerForm
            for(int i=0; i<playerSprites.Count; i++) {
                if(image.sprite == playerSprites[i]) {
                    image.sprite = playerSprites[(i+1)%playerSprites.Count];
                }
            }
        }
        // 右键选择操控该角色（仅用于Client）
        if(onRightClick != null && Input.GetMouseButtonDown(1)) {
            onRightClick();
        }
    }
}
