using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
///   <para> 游戏流程的总控制 </para>
///   <para> 包括玩家操作的实施、玩家操作权控制、回合控制、游戏结束判断等 </para>
/// </summary>
public class GameController : MonoBehaviour {

    // 初始化全局
    void Start()
    {
        // 调试时无Entrance，则现场构造mapChooseState
        if(GameResource.mapChooseState is null) {
            GameResource.mapChooseState = MapChooseState.CreateSample();
        }

        // 初始化
        Init();

        // 初始化完成后，操作权交给玩家
        StartOperating();
    }

    /// <summary>
    ///   <para> 使用mapChooseState初始化，结束后销毁mapChooseState </para>
    /// </summary>
    void Init() {
        // 初始化GameState，其数据来自于MapChooseState
        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID))) {
            if(id != PlayerID.None)
                GameResource.gameState.SetPlayerForm(id, GameResource.mapChooseState.GetPlayerForm(id));
        }
        // nowPlayer是第一个不是Banned的玩家
        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID))) {
            // 排除PlayerID.None，排除PlayerForm.Banned
            if(id != PlayerID.None || GameResource.gameState.GetPlayerForm(id) != PlayerForm.Banned) {
                GameResource.gameState.NowPlayer = id;
                break;
            }
        }

        // todo: 初始化myID

        // 读取存档
        string filename = GameResource.mapChooseState.MapFileName;
        SaveEntity saveEntity = SaveResource.saveManager.LoadMap(filename);
        SaveResource.saveLoader.Load(saveEntity);

        // 删除Banned玩家的所有棋子
        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID))) {
            // 排除PlayerID.None，不是Banned则忽略
            if(id == PlayerID.None || GameResource.gameState.GetPlayerForm(id) != PlayerForm.Banned)
                continue;
            // 获取该玩家的所有棋子
            List<Vector2Int> tokens = ModelResource.tokenSet.Query(id, PlayerID.None);
            // 移除
            foreach(Vector2Int token in tokens)
                ModelResource.tokenSet.Remove(token);
        }

        // 销毁MapChooseState（若存在）
        if(GameResource.mapChooseState.gameObject)
            Destroy(GameResource.mapChooseState.gameObject);
        GameResource.mapChooseState = null;
    }

    /// <summary>
    ///   <para> 移动棋子，移动结束后本角色的回合也结束 </para>
    /// </summary>
    public void Move(Vector2Int from, List<Vector2Int> route) {
        // 修改状态为处理中，这个状态将会持续到下一回合某一玩家获得操控权
        StartProcessing();

        // 走子过程交给moveProcessor处理
        GameResource.moveProcessor.Move(from, route);

        // 更新Loser和Winner，Loser必须在前
        UpdateLoser();
        UpdateWinner();
        // 判断游戏是否结束
        if(GameResource.gameState.Winner != PlayerID.None)
            GameOver();

        // 回合结束，切换至下一位玩家
        NextPlayer();
    }
    
    /// <summary>
    ///   <para> 掷骰子，是RollButton的OnClick函数 </para>
    /// </summary>
    public void RollDice() {
        //生成随机数
        GameResource.gameState.RollResult = new System.Random().Next(6)+1;
        Debug.Log("roll点结果: " + GameResource.gameState.RollResult);
    }

    /// <summary>
    ///   <para> 查询是否有获胜的玩家，若没有，返回-1 </para>
    /// </summary>
    void UpdateWinner() {
        PlayerID winnerCandidate = PlayerID.None; //可能赢（棋子数）的玩家序号
        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID))) {
            // 跳过PlayerID.None
            if(id == PlayerID.None)
                continue;

            // 若只有一个玩家不属于loser，则该玩家获胜
            if(!GameResource.gameState.IsLoser(id)) {
                //若暂无胜利候选者，此玩家就是胜利候选者
                if(winnerCandidate == PlayerID.None)
                    winnerCandidate = id;
                //若已有胜利候选者，则无人获胜
                else return;
            }
        }
        // 更新Winner
        GameResource.gameState.Winner = winnerCandidate;
    }

    /// <summary>
    ///   <para> 更新Loser信息 </para>
    /// </summary>
    void UpdateLoser() {
        foreach (PlayerID id in Enum.GetValues(typeof(PlayerID))) {
            // 排除PlayerID.None，和已经是Loser的
            if(id == PlayerID.None || GameResource.gameState.IsLoser(id))
                continue;
                
            // 查询非Loser玩家控制的棋子数量
            List<Vector2Int> tokens = ModelResource.tokenSet.Query(id, PlayerID.None);

            // 若棋子数量为0就标记为Loser
            if(tokens is null || tokens.Count == 0)
                GameResource.gameState.AddLoser(id);
        }
    }

    /// <summary>
    ///   <para> 游戏结束 </para>
    /// </summary>
    void GameOver() {
        GameResource.gameState.Stage = GameStage.Game_Over;
    }

    /// <summary>
    ///   <para> 切换至下一位玩家 </para>
    /// </summary>
    void NextPlayer() {
        GameState gameState = GameResource.gameState;
        PlayerID nowPlayer = gameState.NowPlayer;
        // 轮换，直到该玩家不是Banned或Loser
        do {
            PlayerID nextPlayer = (PlayerID)( ((int)nowPlayer+1) % gameState.CharacterNumber() );
            // 若轮换到第一位玩家，代表新一回合开始
            if((int)nowPlayer > (int)nextPlayer)
                NextTurn();
            nowPlayer = nextPlayer;
        } while(gameState.GetPlayerForm(nowPlayer) == PlayerForm.Banned || gameState.IsLoser(nowPlayer) || nowPlayer == PlayerID.None);
        gameState.NowPlayer = nowPlayer;

        // 将rollResult还原为-1
        gameState.RollResult = -1;

        // 所有处理完成后，操作权归还玩家
        StartOperating();
    }

    /// <summary>
    ///   <para> 切换至下一回合 </para>
    /// </summary>
    void NextTurn() {
        // 更新gameState
        GameResource.gameState.Turn += 1;
        
        // 通知棋盘更新
        GameResource.boardController.NewTurnUpdate();
    }

    /// <summary>
    ///   <para> 切换至玩家可操作状态 </para>
    /// </summary>
    void StartOperating() {
        GameState gameState = GameResource.gameState;
        // 联机模式：轮到自己，或轮到其他玩家
        if(gameState.MyID != PlayerID.None)
            gameState.Stage = gameState.NowPlayer == gameState.MyID ?
                GameStage.Self_Operating : GameStage.Other_Player_Operating;
        // 本地模式：轮到玩家（自己），或轮到AI
        else
            gameState.Stage = gameState.GetPlayerForm(gameState.NowPlayer) == PlayerForm.Player ? 
                GameStage.Self_Operating : GameStage.Other_Player_Operating;
    }

    /// <summary>
    ///   <para> 切换至系统处理状态 </para>
    /// </summary>
    void StartProcessing() {
        GameState gameState = GameResource.gameState;
        // 联机模式：自己处理中，或其他玩家处理中
        if(gameState.MyID != PlayerID.None)
            gameState.Stage = gameState.NowPlayer == gameState.MyID ?
                GameStage.Self_Operation_Processing : GameStage.Other_Player_Processing;
        // 本地模式：玩家（自己）处理中，或AI处理中
        else
            gameState.Stage = gameState.GetPlayerForm(gameState.NowPlayer) == PlayerForm.Player ? 
                GameStage.Self_Operation_Processing : GameStage.Other_Player_Processing;
    }
}