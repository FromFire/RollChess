using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structure;

/// <summary>
///   <para> 棋盘数据 </para>
/// </summary>
public class Board {

    /// <summary>
    ///   <para> 存储坐标和格子 </para>
    /// </summary>
    private Dictionary<Vector2Int, Cell> positionOfCell;

    public int borderUp {get;}
}