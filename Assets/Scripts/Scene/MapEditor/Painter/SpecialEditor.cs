using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
///   <para> 特殊块绘制 </para>
/// </summary>
public class SpecialEditor : MonoBehaviour, Paint {

    // 正在绘制的块
    private SpecialEffect effect = SpecialEffect.Double_Step;

    // 块绘制的Momento
    EditMomento blockMomento = new EditMomento();

    // 初始化
    void Start() {
        blockMomento.editObject = MapEditObject.Special;
    }

    // 执行momento上的操作
    void Execute(EditMomento momento) {
        // 把对应坐标上的都改成after
        Board board = ModelResource.board;
        for(int i=0; i<momento.position.Count; i++) {
            board.Add(momento.position[i], (Cell)momento.after[i]);
        }
    }

    // 绘制块
    EditMomento Paint(Vector2Int position) {
        // 无需调用Display，因为修改Model后显示会自动更新
        Board board = ModelResource.board;

        // 不允许在没有陆地的格子添加
        if( !board.Contains(position) || board.Get(position).Walkable == false) 
            return null;
        // 不允许在传送门上添加
        if( board.Get(position).Effect == SpecialEffect.Portal)
            return null;

        // 记录修改前后的状态，修改specialEffect即可
        Cell pre = new Cell(board.Get(position));
        Cell after = new Cell(pre);
        after.Effect = effect;
        
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
    ///   <para> 向正在画的一笔中加入新格子，然后预览已经绘制的部分 </para>
    ///   <para> 注意：Model会被修改！ </para>
    /// </summary>
    public void Preview(Vector2Int position) {
        // 若这一格已经画过了，则无视之
        if(blockMomento.position.Contains(position))
            return;
        
        // 调用Paint进行绘制
        EditMomento momento = Paint(position);
        // 绘制不合法的情况
        if(momento is null)
            return;

        // 维护blockMomento
        blockMomento.position.Add(position);
        blockMomento.pre.Add(momento.pre[0]);
        blockMomento.after.Add(momento.after[0]);
    }

    /// <summary>
    ///   <para> 完成这一笔 </para>
    /// </summary>
    public EditMomento Paint() {
        EditMomento ret = new EditMomento(blockMomento);
        
        // 维护blockMomento
        blockMomento = new EditMomento();
        blockMomento.editObject = MapEditObject.Land;

        return ret;
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

    /// <summary>
    ///   <para> 是否正在绘制（false表示正在擦除） </para>
    /// </summary>
    public SpecialEffect Effect {
        get {return effect;}
        set {effect = value;}
    }
}