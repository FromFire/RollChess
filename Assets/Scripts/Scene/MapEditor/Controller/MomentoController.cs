using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 操作管理，可以撤销、重做操作 </para>
/// </summary>
public class MomentoController : MonoBehaviour {

    // 已完成的操作
    private Stack<EditMomento> done = new Stack<EditMomento>();

    // 已完成的操作
    private Stack<EditMomento> undone = new Stack<EditMomento>();

    /// <summary>
    ///   <para> 撤销操作 </para>
    /// </summary>
    public void Undo() {
        Debug.Log("undo");
        // 无操作可撤销
        if(done.Count == 0)
            return;

        // 让相应类型的Paint执行
        EditMomento momento = done.Pop();
        Paint paint = MapEditResource.EditObjectToPaint[momento.editObject];
        paint.Undo(momento);

        // 更新undone
        undone.Push(momento);
    }

    /// <summary>
    ///   <para> 重做操作 </para>
    /// </summary>
    public void Redo() {
        Debug.Log("redo");
        // 无操作可重做
        if(undone.Count == 0)
            return;

        // 让相应类型的Paint执行
        EditMomento momento = undone.Pop();
        Paint paint = MapEditResource.EditObjectToPaint[momento.editObject];
        paint.Redo(momento);

        // 更新done
        done.Push(momento);
    }

    /// <summary>
    ///   <para> 记录已完成的操作 </para>
    /// </summary>
    public void Record(EditMomento momento) {
        done.Push(momento);
        // 清空redo
        undone.Clear();
    }
}