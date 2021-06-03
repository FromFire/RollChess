using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
///   <para> 显示地图的六边形网格 </para>
///   <para> 比地图边界大一圈（20格） </para>
/// </summary>
public class HexGridDisplay : MonoBehaviour
{
    // 显示网格的Tilemap
    [SerializeField] private TilemapManager tilemap;

    // 是否显示网格
    private bool visible;

    // 已填充的边界
    (int up, int down, int left, int right) border;

    // 填充边界到地图边界的余量，默认20
    int padding = 20;

    void Start() {
        ShowGrid();
        // 注册更新
        ModelResource.boardSubject.Attach(ModelModifyEvent.Cell, UpdateSelf);
    }

    // 显示格子
    void ShowGrid() {
        // 获取地图的边界，若没有边界（地图为空），则认为地图边界是(0,0)
        Board board = Board.Get();
        border = GetBoarder();

        // 绘制，边界余量20
        for(int i=border.left; i<=border.right; i++) {
            for(int j=border.down; j<=border.up; j++) {
                tilemap.SetTile(new Vector2Int(i,j), TileType.HexGrid);
            }
        }
    }

    // 隐藏格子
    void HideGrid() {
        // 填充
        for(int i=border.left; i<=border.right; i++) {
            for(int j=border.down; j<=border.up; j++) {
                tilemap.RemoveTile(new Vector2Int(i,j));
            }
        }
    }

    /// <summary>
    ///   <para> 响应Board更新</para>
    /// </summary>
    public void UpdateSelf(Vector2Int position) {
        // 不可见时无视
        if(!visible)
            return;

        // 更新边界
        (int up, int down, int left, int right) newBorder = GetBoarder();
        if(newBorder == border)
            return;
        ShowGrid();
    }

    // 获取应填充的边界
    (int up, int down, int left, int right) GetBoarder() {
        Board board = Board.Get();
        int up = ((board.BorderUp == int.MinValue) ? 0 : board.BorderUp) + padding;
        int right = ((board.BorderRight == int.MinValue) ? 0 : board.BorderRight) + padding;
        int left = ((board.BorderLeft == int.MaxValue) ? 0 : board.BorderLeft) - padding;
        int down = ((board.BorderDown == int.MaxValue) ? 0 : board.BorderDown) - padding;
        return (up, down, left, right);
    }

    /// <summary>
    ///   <para> 是否显示网格 </para>
    /// </summary>
    public bool Visible {
        get { return visible; }
        set {
            if(value) ShowGrid();
            else HideGrid();
            visible = value;
        }
    }
}
