using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 存储单个地图格子情况
public class SingleGrid {
    //棋子是否可通过，默认不可通过
    public bool walkable;

    //表示特殊格子的效果
    public enum Effect {
        None, //无效果
        DoubleStep, //倍速：从此格开始时，移动距离加倍
        BrokenBridge, //危桥：只能通过一次，之后就会损坏
        Portal //传送门：传送到另一个格子
    }
    //此格子的特殊效果
    Effect effect;

    //仅effect=portal时有效，传送的目的地
    Vector2Int portalTarget;
    
    // 格子特殊效果的读写函数
    public Effect SpecialEffect {
        set { effect = value; }
        get { return effect; }
    }

    // 传送门读写函数
    public Vector2Int PortalTarget {
        set {
            effect = Effect.Portal;
            portalTarget = value;
        }
        get { return portalTarget; }
    }

    //构造函数
    public SingleGrid(bool walkable) {
        this.walkable = walkable;
        effect = Effect.None;
    }

    //默认构造函数，用于BaseBoard初始化，否则运行会崩溃
    public SingleGrid() {
        this.walkable = false;
        effect = Effect.None;
    }
}




// 实现地图中数据存储的功能
public class BaseBoard<T> where T:new(){

    // 数据容器
    Dictionary<Vector2Int, T> positionOfGrid;

    //边界
    int borderUp = int.MinValue;
    int borderDown = int.MaxValue;
    int borderLeft = int.MaxValue;
    int borderRight = int.MinValue;

    //初始化，将四个象限初始化为四个默认大小的二维矩阵
    public BaseBoard() {
        positionOfGrid = new Dictionary<Vector2Int, T>();
    }

    //检查某下标是否有定义
    public bool Contains(Vector2Int pos) {
        return positionOfGrid.ContainsKey(pos);
    }

    //获取T
    public T GetData(Vector2Int pos) {
        return Contains(pos) ? positionOfGrid[pos] : new T();
    }

    //设置T
    public void Add(Vector2Int pos, T data) {
        positionOfGrid.Add(pos, data);
        UpdateBorder(pos);
    }
    
    //删除T
    public void RemoveData(Vector2Int pos) {
        if(Contains(pos)) positionOfGrid.Remove(pos);
    }

    //返回所有有定义的数据列表
    public HashSet<Vector2Int> ToPositionsSet() {
        HashSet<Vector2Int> ret = new HashSet<Vector2Int>();
        foreach(KeyValuePair<Vector2Int, T> kvp in positionOfGrid) {
            ret.Add(kvp.Key);
        }
        return ret;
    }

    // 更新地图边界
    void UpdateBorder(Vector2Int pos) {
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