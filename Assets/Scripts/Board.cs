﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地图类
// 加载地图、管理地图状态
public class Board : MonoBehaviour{

    //存储每一个格子
    BaseBoard<SingleGrid> map;

    //由于地图坐标正负都
    int offsetX;
    int offsetY;

    // Start is called before the first frame update
    void Start() {

    }

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
}

// 存储单个格子地图
public class SingleGrid {
    public bool walkable;

    public SingleGrid(bool walkable) {
        this.walkable = walkable;
    }

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

    //默认每个象限20*20，全地图40*40
    const int DEFAULT_CAPACITY = 20;

    //初始化
    public BaseBoard() {
        map_pp=new List<List<T>>();
        map_np=new List<List<T>>();
        map_nn=new List<List<T>>();
        map_pn=new List<List<T>>();
        mapList = new List<List<List<T>>> {map_nn, map_np, map_pn, map_pp};
        for(int i=0; i<4; i++) {
            // mapList[i] = new List<List<T>>(DEFAULT_CAPACITY);
            for(int j=0; j<DEFAULT_CAPACITY; j++){
                mapList[i].Add(new List<T>());
                for(int k=0;k<DEFAULT_CAPACITY;k++)
                    mapList[i][j].Add(new T());
                // mapList[i][j] = new List<T>(DEFAULT_CAPACITY);
            }
        }
    }

    //根据x,y寻找所在地图下标，返回0-3
    int findIndex(int x, int y) {
        int index = x>=0? 2:0;
        index += (y>=0? 1:0);
        return index;
    }

    public T getData(int x, int y) {
        int index = findIndex(x,y);
        return mapList[index][System.Math.Abs(x)][System.Math.Abs(y)];
    }

    public void setData(int x, int y, T data) {
        int index = findIndex(x,y);
        Debug.Log("["+mapList.ToArray().Length+"]");
        Debug.Log("["+mapList[0].ToArray().Length+"]");
        Debug.Log("["+mapList[0][0].ToArray().Length+"]");
        Debug.Log("index"+index+"x"+x+"y"+y);
        mapList[index][System.Math.Abs(x)][System.Math.Abs(y)] = data;
    }
}