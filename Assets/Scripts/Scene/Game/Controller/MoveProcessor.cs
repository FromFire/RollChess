using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 处理一次走子，包括其中特殊块等等 </para>
/// </summary>
public class MoveProcessor: MonoBehaviour  {

    /// <summary>
    ///   <para> 移动棋子 </para>
    /// </summary>
    public void Move(Vector2Int from, List<Vector2Int> route) {
        Board board = Board.Get();
        Vector2Int to = route.Last();
        Debug.Log("走子: ("+ from.x + "." + from.y + ") -> (" + to.x + "." + to.y + ") ");

        // 若目的点是传送门，将传送门的目的地加在route最后
        Cell toCell = board.Get(to);
        if(toCell.Effect == SpecialEffect.Portal)
            route.Add(toCell.Target);

        // 如果危桥是该棋子的终点，则该棋子直接去世
        bool isKilled = false;
        if(board.Get(route.Last()).Effect == SpecialEffect.Broken_Bridge) {
            isKilled = true;
            GameResource.tokenController.Kill(from);
        }

        // 销毁路上的危桥
        foreach(Vector2Int cell in route)
            if(board.Get(cell).Effect == SpecialEffect.Broken_Bridge)
                GameResource.boardController.RemoveBrokenBridge(cell);
        
        // 如果本棋子还活着，就移动到目的地
        if(!isKilled) {
            GameResource.tokenController.Move(from, route.Last());
        }
    }
}