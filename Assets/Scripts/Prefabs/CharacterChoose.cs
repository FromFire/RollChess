using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 选择角色操控方式和玩家 </para>
///   <para> 本组件读取Model！ </para>
///   <para> 子组件：CharacterFormShift </para>
/// </summary>
public class CharacterChoose : MonoBehaviour
{
    // 切换按钮
    public List<CharacterFormShift> buttons;

    // 人数限制
    public Text playerLimit;

    // 初始化
    void Start() {
        // todo: 绑定CharacterFormShift的点击响应函数
    }

    /// <summary>
    ///   <para> 显示玩家数量限制 </para>
    /// </summary>
    public void SetPlayerLimit(int min, int max) {
        // 显示玩家数量限制
        if(min == max) {
            playerLimit.text = min + "人";
        } else {
            playerLimit.text = min + "-" + max + "人";
        }

        // 锁定和解锁玩家
        for(int i=0; i<buttons.Count; i++) {
            if(i<max) {
                buttons[i].Unlock();
                // todo: buttons[i].SetForm
            } else {
                buttons[i].Lock();
            }
        }
    }

}
