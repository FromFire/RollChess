using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 角色操控方式 </para>
/// </summary>
public enum PlayerForm {
    Player,     //玩家操控
    Computer,   //AI操控
    Banned      //禁用角色
}

/// <summary>
///   <para> 格子的特殊效果 </para>
/// </summary>
public enum SpecialEffect {
    None,           //无效果
    Double_Step,    //倍速
    Broken_Bridge,  //危桥
    Portal,         //传送门
    Pulse,          //脉冲
    Stop,           //到此止步
    Magical_Circle, //魔法阵
    Roll_Again      //再掷一次
}

/// <summary>
///   <para> 玩家，用棋子颜色唯一标识 </para>
/// </summary>
public enum PlayerID {
    Red,
    Blue,
    Yellow,
    Green
}

/// <summary>
///   <para> Model修改事件 </para>
/// </summary>
public enum ModelModifyEvent {
    // 地图格子被修改
    Cell,           //地图格子被修改

    // 该格子上的棋子被修改
    Token,          //该格子上的棋子被修改
    
    // 游戏状态被修改 GameState
    Turn,           //回合数
    Roll_Result,    //掷骰子结果
    Player_Form,    //玩家操作形式
    Now_Player,     //当前操作的玩家
    Operation_State,//当前操作状态
    Is_Game_Over,   //游戏是否结束
    Winner          //赢家
}


/// <summary>
///   <para> Tile绘制类型 </para>
///   <para> 用来描述Tile的类型，只面向底层绘图，不描述功能，仅在View内部使用 </para>
/// </summary>
public enum TileType {
    // 空
    None,                   //空

    // 实体块
    Land,                   //陆地
    Ocean,                  //海洋

    // 特殊格子
    Special_Double_Step,    //倍速
    Special_Broken_Bridge,  //危桥
    Special_Portal,         //传送门
    Special_Pulse_On,       //脉冲块开启
    Special_Pulse_Off,      //脉冲块闭合
    Special_Stop,           //到此止步
    Special_Magical_Circle, //魔法阵
    Special_Roll_Again,     //再掷一次

    // 玩家
    Player_Red,
    Player_Blue,
    Player_Yellow,
    Player_Green,
    Player_Neutral,

    // 高亮
    Highlight_Blue,
    Highlight_Yellow,

    // 六边形网格
    HexGrid
}