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

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display() {
        // 记录填充的边界，余量20
        Board board = PublicResource.board;
        for(int i=board.BorderLeft - 20; i<board.BorderRight + 20; i++) {
            for(int j=board.BorderDown - 20; j<board.BorderUp + 20; j++) {
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
    public void Update(Vector2Int position) {
        Cell cell = PublicResource.board.Get(position);

        // 不可走 + 无特效：代表该格子已被移除，填充为海洋
        if(!cell.Walkable && cell.Effect == SpecialEffect.None) {
            tilemap.SetTile(position, TileType.Ocean);
        }
    }
}
