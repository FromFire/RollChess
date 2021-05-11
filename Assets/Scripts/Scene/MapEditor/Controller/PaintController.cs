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

    // 初始化
    void Start() {
        EditObject = MapEditObject.Land;
    }

    // 分析用户输入
    void Update() {
        // 避免与UI按键冲突
        if (CursorMonitor.CursorIsOverUI())
            return;

        // 获取鼠标所在点的点在tilemap上的坐标
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector2Int pos = ToolResource.tilemapManager.WorldToCell(loc); //warning

        // 若左键是按住的状态，则通知BlockPainter（Portal除外）
        // BlockPainter自带去重，不需要额外筛选
        if(Input.GetMouseButton(0) && editObject != MapEditObject.Portal) {
            blockPainter.Preview(pos);
            return;
        }

        // 左键释放时，若有格子已经在这一笔中画出来了，则完成这一笔，并记录之
        if(Input.GetMouseButtonUp(0) && blockPainter.BlockCount() != 0) {
            MapEditResource.momentoController.Record(blockPainter.PaintBlock());
            return;
        }
        //todo：判断无效绘制
    }

    /// <summary>
    ///   <para> 当前绘制的类型 </para>
    /// </summary>
    public MapEditObject EditObject {
        get { return editObject; }
        set {
            editObject = value;
            // 设定Painter和blockPainter
            painter = MapEditResource.EditObjectToPaint[editObject];
            blockPainter = MapEditResource.EditObjectToBlockPaint[editObject];
        }
    }
}