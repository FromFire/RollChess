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

    // 当前的块绘制器
    private Paint painter;

    // 初始化
    void Start() {
        EditObject = MapEditObject.Land;
    }

    // 分析用户输入
    void Update() {
        // 避免与UI冲突（还没画的时候）
        // 如果任何时候都不能与UI冲突，那在UI上松键的时候会视作绘制还没有结束
        // 如果只在按下的一刻判断，则点按钮的第2帧就会开始绘制
        if (CursorMonitor.CursorIsOverUI() && painter.Count() == 0)
            return;

        // 获取鼠标所在点的点在tilemap上的坐标
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector2Int pos = ToolResource.tilemapManager.WorldToCell(loc); //warning

        // 若左键是按住的状态，则通知BlockPainter
        // BlockPainter自带去重，不需要额外筛选
        if(Input.GetMouseButton(0)) {
            painter.Preview(pos);
            return;
        }

        // 左键释放时，若有格子已经在这一笔中画出来了，则完成这一笔，并记录之
        if(Input.GetMouseButtonUp(0) && painter.Count() != 0) {
            EditMomento momento = painter.Paint();
            // 若momento是null，说明这一步无效，直接返回
            if(momento is null)
                return;
            // 记录这一笔
            MapEditResource.momentoController.Record(momento);
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
            // 设定Painter
            painter = MapEditResource.EditObjectToPaint[editObject];
        }
    }
}