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
    Roll_Again,     //再掷一次
}

/// <summary>
///   <para> 玩家，用棋子颜色唯一标识 </para>
/// </summary>
public enum PlayerID {
    Red,
    Blue,
    Yellow,
    Green,
    None,   //无玩家或不合法
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
    Now_Player,     //当前操作的玩家
    Stage,          //当前操作状态
    Is_Game_Over,   //游戏是否结束
    Winner,         //赢家
    Loser,          //输家

    // 地图选择被修改 MapChooseState
    Map_File_Name,  //地图文件名称
    Player_Limit,   //玩家人数限制
    Map_Name,       //地图名称
    Player_Form,    //玩家操作形式
    
    // 网络状态
    Client_Success, //客户端连接成功
    Server_Success, //服务器创建成功
    Player_Change,  //Player修改
    Disconnect,     //断联
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
    Special_Neutral_Player, //中立玩家

    // 玩家
    Player_Red,
    Player_Blue,
    Player_Yellow,
    Player_Green,

    // 高亮
    Highlight_Blue,
    Highlight_Yellow,

    // 六边形网格
    HexGrid
}

/// <summary>
///   <para> 玩家操作状态，用处是限制玩家操作 </para>
/// </summary>
public enum GameStage {
    Self_Operating,             // 自己（受本机控制，可以是多个角色）的行动回合，可以操作，可以查看信息
    Self_Operation_Processing,  // 系统正在处理自己的操作，不可以操作，不可以查看消息
    Other_Player_Operating,     // 其他玩家的操作回合，不可以操作，可以查看消息
    Other_Player_Processing,    // 系统正在处理其他玩家的操作，不可以操作，可以查看消息
    Game_Over,                  // 游戏结束，所有玩家均不能操作
}

/// <summary>
///   <para> 玩家操作状态，用处是限制玩家操作 </para>
/// </summary>
public enum MapEditObject {
    Land,   // 陆地
    Token,  // 棋子
    Special,// 特殊块
    Portal, // 传送门
}