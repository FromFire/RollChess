using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 选择角色操控方式和玩家 </para>
///   <para> 本组件读取Model！ </para>
///   <para> 子组件：CharacterFormShift </para>
/// </summary>
public class ChooseCharacter : MonoBehaviour
{
    // 切换按钮
    [SerializeField] List<CharacterFormShift> buttons;

    // 人数限制显示
    [SerializeField] Text playerLimit;

    // 初始化
    void Start() {
        ModelResource.mapChooseSubject.Attach(ModelModifyEvent.Player_Limit, UpdateSelf);
    }

    /// <summary>
    ///   <para> 更新自身显示 </para>
    /// </summary>
    public void UpdateSelf() {
        // 获取人数限制
        (int min, int max) limit = EntranceResource.mapChooseState.PlayerLimit;
        int max = limit.max;
        int min = limit.min;

        // 更新标签显示
        if(min == max) {
            playerLimit.text = min + "人";
        } else {
            playerLimit.text = min + "-" + max + "人";
        }

        // 更新按钮锁定
        for(int i=0; i<buttons.Count; i++) {
            // Max之前的解锁，之后的锁定
            buttons[i].IsLocked = (i >= max);
        }
    }
}
