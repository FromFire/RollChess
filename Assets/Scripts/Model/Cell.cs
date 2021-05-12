using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 棋盘的一个格子 </para>
/// </summary>
public class Cell : Object{
    // 所在坐标
    // 可以获取，不能修改，只能在初始化时指定
    protected Vector2Int position;

    // 是否可放置棋子
    protected bool walkable;

    // 格子的特殊效果
    protected SpecialEffect effect;

    // 不能构造空的Cell
    private Cell() {}

    // 构造函数
    public Cell(Cell cell) {
        position = cell.position;
        walkable = cell.walkable;
        effect = cell.effect;
    }

    // 构造函数
    public Cell(Vector2Int position, bool walkable, SpecialEffect effect) {
        this.position = position;
        this.walkable = walkable;
        this.effect = effect;
    }

    /// <summary>
    ///   <para> 所在坐标 </para>
    ///   <para> 可以获取，不能修改，只能在初始化时指定 </para>
    /// </summary>
    public Vector2Int Position {
        get {return position;}
    }

    /// <summary>
    ///   <para> 是否可放置棋子 </para>
    /// </summary>
    public bool Walkable {
        get {return walkable;}
        set {
            walkable = value;
            //推送修改
            ModelResource.boardSubject.Notify(ModelModifyEvent.Cell, position); 
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
            ModelResource.boardSubject.Notify(ModelModifyEvent.Cell, position); 
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
        this.Effect = SpecialEffect.Portal;
    }

    /// <summary>
    ///   <para> 传送目的地 </para>
    /// </summary>
    public Vector2Int Target {
        get {return target;}
        set {
            target = value;
            //推送修改
            ModelResource.boardSubject.Notify(ModelModifyEvent.Cell, position); 
        }
    }
}