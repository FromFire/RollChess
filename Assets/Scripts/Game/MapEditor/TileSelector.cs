using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Structure;
using UnityEngine;

namespace Game.MapEditor {
    /// <summary>
    ///   <para>Tile切换装置。</para>
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class TileSelector : MonoBehaviour {
        private List<TileType> selectedTileTypes = new List<TileType>() {
            TileType.Land_Lawn_Green,
            TileType.Special_Portal,
            TileType.Token_Tank_Red
        };

        private TileType selectedTileType {
            get => selectedTileTypes[selectedTilemapTypeId];
            set => selectedTileTypes[selectedTilemapTypeId] = value;
        }

        private TilemapType selectedTilemapType = TilemapType.Land;

        private int selectedTilemapTypeId => MyTypes.GetId(selectedTilemapType) - 1;

        public TileType GetSelectedTileType() {
            return selectedTileType;
        }

        public void ShiftSelectedTileType(int offset = 1) {
            MyTypes.Shift(selectedTileType, offset);
        }

        public TilemapType GetSelectedTilemapType() {
            return selectedTilemapType;
        }

        public void ShiftSelectedTilemapType(int offset = 1) {
            MyTypes.Shift(selectedTilemapType, offset);
        }
    }
}