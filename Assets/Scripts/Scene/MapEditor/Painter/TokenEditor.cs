using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
///   <para> 棋子绘制 </para>
/// </summary>
public class TokenEditor : MonoBehaviour, Paint {
    
    // 正在绘制的玩家
    private PlayerID player = PlayerID.Red;

    // 块绘制的Momento
    EditMomento blockMomento = new EditMomento();

    // 初始化
    void Start() {
        blockMomento.editObject = MapEditObject.Token;
    }

    // 执行momento上的操作
    void Execute(EditMomento momento) {
        // 把对应坐标上的都改成after
        TokenSet tokenSet = ModelResource.tokenSet;
        for(int i=0; i<momento.position.Count; i++) {
            Token after = (Token)(momento.after[i]);
            // 若after的player为none，则移除此棋子
            if(after.Player == PlayerID.None)
                tokenSet.Remove(after.Position);
            else
                tokenSet.Add((Token)momento.after[i]);
        }
    }

    // 绘制块
    public EditMomento Paint(Vector2Int position) {
        // 无需调用Display，因为修改Model后显示会自动更新
        TokenSet tokenSet = ModelResource.tokenSet;

        // 不允许在没有实体块的格子上加人
        Board board = ModelResource.board;
        if(!board.Contains(position) || board.Get(position).Walkable == false)
            return null;

        // 记录修改前后的状态
        Token pre, after;
        // 若已有记录，只需把player改为当前的player
        if(ModelResource.tokenSet.Contains(position)) {
            pre = tokenSet.Get(position);
            after = new Token(pre);
            after.Player = player;
        }
        // 若无记录，则修改后当前的player的默认Token
        else {
            pre = new Token(position, PlayerID.None);
            after = new Token(position, player);
        }
        
        // 生成Momento
        EditMomento momento = new EditMomento();
        momento.editObject = MapEditObject.Token;
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
        blockMomento.editObject = MapEditObject.Token;

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
    public PlayerID Player {
        get {return player;}
        set {player = value;}
    }
}