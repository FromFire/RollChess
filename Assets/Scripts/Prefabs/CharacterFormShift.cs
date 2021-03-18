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
    ///   <para> 锁定玩家。显示为Banned，按钮变灰。 </para>
    /// </summary>
    public void Lock() {
        button.interactable = false;
        SetForm(PlayerForm.Banned);
    }

    /// <summary>
    ///   <para> 解锁玩家。按钮变为可用。 </para>
    ///   <para> 注意：解锁后须使用UpdateDisplay指定 </para>
    /// </summary>
    public void Unlock() {
        button.interactable = true;
    }

    /// <summary>
    ///   <para> 设置显示的PlayerForm。 </para>
    /// </summary>
    public void SetForm(PlayerForm playerForm) {
        image.sprite = playerSprites[(int)playerForm];
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
