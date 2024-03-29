using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 显示实体格子、特殊块、传送门箭头 </para>
///   <para> 使用时和TilemapManager挂在同一个Object下 </para>
/// </summary>
// todo：三个显示部分关系不大，考虑拆分
public class BoardDisplay : MonoBehaviour {
    // 显示地板
    [SerializeField] private TilemapManager tilemapManagerBoard;

    // 显示特殊格子贴图
    [SerializeField] private TilemapManager tilemapManagerSpecial;

    // 所有传送门箭头的父对象
    [SerializeField] private GameObject portalArrows;

    // 传送门的样式案例
    [SerializeField] private GameObject arrowSample;

    // 所有传送门对象，key是起始坐标
    private Dictionary<Vector2Int, GameObject> arrows = new Dictionary<Vector2Int, GameObject>();

    void Start() {
        Display();
        // 注册更新
        ModelResource.boardSubject.Attach(ModelModifyEvent.Cell, Draw);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display() {
        // 获取有效数据列表
        Board board = Board.Get();
        HashSet<Vector2Int> keyInfo = board.ToPositionSet();
        foreach (Vector2Int pos in keyInfo) {
            Draw(pos);
        }
    }

    /// <summary>
    ///   <para> 绘制此坐标 </para>
    /// </summary>
    public void Draw(Vector2Int position) {
        Board board = Board.Get();
        Cell cell = board.Get(position);

        // 若不可走且无特效，则抹去它
        if(!cell.Walkable && cell.Effect == SpecialEffect.None) {
            // 判定格子原Tile是因为防止抹去Unwalkable填充的海洋
            if(tilemapManagerBoard.GetTile(position) != TileType.Ocean)
                tilemapManagerBoard.RemoveTile(position);
            tilemapManagerSpecial.RemoveTile(position);
            // 抹去传送门
            if(arrows.ContainsKey(position)) {
                Destroy(arrows[position]);
                arrows.Remove(position);
            }
            return;
        }

        // 显示TilemapBoard层，地图格子
        tilemapManagerBoard.SetTile(position, TileType.Land);
            
        // 显示TilemapSpecial层，特殊格子效果
        SpecialEffect specialEffect = board.Get(position).Effect;
        if(specialEffect != SpecialEffect.None)
            tilemapManagerSpecial.SetTile(position, Transform.tileTypeOfSpecialEffect[specialEffect]);
        else
            tilemapManagerSpecial.RemoveTile(position);

        // 绘制传送门之前，无论什么情况，先将它擦除
        RemoveArrow(position);

        // 显示传送门之间的箭头
        if (board.Get(position).Effect == SpecialEffect.Portal) {
            Vector2Int from = new Vector2Int(position.x, position.y);
            Vector2Int to = board.Get(position).Target;

            // 若from=to，则此传送门无效
            if(from == to)
                return;
                
            // 由于箭头和Grid相对位置绑定，箭头起止点均为Grid的local坐标
            Vector3 from3 = tilemapManagerSpecial.CellToLocal((Vector3Int)from);
            Vector3 to3 = tilemapManagerSpecial.CellToLocal((Vector3Int)to);

            //绘制箭头
            GameObject obj = Object.Instantiate(arrowSample);
            obj.transform.parent = portalArrows.transform;
            LineRenderer line = obj.GetComponent<LineRenderer>();
            DrawCurve(from3, (from3+to3)/2 + 2*Vector3.up, to3, line);

            //维护arrows
            arrows.Add(position, obj);
        } 
    }

    // 画贝塞尔曲线
    void DrawCurve (Vector3 point1,Vector3 point2,Vector3 point3,LineRenderer MyL) {
        int vertexCount = 30;//采样点数量
        List<Vector3> pointList = new List<Vector3> ();

        for (float ratio = 0; ratio <= 1; ratio +=1.0f/ vertexCount)
        {
            Vector3 tangentLineVertex1 = Vector3.Lerp (point1, point2, ratio);
            Vector3 tangentLineVectex2 = Vector3.Lerp (point2, point3, ratio);
            Vector3 bezierPoint = Vector3.Lerp (tangentLineVertex1, tangentLineVectex2, ratio);
            pointList.Add (bezierPoint);
        }
        MyL.positionCount = pointList.Count;
        MyL.SetPositions (pointList.ToArray());
    } 
    
    // 擦除箭头
    void RemoveArrow (Vector2Int position) {
        if(!arrows.ContainsKey(position))
            return;
        // 擦除
        Destroy(arrows[position]);
        arrows.Remove(position);
    }
}