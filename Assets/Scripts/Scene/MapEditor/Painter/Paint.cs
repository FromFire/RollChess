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
    ///   <para> 擦除块 </para>
    /// </summary>
    EditMomento Erase(Vector2Int position);

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
    ///   <para> 绘制块 </para>
    /// </summary>
    EditMomento PaintBlock(Vector2Int position);

    /// <summary>
    ///   <para> 擦除块 </para>
    /// </summary>
    EditMomento EraseBlock(Vector2Int position);
}