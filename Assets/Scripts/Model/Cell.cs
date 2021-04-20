using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 棋盘的一个格子 </para>
/// </summary>
public class Cell {

    /// <summary>
    ///   <para> 是否可放置棋子 </para>
    /// </summary>
    public bool walkable;

    /// <summary>
    ///   <para> 是否可放置棋子 </para>
    /// </summary>
    public SpecialEffect effect;

    public Cell() {
        walkable = false;
        effect = SpecialEffect.None;
    }

    public Cell(bool walkable, SpecialEffect effect) {
        this.walkable = walkable;
        this.effect = effect;
    }
}

/// <summary>
///   <para> 传送门型格子 </para>
/// </summary>
public class PortalCell : Cell {

    /// <summary>
    ///   <para> 传送目的地 </para>
    /// </summary>
    public Vector2Int target;

    public PortalCell(Cell cell, Vector2Int target) : base(cell.walkable, cell.effect) {
        this.target = target;
    }

    public PortalCell(bool walkable, SpecialEffect effect, Vector2Int target) : base(walkable, effect) {
        this.target = target;
    }
}