﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// 地图类
// 加载地图、管理地图状态
public class Board : MonoBehaviour{

    //存储每一个格子
    BaseBoard<SingleGrid> map;

    // Start is called before the first frame update
    void Start() {

    }

    //初始化
    public void init(List<SingleMapGridEntity> entity) {
        //导入地图信息
        map = new BaseBoard<SingleGrid>();
        foreach(SingleMapGridEntity grid in entity) {
            map.setData(grid.x, grid.y, new SingleGrid(true));
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    // 返回该坐标的格子是否可通过
    public bool isWalkable(Vector2Int pos) {
        return map.getData(pos.x, pos.y).walkable;
    }

    //获取所有与该格子相邻的可走格子
    public List<Vector2Int> getNeighbors(Vector2Int pos) {
        List<Vector2Int> ret = new List<Vector2Int>();

        //无论奇偶，上下左右都可达
        List<Vector2Int> offsets = new List<Vector2Int> {
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(0,1),
            new Vector2Int(0,-1)
        };
        //偶数行：斜向左可达(-1,+1)和(-1,-1)
        if(pos.y % 2 == 0) {
            offsets.Add(new Vector2Int(-1,1));
            offsets.Add(new Vector2Int(-1,-1));
        }
        //奇数行：斜向右可达(+1,+1)和(+1,-1)
        else {
            offsets.Add(new Vector2Int(1,1));
            offsets.Add(new Vector2Int(1,-1));
        }

        //筛选出可到达的格子
        foreach(Vector2Int offset in offsets) {
            if(isWalkable(pos+offset)) {
                ret.Add(pos+offset);
            }
        }

        return ret;
    }

    // 显示所有从pos出发前进step步可到达的格子
    public List<Vector2Int> getReachableGrids(Vector2Int pos, int step) {
        List<Vector2Int> ret = new List<Vector2Int>();

        //需要的信息：当前格子的坐标、上一步的坐标，已走步数
        Queue<(Vector2Int now, Vector2Int pre, int step)> queue = new Queue<(Vector2Int now, Vector2Int pre, int step)>();

        //BFS
        queue.Enqueue( (pos, pos, 0) );
        while(queue.Count != 0) {
            var thisTuple = queue.Dequeue();

            //若step足够，不进行操作，直接加入返回列表
            if(thisTuple.step == step) {
                ret.Add(thisTuple.now);
                continue;
            }

            //得到所有合法的下一步
            //如果只有一个合法的下一步，说明now在端点上，不考虑它的上一步是否和下一步重合，直接入队
            List<Vector2Int> nextGrids = getNeighbors(thisTuple.now);
            if(nextGrids.Count == 1) {
                queue.Enqueue( (nextGrids[0], thisTuple.now, thisTuple.step+1) );
                continue;
            }

            //将所有不和上一步重合的下一步入队
            foreach(Vector2Int next in nextGrids) {
                if(next != thisTuple.pre) {
                    queue.Enqueue( (next, thisTuple.now, thisTuple.step+1) );
                }
            }

        }

        //返回列表去重
        ret = ret.Distinct().ToList();

        return ret;
    }
}






// 存储单个地图格子情况
public class SingleGrid {
    //棋子是否可通过，默认不可通过
    public bool walkable;

    //构造函数
    public SingleGrid(bool walkable) {
        this.walkable = walkable;
    }

    //默认构造函数，用于BaseBoard初始化，否则运行会崩溃
    public SingleGrid() {
        this.walkable = false;
    }
}






// 实现地图基本功能，主要是支持两个维度坐标可在正负两方向无限延伸
public class BaseBoard<T> where T:new(){
    //数据容器，四象限分别存储，0存储且仅存储在正象限上
    //P: positive正, N: negative负, 前x轴后y轴
    List<List<T>> map_pp;
    List<List<T>> map_pn;
    List<List<T>> map_np;
    List<List<T>> map_nn;

    //存储四象限，用于批量操作
    //顺序是nn,np,pn,pp，即二进制递增
    List<List<List<T>>> mapList;

    //默认每个象限20*20，全地图39*39
    const int DEFAULT_CAPACITY = 20;

    //初始化，将四个象限初始化为四个默认大小的二维矩阵
    public BaseBoard() {
        map_pp=new List<List<T>>();
        map_np=new List<List<T>>();
        map_nn=new List<List<T>>();
        map_pn=new List<List<T>>();
        mapList = new List<List<List<T>>> {map_nn, map_np, map_pn, map_pp};

        //使用new List<T>会运行崩溃，因为List初始化不能指定容量
        for(int i=0; i<4; i++) {
            for(int j=0; j<DEFAULT_CAPACITY; j++){
                mapList[i].Add(new List<T>());
                for(int k=0;k<DEFAULT_CAPACITY;k++) {
                    mapList[i][j].Add(new T());
                }
            }
        }
    }

    //根据x,y寻找所在地图下标，返回0-3
    int findIndex(int x, int y) {
        return 0 + (x>=0? 2:0) + (y>=0? 1:0);
    }

    //获取T
    public T getData(int x, int y) {
        int index = findIndex(x,y);
        return mapList[index][System.Math.Abs(x)][System.Math.Abs(y)];
    }

    //设置T
    public void setData(int x, int y, T data) {
        int index = findIndex(x,y);
        mapList[index][System.Math.Abs(x)][System.Math.Abs(y)] = data;
    }
}