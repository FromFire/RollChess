using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 对棋盘数据（Board + Cell）的操作和处理 </para>
/// </summary>
public class BoardController: MonoBehaviour  {

    /// <summary>
    ///   <para> 新一回合开始时更新地图 </para>
    /// </summary>
    public void NewTurnUpdate() {
        //暂无，考虑未来加入的脉冲块
    }

    /// <summary>
    ///   <para> 移除一座危桥 </para>
    /// </summary>
    public void RemoveBrokenBridge(Vector2Int position) {
        Board board = Board.Get();
        board.Get(position).Effect = SpecialEffect.None;
        board.Get(position).Walkable = false;
    }
}