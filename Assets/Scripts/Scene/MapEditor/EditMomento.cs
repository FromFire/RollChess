using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 绘制记录 </para>
/// </summary>
public class EditMomento {

    /// <summary>
    ///   <para> 操作的坐标 </para>
    /// </summary>
    public List<Vector2Int> position;

    /// <summary>
    ///   <para> 操作的类型 </para>
    /// </summary>
    public MapEditObject editObject;

    /// <summary>
    ///   <para> 操作前的状态 </para>
    /// </summary>
    public List<Object> pre = new List<Object>();

    /// <summary>
    ///   <para> 操作后的状态 </para>
    /// </summary>
    public List<Object> after = new List<Object>();

    public EditMomento() {}

    /// <summary>
    ///   <para> 复制 </para>
    /// </summary>
    public EditMomento(EditMomento momento) {
        for(int i=0; i<position.Count; i++) {
            position.Add(momento.position[i]);
            pre.Add(momento.pre[i]);
            after.Add(momento.after[i]);
        }
        editObject = momento.editObject;
    }
}