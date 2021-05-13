using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
///   <para> 显示不可走的格子，即背景格 </para>
///   <para> 比地图边界大一圈（20格） </para>
/// </summary>
public class UnwalkableDisplay : MonoBehaviour
{
    // 显示的tilemap
    // 由于海和陆地有相互遮挡关系，本类的tilemap和BoardDisplay中的tilemap是同一个
    [SerializeField] private TilemapManager tilemap;

    // 已填充的边界
    (int up, int down, int left, int right) border;

    // 填充边界到地图边界的余量，默认20
    int padding = 20;

    void Start() {
        Display();
        // 注册更新
        ModelResource.boardSubject.Attach(ModelModifyEvent.Cell, UpdateSelf);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display() {
        // 获取地图的边界，若没有边界（地图为空），则认为地图边界是(0,0)
        Board board = ModelResource.board;
        border = GetBoarder();

        // 绘制，边界余量20
        for(int i=border.left; i<=border.right; i++) {
            for(int j=border.down; j<=border.up; j++) {
                // 避免和陆地冲突
                HashSet<Vector2Int> cellSet = tilemap.CellSet;
                if(!cellSet.Contains(new Vector2Int(i,j)))
                    tilemap.SetTile(new Vector2Int(i,j), TileType.Ocean);
            }
        }
    }

    /// <summary>
    ///   <para> 响应Board更新</para>
    /// </summary>
    public void UpdateSelf(Vector2Int position) {
        Cell cell = ModelResource.board.Get(position);

        // 不可走 + 无特效：代表该格子已被移除，填充为海洋
        if(!cell.Walkable && cell.Effect == SpecialEffect.None) {
            tilemap.SetTile(position, TileType.Ocean);
        }

        // 更新边界
        (int up, int down, int left, int right) newBorder = GetBoarder();
        if(newBorder == border)
            return;
        // 边界扩大时，绘制随之扩大，缩小的不管
        for(int i=newBorder.left; i<=newBorder.right; i++) {
            for(int j=newBorder.down; j<=newBorder.up; j++) {
                cell = ModelResource.board.Get(new Vector2Int(i, j));
                // 如果在newBorder内而不在border内，则画之
                // 和上面的不合并，因为不确定position是否在newBorder内
                if( cell is null || (!cell.Walkable && cell.Effect == SpecialEffect.None) )
                    tilemap.SetTile(new Vector2Int(i,j), TileType.Ocean);
            }
        }
        border = newBorder;
    }

    // 获取应填充的边界
    (int up, int down, int left, int right) GetBoarder() {
        Board board = ModelResource.board;
        int up = ((board.BorderUp == int.MinValue) ? 0 : board.BorderUp) + padding;
        int right = ((board.BorderRight == int.MinValue) ? 0 : board.BorderRight) + padding;
        int left = ((board.BorderLeft == int.MaxValue) ? 0 : board.BorderLeft) - padding;
        int down = ((board.BorderDown == int.MaxValue) ? 0 : board.BorderDown) - padding;
        return (up, down, left, right);
    }
}
