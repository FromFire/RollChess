using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 绘制器 </para>
/// </summary>
public interface Paint {

    /// <summary>
    ///   <para> 绘制块 </para>
    /// </summary>
    EditMomento Paint(Vector2Int position);

    /// <summary>
    ///   <para> 撤销操作 </para>
    /// </summary>
    void Undo(EditMomento momento);

    /// <summary>
    ///   <para> 重做操作 </para>
    /// </summary>
    void Redo(EditMomento momento);
}

/// <summary>
///   <para> 批量绘制器 </para>
/// </summary>
public interface BlockPaint {
    
    /// <summary>
    ///   <para> 向正在画的一笔中加入新格子，然后预览已经绘制的部分 </para>
    /// </summary>
    void Preview(Vector2Int position);

    /// <summary>
    ///   <para> 获取正在画的一笔中有多少格 </para>
    /// </summary>
    int BlockCount();

    /// <summary>
    ///   <para> 获取上一格的坐标 </para>
    ///   <para> 若无上一格，返回xy均为int.MaxValue </para>
    /// </summary>
    Vector2Int LastPosition();

    /// <summary>
    ///   <para> 绘制当前的块 </para>
    /// </summary>
    EditMomento PaintBlock();
}