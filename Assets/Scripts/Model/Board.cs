using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 棋盘数据 </para>
/// </summary>
public class Board : MonoBehaviour {

    /// <summary>
    ///   <para> 存储坐标和格子 </para>
    /// </summary>
    private Dictionary<Vector2Int, Cell> map;

    // 数据边界
    private int borderUp;
    private int borderDown;
    private int borderLeft;
    private int borderRight;

    // 构造函数
    public Board() {
        map = new Dictionary<Vector2Int, Cell>();
        borderUp = borderRight = int.MinValue;
        borderDown = borderLeft = int.MaxValue;
    }

    /// <summary>
    ///   <para> 加载数据 </para>
    /// </summary>
    public void Load(SaveEntity saveEntity) {
        // 加载可走格子信息，默认无效果
        List<LandSaveEntity> mapEntity = saveEntity.map;
        foreach(LandSaveEntity cell in mapEntity) {
            Cell newCell = new Cell( new Vector2Int(cell.x, cell.y), true, SpecialEffect.None);
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
        PublicResource.boardSubject.Notify(ModelModifyEvent.Cell, pos);
    }

    /// <summary>
    ///   <para> 删除Cell </para>
    /// </summary>
    public void Remove(Vector2Int pos) {
        if(Contains(pos)) map.Remove(pos);

        // 推送修改
        PublicResource.boardSubject.Notify(ModelModifyEvent.Cell, pos);
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

    // 获取边界
    public int BorderUp { get{ return borderUp; } }
    public int BorderDown { get{ return borderDown; } }
    public int BorderLeft { get{ return borderLeft; } }
    public int BorderRight { get{ return borderRight; } }
}