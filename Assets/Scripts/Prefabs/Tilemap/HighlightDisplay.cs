using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///   <para> 显示地图格子的高亮，例如选中格子时 </para>
/// </summary>
public class HighlightDisplay : MonoBehaviour {
    // 高亮自身格子和可到达格子
    // 由于两者必定同时显示，同时消失，所以重叠也无妨
    [SerializeField] private TilemapManager tilemapReachableHighlight;

    // 高亮路径
    [SerializeField] private TilemapManager tilemapRouteHighlight;

    // 正在高亮的路径的终点
    // 用于判断正在高亮的路径是哪一条
    private Vector3Int highlightRouteEnd = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);

    /// <summary>
    ///   <para> 高亮己方棋子（蓝色） </para>
    ///   <para> 用于：用户选中己方棋子时 </para>
    /// </summary>
    public void HighlightToken(Vector2Int position) {
        tilemapReachableHighlight.SetTile(position, TileType.Highlight_Blue);
    }

    /// <summary>
    ///   <para> 高亮可到达的位置（黄色） </para>
    ///   <para> 用于：用户选中己方棋子，系统自动显示棋子可走到的位置 </para>
    /// </summary>
    public void HighlightReachableCell(Vector2Int position) {
        tilemapReachableHighlight.SetTile(position, TileType.Highlight_Yellow);
    }

    /// <summary>
    ///   <para> 取消所有高亮 </para>
    /// </summary>
    public void CancelHighlight() {
        // 清空ReachableHighlight层
        List<Vector2Int> poses = new List<Vector2Int>(tilemapReachableHighlight.CellSet);
        foreach(Vector2Int pos in poses) {
            tilemapReachableHighlight.RemoveTile(pos);
        }
        // 清空RouteHighlight层
        CancelRouteHightlight();
    }

    /// <summary>
    ///   <para> 高亮一条路径（蓝色） </para>
    /// </summary>
    public void HighlightRoute(List<Vector2Int> route) {
        foreach(Vector2Int pos in route) {
            tilemapReachableHighlight.SetTile(pos, TileType.Highlight_Blue);
        }
        highlightRouteEnd = new Vector3Int(route.Last().x, route.Last().y, 0);
    }

    /// <summary>
    ///   <para> 取消高亮路径 </para>
    /// </summary>
    public void CancelRouteHightlight() {
        foreach(Vector2Int pos in tilemapRouteHighlight.CellSet) {
            tilemapRouteHighlight.RemoveTile(pos);
        }
        highlightRouteEnd = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
    }

    /// <summary>
    ///   <para> 正在高亮的路径的终点 </para>
    /// </summary>
    public Vector3Int HighlightRouteEnd {
        get {return highlightRouteEnd;}
    }
}