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

    /// <summary>
    ///   <para> 数据上边界 </para>
    /// </summary>
    public int borderUp {get;}

    /// <summary>
    ///   <para> 数据下边界 </para>
    /// </summary>
    public int borderDown {get;}

    /// <summary>
    ///   <para> 数据左边界 </para>
    /// </summary>
    public int borderLeft {get;}

    /// <summary>
    ///   <para> 数据右边界 </para>
    /// </summary>
    public int borderRight {get;}

    // todo: subject 更新推送

    /// <summary>
    ///   <para> 初始化空的列表 </para>
    /// </summary>
    public void Init() {

    }

    /// <summary>
    ///   <para> 包含性检查 </para>
    /// </summary>
    public bool Contains(Vector2Int pos) {
        return positionOfCell.ContainsKey(pos);
    }

    /// <summary>
    ///   <para> 设置和增加Cell </para>
    /// </summary>
    public void Add(Vector2Int pos, Cell cell) {

    }

    /// <summary>
    ///   <para> 删除Cell </para>
    /// </summary>
    public void Remove(Vector2Int pos) {

    }

    /// <summary>
    ///   <para> 获取数据 </para>
    /// </summary>
    public Cell Get(Vector2Int pos) {
        return null;
    }

    /// <summary>
    ///   <para> 导出为Set格式 </para>
    /// </summary>
    public HashSet<Vector2Int> ToPositionSet() {
        return null;
    }

    /// <summary>
    ///   <para> 更新边界 </para>
    /// </summary>
    private void UpdateBorder(Vector2Int pos) {

    }
}