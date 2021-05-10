using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 绘制记录管理 </para>
/// </summary>
public class EditCareTaker {

    // 操作记录
    private Stack<EditMomento> momentos = new Stack<EditMomento>();

    // 记录数量上限
    private int maxMomento;

    /// <summary>
    ///   <para> 获取最新记录 </para>
    /// </summary>
    public EditMomento Peek() {
        return momentos.Peek();
    }

    /// <summary>
    ///   <para> 获取最新记录，并使之出栈 </para>
    /// </summary>
    public EditMomento Pop() {
        return momentos.Pop();
    }

    /// <summary>
    ///   <para> 添加记录 </para>
    /// </summary>
    public void Push(EditMomento momento) {
        momentos.Push(momento);
    }

    /// <summary>
    ///   <para> 清除所有记录 </para>
    /// </summary>
    public void Clear() {
        momentos.Clear();
    }

    /// <summary>
    ///   <para> 获取记录数量 </para>
    /// </summary>
    public int Count() {
        return momentos.Count;
    }
}