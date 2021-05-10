using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 绘制器，接受用户输入，并将操作分发给绘制器 </para>
/// </summary>
public class PaintController : MonoBehaviour {

    // 当前绘制的类型
    private MapEditObject editObject;

    // 当前的绘制器
    private Paint painter;

    // 当前的块绘制器
    private BlockPaint blockPainter;

    /// <summary>
    ///   <para> 分析用户输入 </para>
    /// </summary>
    void Update() {
        // 避免与UI按键冲突
        if (CursorMonitor.CursorIsOverUI())
            return;

        // 获取鼠标所在点的点在tilemap上的坐标
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector2Int pos = GameResource.tilemapManager.WorldToCell(loc); //warning

        // 若左键是按住的状态，则通知BlockPainter（Portal除外）
        // BlockPainter自带去重，不需要额外筛选
        if(Input.GetMouseButton(0) && editObject != MapEditObject.Portal) {
            blockPainter.Preview(pos);
            return;
        }

        // 左键释放时，若有格子已经在这一笔中画出来了，则完成这一笔，并记录之
        if(Input.GetMouseButtonUp(0) && blockPainter.BlockCount() != 0) {
            MapEditResource.editCareTaker.Push(blockPainter.PaintBlock());
            return;
        }
    }

    /// <summary>
    ///   <para> 当前绘制的类型 </para>
    /// </summary>
    public MapEditObject EditObject {
        get { return editObject; }
        set {
            editObject = value;

            // 设定Painter
            Dictionary<MapEditObject, Paint> paintDic = new Dictionary<MapEditObject, Paint>() {
                {MapEditObject.Land, MapEditResource.landEditor},
                {MapEditObject.Token, MapEditResource.tokenEditor},
                {MapEditObject.Special, MapEditResource.specialEditor},
                {MapEditObject.Portal, MapEditResource.portalEditor}
            };
            painter = paintDic[editObject];

            // 设定blockPainter
            Dictionary<MapEditObject, BlockPaint> blockPaintDic = new Dictionary<MapEditObject, BlockPaint>() {
                {MapEditObject.Land, MapEditResource.landEditor},
                {MapEditObject.Token, MapEditResource.tokenEditor},
                {MapEditObject.Special, MapEditResource.specialEditor},
                {MapEditObject.Portal, null}
            };
            blockPainter = blockPaintDic[editObject];
        }
    }
}