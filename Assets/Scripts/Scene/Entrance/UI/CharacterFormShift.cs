using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换角色操控方式的按钮，可以锁定或解锁按钮 </para>
/// </summary>
public class CharacterFormShift : MonoBehaviour
{
    // 自身的PlayerID
    [SerializeField] PlayerID playerID;

    // 是否已锁定
    private bool _isLocked;
    
    // 是否是多人模式
    public bool isMultiplePlayer = true;

    // 切换按钮
    [SerializeField] Button button;
    // 图标
    [SerializeField] Image image;
    // 选择栏的三个图标
    [SerializeField] List<Sprite> playerSprites;
    
    // 上级的ChooseCharacter
    [SerializeField] private ChooseCharacter chooseCharacter;

    void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Player_Form, UpdateSelf);
    }

    /// <summary>
    ///   <para> 更新自身显示 </para>
    /// </summary>
    public void UpdateSelf() {
        PlayerForm form = MapChooseState.Get().GetPlayerForm(playerID);
        Show(form);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Show(PlayerForm form) {
        image.sprite = playerSprites[(int)form];
    }

    /// <summary>
    ///   <para> 按钮锁定控制 </para>
    ///   <para> 锁定时显示为Banned，按钮变灰。 </para>
    ///   <para> 解锁时恢复原本选项，按钮变为可用。 </para>
    /// </summary>
    public bool IsLocked {
        get {return _isLocked;}
        set {
            _isLocked = value;
            button.interactable = !_isLocked;
            if(_isLocked)
                Show(PlayerForm.Banned);
            else
                UpdateSelf();
        }
    }

    /// <summary>
    ///   <para> 点击响应函数 </para>
    /// </summary>
    public void OnClick() {
        if(isMultiplePlayer)
            chooseCharacter.Choose(playerID);
        else
            PlayerOperationController.Get().ShiftPlayerFrom(playerID);
    }
}
