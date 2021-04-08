using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Structure_old {
    public class TilePalette : MonoBehaviour {
        public readonly Dictionary<TileType, Tile> tileOfTileType = new Dictionary<TileType, Tile>();

        public readonly Dictionary<Tile, TileType> tileTypeOfTile = new Dictionary<Tile, TileType>();

        // Start is called before the first frame update
        void Start() {
            foreach (KeyValuePair<TileType, string> pair in Constants.resourcePathOfTileType) {
                Tile tile = Resources.Load<Tile>(pair.Value);
                tileOfTileType.Add(pair.Key, tile);
                tileTypeOfTile.Add(tile, pair.Key);
            }

            tileOfTileType.Add(TileType.End, null);
        }
    }
}