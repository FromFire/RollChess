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
