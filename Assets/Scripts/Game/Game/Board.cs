using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Widget;

// 地图类
// 加载地图、管理地图状态
public class Board : MonoBehaviour{
    //棋盘的显示
    public BoardDisplay boardDisplay;

    //存储每一个格子
    BaseBoard<SingleGrid> map;

    // Start is called before the first frame update
    void Start() {

    }

    //初始化
    public void Init(List<SingleMapGridEntity> mapEntity, List<SingleSpecialEntity> specialEntity, List<SinglePortalEntity> portalEntity) {
        //导入地图信息
        map = new BaseBoard<SingleGrid>();
        foreach(SingleMapGridEntity grid in mapEntity) {
            map.Add(new Vector2Int(grid.x, grid.y), new SingleGrid(true));
        }

        //导入特殊格子信息
        foreach(SingleSpecialEntity special in specialEntity) {
            SingleGrid.Effect effect = SingleGrid.Effect.None;
            switch(special.effect) {
                case "doubleStep":
                    effect = SingleGrid.Effect.DoubleStep;
                    break;
                case "brokenBridge":
                    effect = SingleGrid.Effect.BrokenBridge;
                    break;
            }
            map.GetData(new Vector2Int(special.x, special.y)).SpecialEffect = effect;
        }

        //导入传送门信息
        foreach(SinglePortalEntity portal in portalEntity) {
            map.GetData(new Vector2Int(portal.fromX, portal.fromY)).PortalTarget = new Vector2Int(portal.toX, portal.toY);
        }
        
        boardDisplay.Display(map);
    }

    // Update is called once per frame
    void Update() {
        
    }

    // 返回该坐标的格子是否可通过
    public bool IsWalkable(Vector2Int pos) {
        return map.GetData(pos).walkable;
    }

    //获取所有与该格子相邻的可走格子
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
            if(IsWalkable(pos+offset)) {
                ret.Add(pos+offset);
            }
        }

        return ret;
    }

    // 获取所有从pos出发前进step步可到达的格子
    public List<(Vector2Int pos, List<Vector2Int> route)> GetReachableGrids(Vector2Int pos, int step) {
        //若开始的格子是doubleStep，步数翻倍
        if(GetEffect(pos) == SingleGrid.Effect.DoubleStep) {
            step *= 2;
            Debug.Log("doubleStep: " + step);
        }

        //返回列表
        List<(Vector2Int now, List<Vector2Int> pre)> ret = new List<(Vector2Int now, List<Vector2Int> pre)>();

        //需要的信息：当前格子的坐标、上一步的坐标，已走步数
        Queue<(Vector2Int now, List<Vector2Int> pre, int step)> queue = new Queue<(Vector2Int now, List<Vector2Int> pre, int step)>();

        //BFS
        queue.Enqueue( (pos, new List<Vector2Int>{pos}, 0) );
        while(queue.Count != 0) {
            var thisTuple = queue.Dequeue();

            //若step足够，不进行操作，直接加入返回列表
            if(thisTuple.step == step) {
                ret.Add( (thisTuple.now, thisTuple.pre) );
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

        //返回列表去重
        Debug.Log("能走到的格子数："+ret.Count);
        ret = ret.Distinct().ToList();

        return ret;
    }

    //查询某格的特殊格子效果
    public SingleGrid.Effect GetEffect(Vector2Int pos) {
        return map.GetData(pos).SpecialEffect;
    }

    //查询该格子的传送门目的地（仅当该格子是传送门时）
    public Vector2Int GetPortalTarget(Vector2Int pos) {
        return map.GetData(pos).PortalTarget;
    }

    //检测路上的危桥并移除危桥
    public void DetectBrokenBridge(List<Vector2Int> route) {
        foreach(Vector2Int grid in route) {
            if(GetEffect(grid) == SingleGrid.Effect.BrokenBridge) {
                map.GetData(grid).SpecialEffect = SingleGrid.Effect.None;
                map.GetData(grid).walkable = false;
                boardDisplay.RemoveGrid(grid);
            }
        }
    }

    //检测某坐标是否在地图合法范围内
    public bool IsInBoard(Vector2Int pos) {
        return map.Contains(pos);
    }
}


