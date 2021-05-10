using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 陆地绘制 </para>
/// </summary>
public class LandEditor : MonoBehaviour, Paint, BlockPaint {

    // 是否正在绘制（false表示正在擦除）
    private bool isPainting;

    // 块绘制的Momento
    EditMomento blockMomento = new EditMomento();

    /// <summary>
    ///   <para> 绘制块 </para>
    /// </summary>
    public EditMomento Paint(Vector2Int position) {
        // 无需调用Display，因为修改Model后显示会自动更新
        Board board = ModelResource.board;

        // 记录修改前后的状态
        Cell pre, after;
        // 若已有记录，只需把walkable改为true
        if(board.Contains(position)) {
            pre = new Cell(board.Get(position));
            after = new Cell(board.Get(position));
            after.Walkable = isPainting;
        }
        // 若无记录，则修改后是walkable=true的默认Cell
        else {
            pre = new Cell(position, false, SpecialEffect.None);
            after = new Cell(position, isPainting, SpecialEffect.None);
        }
        
        // 生成Momento
        EditMomento momento = new EditMomento();
        momento.editObject = MapEditObject.Land;
        momento.pre.Add(pre);
        momento.after.Add(after);
        momento.position.Add(position);

        // 执行Momento
        Execute(momento);
        return momento;
    }

    /// <summary>
    ///   <para> 撤销操作 </para>
    /// </summary>
    public void Undo(EditMomento momento) {
        // 交换pre和after
        EditMomento revertMoment = new EditMomento(momento);
        revertMoment.pre = new List<Object>(momento.after.ToArray());
        revertMoment.after = new List<Object>(momento.pre.ToArray());
        // 执行新momento
        Execute(revertMoment);
    }

    /// <summary>
    ///   <para> 重做操作 </para>
    /// </summary>
    public void Redo(EditMomento momento) {
        Execute(momento);
    }

    /// <summary>
    ///   <para> 执行momento上的操作 </para>
    /// </summary>
    void Execute(EditMomento momento) {
        // 把对应坐标上的都改成after
        Board board = ModelResource.board;
        for(int i=0; i<momento.position.Count; i++) {
            board.Add(momento.position[i], (Cell)momento.after[i]);
        }
    }

    /// <summary>
    ///   <para> 向正在画的一笔中加入新格子，然后预览已经绘制的部分 </para>
    ///   <para> 注意：Model会被修改！ </para>
    /// </summary>
    public void Preview(Vector2Int position) {
        // 调用Paint进行绘制
        EditMomento momento = Paint(position);

        // 维护blockMomento
        blockMomento.position.Add(position);
        blockMomento.pre.Add(momento.pre[0]);
        blockMomento.after.Add(momento.after[0]);
    }

    /// <summary>
    ///   <para> 完成这一笔 </para>
    /// </summary>
    public EditMomento PaintBlock() {
        EditMomento ret = new EditMomento(blockMomento);
        
        // 维护blockMomento
        blockMomento = new EditMomento();
        blockMomento.editObject = MapEditObject.Land;

        return ret;
    }
}