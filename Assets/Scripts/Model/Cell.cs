using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 棋盘的一个格子 </para>
/// </summary>
public class Cell {
    // 所在坐标
    protected Vector2Int position;

    // 是否可放置棋子
    protected bool walkable;

    // 格子的特殊效果
    protected SpecialEffect effect;

    /// <summary>
    ///   <para> 更新推送 </para>
    /// </summary>
    public PositionSubject subject;

    // 构造函数
    public Cell() {
        walkable = false;
        effect = SpecialEffect.None;
    }

    // 构造函数
    public Cell(Cell cell) {
        position = cell.position;
    }

    // 构造函数
    public Cell(bool walkable, SpecialEffect effect) {
        this.walkable = walkable;
        this.effect = effect;
    }

    /// <summary>
    ///   <para> 所在坐标 </para>
    /// </summary>
    public Vector2Int Position {
        get {return position;}
        set {
            position = value;
            //推送修改
            subject.Notify(ModelModifyEvent.Cell, position); 
        }
    }

    /// <summary>
    ///   <para> 是否可放置棋子 </para>
    /// </summary>
    public bool Walkable {
        get {return walkable;}
        set {
            walkable = value;
            //推送修改
            subject.Notify(ModelModifyEvent.Cell, position); 
        }
    }

    /// <summary>
    ///   <para> 是否可放置棋子 </para>
    /// </summary>
    public SpecialEffect Effect {
        get {return effect;}
        set {
            effect = value;
            //推送修改
            subject.Notify(ModelModifyEvent.Cell, position); 
        }
    }
}

/// <summary>
///   <para> 传送门型格子 </para>
/// </summary>
public class PortalCell : Cell {

    // 传送目的地
    private Vector2Int target;

    // 构造函数
    public PortalCell(Cell cell, Vector2Int target) : base(cell) {
        this.target = target;
    }

    // 构造函数
    public PortalCell(bool walkable, SpecialEffect effect, Vector2Int target) : base(walkable, effect) {
        this.target = target;
    }

    /// <summary>
    ///   <para> 传送目的地 </para>
    /// </summary>
    public Vector2Int Target {
        get {return target;}
        set {
            target = value;
            //推送修改
            subject.Notify(ModelModifyEvent.Cell, position); 
        }
    }
}