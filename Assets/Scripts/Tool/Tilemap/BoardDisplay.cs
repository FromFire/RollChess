using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 显示实体格子、特殊块、传送门箭头、特殊格子浮窗介绍 </para>
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

    // 弹窗
    [SerializeField] private Popup popup;

    void Start() {
        Display();
        // 注册更新
        ModelResource.boardSubject.Attach(ModelModifyEvent.Cell, UpdateSelf);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display() {
        // 获取有效数据列表
        Board board = ModelResource.board;
        HashSet<Vector2Int> keyInfo = board.ToPositionSet();
        HashSet<Vector2Int> walkablePos = new HashSet<Vector2Int>();
        foreach (Vector2Int pos in keyInfo) {
            //去除不可走的坐标
            if (board.Get(pos).Walkable)
                walkablePos.Add(pos);
        }

        // 显示TilemapBoard层，地图格子
        foreach (Vector2Int pos in walkablePos)
            tilemapManagerBoard.SetTile(pos, TileType.Land);

        // 显示TilemapSpecial层，特殊格子效果
        foreach (Vector2Int pos in walkablePos) {
            SpecialEffect specialEffect = board.Get(pos).Effect;
            if(specialEffect != SpecialEffect.None)
                tilemapManagerSpecial.SetTile(pos, Transform.tileTypeOfSpecialEffect[specialEffect]);
        }

        // 显示传送门之间的箭头
        foreach (Vector2Int pos in walkablePos) {
            if (board.Get(pos).Effect == SpecialEffect.Portal) {
                // 由于箭头和Grid相对位置绑定，箭头起止点均为Grid的local坐标
                Vector2Int from = new Vector2Int(pos.x, pos.y);
                Vector2Int to = ((PortalCell)board.Get(pos)).Target;
                Vector3 from3 = tilemapManagerSpecial.CellToLocal((Vector3Int)from);
                Vector3 to3 = tilemapManagerSpecial.CellToLocal((Vector3Int)to);

                //绘制箭头
                GameObject obj = Object.Instantiate(arrowSample);
                obj.transform.parent = portalArrows.transform;
                LineRenderer line = obj.GetComponent<LineRenderer>();
                DrawCurve(from3, (from3+to3)/2 + 2*Vector3.up, to3, line);
            }
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

    void Update() {
        // 当鼠标所在格子是特殊格子时，在画面左上角显示该格子的介绍
        Board board = ModelResource.board;
        Vector2Int pointedCell = tilemapManagerSpecial.CursorPointingCell();
        // 避免空格子
        if(!board.Contains(pointedCell))
            return;
        // 获取内容，弹出Popup
        SpecialEffect pointedEffect = board.Get(pointedCell).Effect;
        if(pointedEffect != SpecialEffect.None) {
            // 设置SpecialPopupContent的内容
            SpecialIntroductionItem item = GameResource.specialIntroduction.introduction[pointedEffect];
            SpecialPopupContent content = popup.Content.GetComponent<SpecialPopupContent>();
            content.Title = item.introTitle;
            content.Describe = item.introText;
            content.EffectIntro = item.effectText;

            //设置PopUp显示
            popup.Show();
        } else {
            popup.Hide();
        }
    }

    /// <summary>
    ///   <para> Board更新时调用 </para>
    /// </summary>
    public void UpdateSelf(Vector2Int position) {
        Cell cell = ModelResource.board.Get(position);

        // 不可走 + 无特效：代表该格子已被移除，
        // 由UnwalkableDisplay覆盖该格为海洋，所以不需要Board层处理，清除特殊层即可
        // 例如：经过危桥后
        if(!cell.Walkable && cell.Effect == SpecialEffect.None) {
            tilemapManagerSpecial.RemoveTile(position);
        }
    }
}