using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
///   <para> 传送门绘制 </para>
///   <para> 绘制方法：鼠标按下的格子是source，松开的格子是target </para>
/// </summary>
public class PortalEditor : MonoBehaviour, Paint {

    // 块绘制的Momento
    EditMomento blockMomento = new EditMomento();

    // 初始化
    void Start() {
        blockMomento.editObject = MapEditObject.Portal;
    }

    // 执行momento上的操作
    void Execute(EditMomento momento) {
        // 只需改source，momento里只会有一个position
        ModelResource.board.Add(momento.position[0], (Cell)momento.after[0]);
    }

    /// <summary>
    ///   <para> 把传送门的target设置成position，并预览 </para>
    ///   <para> 注意：Model会被修改！ </para>
    /// </summary>
    public void Preview(Vector2Int position) {
        // 若此格子没有陆地，则无视之
        Board board = ModelResource.board;
        if(!board.Contains(position) || board.Get(position).Walkable == false)
            return;

        // 如果momento一条记录都无，说明这是第一次点击，position就是source
        if(blockMomento.position.Count == 0) {
            blockMomento.position.Add(position);
            blockMomento.pre.Add(new Cell(board.Get(position)));
            blockMomento.after.Add(new PortalCell((Cell)blockMomento.pre[0], position));
            Execute(blockMomento);
            return;
        }
        
        // 不是同一格的话，修改source.target（和原本不同才改）
        Vector2Int targetNow = ((PortalCell)board.Get(blockMomento.position[0])).Target;
        if(position != targetNow) {
            // 修改board和after的target
            ((PortalCell)board.Get(blockMomento.position[0])).Target = position;
            Execute(blockMomento);
        }
    }

    /// <summary>
    ///   <para> 完成这一笔 </para>
    /// </summary>
    public EditMomento Paint() {
        // 如果source和target在同一格，则恢复board，返回null
        if( ((PortalCell)blockMomento.after[0]).Target == blockMomento.position[0] ) {
            Undo(blockMomento);
            Debug.Log("null");
            return null;
        }

        EditMomento ret = new EditMomento(blockMomento);
        // 维护blockMomento
        blockMomento = new EditMomento();
        blockMomento.editObject = MapEditObject.Portal;

        return ret;
    }

    /// <summary>
    ///   <para> 撤销操作 </para>
    /// </summary>
    public void Undo(EditMomento momento) {
        // 交换pre和after
        EditMomento revertMomento = new EditMomento(momento);
        revertMomento.pre = new List<Object>(momento.after.ToArray());
        revertMomento.after = new List<Object>(momento.pre.ToArray());

        // 执行新momento
        Execute(revertMomento);
    }

    /// <summary>
    ///   <para> 重做操作 </para>
    /// </summary>
    public void Redo(EditMomento momento) {
        Execute(momento);
    }

    /// <summary>
    ///   <para> 获取正在画的一笔中有多少格 </para>
    /// </summary>
    public int Count() {
        return blockMomento.position.Count;
    }

    /// <summary>
    ///   <para> 获取上一格的坐标 </para>
    ///   <para> 若无上一格，返回xy均为int.MaxValue </para>
    /// </summary>
    public Vector2Int LastPosition() {
        return blockMomento.position.Count == 0 ? new Vector2Int(int.MaxValue, int.MaxValue) : blockMomento.position.Last();
    }
}