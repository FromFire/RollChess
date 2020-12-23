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




// 实现地图基本功能，主要是支持两个维度坐标可在正负两方向无限延伸
public class BaseBoard<T> where T:new(){

    /// <summary>
    ///   <para> 数据容器 </para>
    ///   <para> 第一维：容量=4。四象限分别存储，顺序是负负，负正，正负，正正，即二进制递增。
    ///          四象限分别存储，0存储且仅存储在正象限上。 </para>
    ///   <para> 第二维：容量=capacity。x轴。 </para>
    ///   <para> 第三维：容量=capacity。y轴。 </para>
    /// </summary>
    List<List<List<T>>> mapList;

    //默认每个象限20*20，全地图39*39
    const int DEFAULT_CAPACITY = 20;
    //实际容量
    int capacity;

    // 可能不为空的坐标集合（可能不为空：此坐标被设置过）
    HashSet<Vector2Int> validPositionList;

    //初始化，将四个象限初始化为四个默认大小的二维矩阵
    public BaseBoard() {
        //初始化数据容器
        mapList = new List<List<List<T>>> {new List<List<T>>(), new List<List<T>>(), new List<List<T>>(), new List<List<T>>()};

        //初始化容量
        capacity = DEFAULT_CAPACITY;

        //使用new List<T>会运行崩溃，因为List初始化不能指定容量
        for(int i=0; i<4; i++) {
            for(int j=0; j<DEFAULT_CAPACITY; j++){
                mapList[i].Add(new List<T>());
                for(int k=0;k<DEFAULT_CAPACITY;k++) {
                    mapList[i][j].Add(new T());
                }
            }
        }

        //初始化validPositionList
        validPositionList = new HashSet<Vector2Int>();
    }

    //根据x,y寻找所在地图下标，返回0-3
    int FindIndex(Vector2Int pos) {
        return 0 + (pos.x>=0? 2:0) + (pos.y>=0? 1:0);
    }

    //检查某下标是否合法
    public bool IsValid(Vector2Int pos) {
        int index = FindIndex(pos);
        if(pos.x >= capacity || pos.y >= capacity) {
            return false;
        }
        return true;
    }

    //获取T
    public T GetData(Vector2Int pos) {
        int index = FindIndex(pos);
        Debug.Assert(IsValid(pos));
        return mapList[index][System.Math.Abs(pos.x)][System.Math.Abs(pos.y)];
    }

    //设置T
    public void SetData(Vector2Int pos, T data) {
        // 若pos不在地图范围内，则扩充地图直到地图包含pos的位置
        while(!IsValid(pos)) {
            ExpandSpace();
        }
        // 设置信息
        int index = FindIndex(pos);
        mapList[index][System.Math.Abs(pos.x)][System.Math.Abs(pos.y)] = data;
        // 所有设置过的都视为关键信息
        validPositionList.Add(pos);
    }
    
    //删除T
    public void RemoveData(Vector2Int pos) {
        int index = FindIndex(pos);
        mapList[index][System.Math.Abs(pos.x)][System.Math.Abs(pos.y)] = new T();
        validPositionList.Remove(pos);
    }

    //获取所有可能非空的坐标
    public HashSet<Vector2Int> ToList() {
        return validPositionList;
    }

    //将地图长宽都*2，即面积*4
    void ExpandSpace() {
        int expandedCapacity = 2 * capacity;
        for(int map=0; map<4; map++) {
            // 扩充y，[a][a]扩充为[a][2a]
            for(int i=0; i<capacity; i++) {
                for(int j=capacity; j<expandedCapacity; j++) {
                    mapList[map][i].Add(new T());
                }
            }

            // 扩充x，[a][2a]扩充为[2a][2a]
            for(int i=capacity; i<expandedCapacity; i++) {
                mapList[map].Add(new List<T>());
                for(int j=0; j<expandedCapacity; j++) {
                    mapList[map][i].Add(new T());
                }
            }
        }
        capacity *= 2;
    }
}