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
    public List<Cell> position;

    /// <summary>
    ///   <para> 操作的坐标 </para>
    /// </summary>
    public MapEditObject editObject;

    /// <summary>
    ///   <para> 操作前的状态 </para>
    /// </summary>
    public List<Object> pre;

    /// <summary>
    ///   <para> 操作后的状态 </para>
    /// </summary>
    public List<Object> after;
}