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
        int boardUp = (board.BorderUp == int.MinValue) ? 0 : board.BorderUp;
        int boardRight = (board.BorderRight == int.MinValue) ? 0 : board.BorderRight;
        int boardLeft = (board.BorderLeft == int.MaxValue) ? 0 : board.BorderLeft;
        int borderDown = (board.BorderDown == int.MaxValue) ? 0 : board.BorderDown;

        // 绘制，边界余量20
        for(int i=boardLeft - 20; i<boardRight + 20; i++) {
            for(int j=borderDown - 20; j<boardUp + 20; j++) {
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
    }
}
