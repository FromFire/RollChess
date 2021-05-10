using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 特殊块绘制 </para>
/// </summary>
public class SpecialEditor : MonoBehaviour, Paint, BlockPaint {
    
    /// <summary>
    ///   <para> 绘制块 </para>
    /// </summary>
    public EditMomento Paint(Vector2Int position) {
        return new EditMomento();
    }

    /// <summary>
    ///   <para> 擦除块 </para>
    /// </summary>
    public EditMomento Erase(Vector2Int position) {
        return new EditMomento();
    }

    /// <summary>
    ///   <para> 撤销操作 </para>
    /// </summary>
    public void Undo(EditMomento momento) {

    }

    /// <summary>
    ///   <para> 重做操作 </para>
    /// </summary>
    public void Redo(EditMomento momento) {

    }

    /// <summary>
    ///   <para> 绘制块 </para>
    /// </summary>
    public EditMomento Preview(Vector2Int position) {
        return new EditMomento();
    }

    /// <summary>
    ///   <para> 擦除块 </para>
    /// </summary>
    public EditMomento PaintBlock(Vector2Int position) {
        return new EditMomento();
    }
}