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

        public void SetTile(Vector2Int cell, TileType tileType) {
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
            return true;
        }
    }
}