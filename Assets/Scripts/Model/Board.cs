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

    // 数据边界
    private int borderUp;
    private int borderDown;
    private int borderLeft;
    private int borderRight;

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
        positionOfCell.Add(pos, cell);
        UpdateBorder(pos);
    }

    /// <summary>
    ///   <para> 删除Cell </para>
    /// </summary>
    public void Remove(Vector2Int pos) {
        if(Contains(pos)) positionOfCell.Remove(pos);
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
        HashSet<Vector2Int> ret = new HashSet<Vector2Int>();
        foreach(KeyValuePair<Vector2Int, Cell> kvp in positionOfCell) {
            ret.Add(kvp.Key);
        }
        return ret;
    }

    /// <summary>
    ///   <para> 更新边界 </para>
    /// </summary>
    private void UpdateBorder(Vector2Int pos) {
        borderLeft = System.Math.Min(borderLeft, pos.x);
        borderRight = System.Math.Max(borderRight, pos.x);
        borderUp = System.Math.Max(borderUp, pos.y);
        borderDown = System.Math.Min(borderDown, pos.y);
    }

    // 获取边界
    public int BorderUp { get{ return borderUp; } }
    public int BorderDown { get{ return borderDown; } }
    public int BorderLeft { get{ return borderLeft; } }
    public int BorderRight { get{ return borderRight; } }
}