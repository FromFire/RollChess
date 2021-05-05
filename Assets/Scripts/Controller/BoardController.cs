using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 对棋盘数据（Board + Cell）的操作和处理 </para>
/// </summary>
public class BoardController: MonoBehaviour  {
    void Start() {
        // 注册响应更新
        PublicResource.boardSubject.Attach(ModelModifyEvent.Cell, RemoveBrokenBridge);
        PublicResource.gameStateSubject.Attach(ModelModifyEvent.Turn, NewTurnUpdate);
    }

    /// <summary>
    ///   <para> 处理经过路径时地图的变化 </para>
    ///   <para> 例如：危桥 </para>
    /// </summary>
    public void PassRoute(List<Vector2Int> route) {
        Board board = PublicResource.board;

        // 检测路上所有危桥，并移除
        foreach(Vector2Int cell in route)
            if(board.Get(cell).Effect == SpecialEffect.Broken_Bridge)
                RemoveBrokenBridge(cell);
    }

    /// <summary>
    ///   <para> 新一回合开始时更新地图 </para>
    /// </summary>
    public void NewTurnUpdate() {
        //暂无，考虑未来加入的脉冲块
    }

    /// <summary>
    ///   <para> 移除一座危桥 </para>
    ///   <para> 是Board更新时的响应函数 </para>
    /// </summary>
    public void RemoveBrokenBridge(Vector2Int position) {
        Board board = PublicResource.board;
        board.Get(position).Effect = SpecialEffect.None;
        board.Get(position).Walkable = false;
        // todo:考虑分2步推送是否可能导致bug
    }
}