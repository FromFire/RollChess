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
    // 地图人数限制
    private int _limitMax;
    private int _limitMin;

    // 切换按钮
    [SerializeField] List<CharacterFormShift> buttons;

    // 人数限制显示
    [SerializeField] Text playerLimit;

    // 初始化
    void Start() {
        // todo: 绑定CharacterFormShift的点击响应函数
    }

    /// <summary>
    ///   <para> 人数限制上限 </para>
    /// </summary>
    public int LimitMax {
        get {return _limitMax;}
        set {
            _limitMax = value;
            // 保证Min<Max
            if(_limitMin > _limitMax) {
                _limitMin = _limitMax;
            }
            //更新显示
            UpdateLimitLabel();
            UpdateLock();
        }
    }

    /// <summary>
    ///   <para> 人数限制下限 </para>
    /// </summary>
    public int LimitMin {
        get {return _limitMin;}
        set {
            _limitMin = value;
            // 保证Min<Max
            if(_limitMin > _limitMax) {
                _limitMax = _limitMin;
            }
            //更新显示
            UpdateLimitLabel();
            UpdateLock();
        }
    }

    // 更新玩家数量限制显示
    void UpdateLimitLabel() {
        if(_limitMin == _limitMax) {
            playerLimit.text = _limitMin + "人";
        } else {
            playerLimit.text = _limitMin + "-" + _limitMax + "人";
        }
    }

    // 更新按钮锁定情况
    void UpdateLock() {
        for(int i=0; i<buttons.Count; i++) {
            // Max之前的解锁，之后的锁定
            buttons[i].IsLocked = (i >= _limitMax);
        }
    }

}
