using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 提供地图辅助功能，例如寻路 </para>
///   <para> 本类不直接修改数据！ </para>
/// </summary>
public class BoardAssistant : MonoBehaviour {

    /// <summary>
    ///   <para> 获取所有从pos出发前进step步可到达的格子 </para>
    /// </summary>
    public Dictionary<Vector2Int, List<Vector2Int>> GetRoute(Vector2Int pos, int step) {
        //若开始的格子是doubleStep，步数翻倍
        if(ModelResource.board.Get(pos).Effect == SpecialEffect.Double_Step) {
            step *= 2;
            Debug.Log("doubleStep: " + step);
        }

        //返回列表
        Dictionary<Vector2Int, List<Vector2Int>> ret = new Dictionary<Vector2Int, List<Vector2Int>>();

        //需要的信息：当前格子的坐标、上一步的坐标，已走步数
        Queue<(Vector2Int now, List<Vector2Int> pre, int step)> queue = new Queue<(Vector2Int now, List<Vector2Int> pre, int step)>();

        //获取本阵营棋子的位置，己方棋子不允许重叠
        List<Vector2Int> tokens = ModelResource.tokenSet.Query(GameResource.gameState.NowPlayer, PlayerID.None);

        //BFS
        queue.Enqueue( (pos, new List<Vector2Int>{pos}, 0) );
        while(queue.Count != 0) {
            var thisTuple = queue.Dequeue();

            //若step足够且不与其他棋子重叠，不进行操作，直接加入返回列表
            //若step足够但与其他棋子重叠，结果无效，与自己重叠（走回原点）可以
            if(thisTuple.step == step) {
                if(tokens.Contains(thisTuple.now) && !thisTuple.now.Equals(pos))
                    continue;
                thisTuple.pre.Add(thisTuple.now);
                ret[thisTuple.now] = thisTuple.pre;
                continue;
            }

            //得到所有合法的下一步
            //如果只有一个合法的下一步，说明now在端点上，不考虑它的上一步是否和下一步重合，直接入队
            List<Vector2Int> nextGrids = GetNeighbors(thisTuple.now);
            if(nextGrids.Count == 1) {
                thisTuple.pre.Add(thisTuple.now);
                queue.Enqueue( (nextGrids[0], thisTuple.pre, thisTuple.step+1) );
                continue;
            }

            //将所有不和上一步重合的下一步入队
            foreach(Vector2Int next in nextGrids) {
                if(next != thisTuple.pre.Last()) {
                    List<Vector2Int> newPre = new List<Vector2Int> (thisTuple.pre);
                    newPre.Add(thisTuple.now);
                    queue.Enqueue( (next, newPre, thisTuple.step+1) );
                }
            }
        }
        return ret;
    }

    /// <summary>
    ///   <para> 获取所有与该格子相邻的可走格子 </para>
    /// </summary>
    List<Vector2Int> GetNeighbors(Vector2Int pos) {
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
            Board board = ModelResource.board;
            if(board.Contains(pos+offset) && board.Get(pos+offset).Walkable) {
                ret.Add(pos+offset);
            }
        }

        return ret;
    }

}