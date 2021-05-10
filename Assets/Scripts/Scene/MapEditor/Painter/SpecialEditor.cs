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
    ///   <para> 执行momento上的操作 </para>
    /// </summary>
    void Execute(EditMomento momento) {
    }

    /// <summary>
    ///   <para> 向正在画的一笔中加入新格子，然后预览已经绘制的部分 </para>
    ///   <para> 注意：Model会被修改！ </para>
    /// </summary>
    public void Preview(Vector2Int position) {
    }

    /// <summary>
    ///   <para> 完成这一笔 </para>
    /// </summary>
    public EditMomento PaintBlock() {

        return new EditMomento();
    }

    /// <summary>
    ///   <para> 获取正在画的一笔中有多少格 </para>
    /// </summary>
    public int BlockCount() {
        return 0;
    }

    /// <summary>
    ///   <para> 获取上一格的坐标 </para>
    ///   <para> 若无上一格，返回xy均为int.MaxValue </para>
    /// </summary>
    public Vector2Int LastPosition() {
        return new Vector2Int(int.MaxValue, int.MaxValue);
    }
}