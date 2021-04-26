using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 棋盘数据 </para>
/// </summary>
public class Board {

    /// <summary>
    ///   <para> 存储坐标和格子 </para>
    /// </summary>
    private Dictionary<Vector2Int, Cell> map;

    // 数据边界
    private int borderUp;
    private int borderDown;
    private int borderLeft;
    private int borderRight;

    /// <summary>
    ///   <para> 更新推送 </para>
    /// </summary>
    public PositionSubject subject;

    // 构造函数
    public Board() {
        map = new Dictionary<Vector2Int, Cell>();
        borderUp = borderRight = int.MinValue;
        borderDown = borderLeft = int.MaxValue;
        subject = new PositionSubject();
    }

    /// <summary>
    ///   <para> 加载数据 </para>
    /// </summary>
    public void Load(SaveEntity saveEntity) {
        // 加载可走格子信息，默认无效果
        List<LandSaveEntity> mapEntity = saveEntity.map;
        foreach(LandSaveEntity cell in mapEntity) {
            Cell newCell = new Cell( new Vector2Int(cell.x, cell.y), true, SpecialEffect.None);
            newCell.subject = this.subject;
            Add(new Vector2Int(cell.x, cell.y), newCell);
        }

        // 加载特殊格子信息
        List<SpecialSaveEntity> specialEntity = saveEntity.special;
        foreach(SpecialSaveEntity cell in specialEntity) {
            Get(new Vector2Int(cell.x, cell.y)).Effect = Transform.specialEffectOfName[cell.effect];
        }

        // 加载传送门信息
        List<PortalSaveEntity> portalEntity = saveEntity.portal;
        foreach(PortalSaveEntity cell in portalEntity) {
            Vector2Int pos = new Vector2Int(cell.fromX, cell.fromY);
            Add(pos, new PortalCell( Get(pos), new Vector2Int(cell.toX, cell.toY) ));
        }

        // 初始化时在Add()中推送修改，但此时Subject中无observer，所以推送无效
        // View将统一在初始化时读取和显示数据
    }

    // 获取所有从pos出发前进step步可到达的格子
    public List<(Vector2Int pos, List<Vector2Int> route)> GetReachableGrids(Vector2Int pos, int step) {
        //若开始的格子是doubleStep，步数翻倍
        if(Get(pos).Effect == SpecialEffect.Double_Step) {
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

    

    /// <summary>
    ///   <para> 包含性检查 </para>
    /// </summary>
    public bool Contains(Vector2Int pos) {
        return map.ContainsKey(pos);
    }

    /// <summary>
    ///   <para> 设置和增加Cell </para>
    /// </summary>
    public void Add(Vector2Int pos, Cell cell) {
        map.Add(pos, cell);
        UpdateBorder(pos);

        // 推送修改
        subject.Notify(ModelModifyEvent.Cell, pos);
    }

    /// <summary>
    ///   <para> 删除Cell </para>
    /// </summary>
    public void Remove(Vector2Int pos) {
        if(Contains(pos)) map.Remove(pos);

        // 推送修改
        subject.Notify(ModelModifyEvent.Cell, pos);
    }

    /// <summary>
    ///   <para> 获取数据 </para>
    /// </summary>
    public Cell Get(Vector2Int pos) {
        return null;
    }

    /// <summary>
    ///   <para> 导出为Set格式 </para>
    /// </summary>
    public HashSet<Vector2Int> ToPositionSet() {
        HashSet<Vector2Int> ret = new HashSet<Vector2Int>();
        foreach(KeyValuePair<Vector2Int, Cell> kvp in map) {
            ret.Add(kvp.Key);
        }
        return ret;
    }

    /// <summary>
    ///   <para> 更新边界 </para>
    /// </summary>
    private void UpdateBorder(Vector2Int pos) {
        borderLeft = System.Math.Min(borderLeft, pos.x);
        borderRight = System.Math.Max(borderRight, pos.x);
        borderUp = System.Math.Max(borderUp, pos.y);
        borderDown = System.Math.Min(borderDown, pos.y);
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
            if(Get(pos+offset).Walkable) {
                ret.Add(pos+offset);
            }
        }

        return ret;
    }

    // 获取边界
    public int BorderUp { get{ return borderUp; } }
    public int BorderDown { get{ return borderDown; } }
    public int BorderLeft { get{ return borderLeft; } }
    public int BorderRight { get{ return borderRight; } }
}