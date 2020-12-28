using System.Collections.Generic;
using Structure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Widget {
    /// <summary>
    ///   <para>管理Tilemap。</para>
    /// </summary>
    public class TilemapManager : MonoBehaviour {
        public Tilemap tilemap;
        public TilemapType type;
        public TilePalette palette;
        public readonly HashSet<Vector2Int> cells=new HashSet<Vector2Int>();

        public void SetTile(Vector2Int cell, TileType tileType) {
            if (!cells.Contains(cell)) cells.Add(cell);
            tilemap.SetTile((Vector3Int) cell, palette.tileOfTileType[tileType]);
        }

        public TileType GetTile(Vector2Int cell) {
            Tile tile = tilemap.GetTile<Tile>((Vector3Int) cell);
            if (tile is null) return TileType.End;
            else return palette.tileTypeOfTile[tile];
        }

        public bool EraseTile(Vector2Int cell) {
            if (tilemap.GetTile((Vector3Int) cell) is null) return false;
            tilemap.SetTile((Vector3Int) cell, null);
            cells.Remove(cell);
            return true;
        }

        public Vector2Int WorldToCell(Vector3 loc) {
            Vector3Int vector = tilemap.WorldToCell(loc);
            return new Vector2Int(vector.x, vector.y);
        }
    }
}