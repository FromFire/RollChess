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

    // 已填充的地图边界
    int borderUp, borderDown, borderLeft, borderRight;

    void Start() {
        ShowGrid();
    }

    // 显示格子
    void ShowGrid() {
        // 记录填充的边界，余量20
        Board board = ModelResource.board;
        borderUp = board.BorderUp + 20;
        borderDown = board.BorderDown - 20;
        borderLeft = board.BorderLeft - 20;
        borderRight = board.BorderRight + 20;

        // 填充
        for(int i=borderLeft; i<borderRight; i++) {
            for(int j=borderDown; j<borderUp; j++) {
                tilemap.SetTile(new Vector2Int(i,j), TileType.HexGrid);
            }
        }
    }

    // 隐藏格子
    void HideGrid() {
        // 填充
        for(int i=borderLeft; i<borderRight; i++) {
            for(int j=borderDown; j<borderUp; j++) {
                tilemap.RemoveTile(new Vector2Int(i,j));
            }
        }
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
