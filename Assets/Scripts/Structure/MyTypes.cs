using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Structure {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum TileType {
        Begin,
        Land_Begin,
        Land_Lawn_Green,
        Land_End,
        Special_Begin,
        Special_Portal,
        Special_DoubleStep,
        Special_BrokenBridge,
        Special_End,
        Token_Begin,
        Token_Tank_Blue,
        Token_Tank_Red,
        Token_End,
        End
    };

    public enum TilemapType {
        Begin,
        Land,
        Special,
        Token,
        End
    };

    /// <summary>
    ///   <para> 角色的操控方式 </para>
    /// </summary>
    public enum PlayerChoices{
        Player,     // 玩家控制
        Comuputer,  // AI控制
        Banned      // 此角色不参与游戏
    };
    
    public static class MyTypes {
        private const int TileTypeHead = (int) TileType.Begin + 1;
        private const int TileTypeLandHead = (int) TileType.Land_Begin + 1;
        private const int TileTypeLandTail = (int) TileType.Land_End - 1;
        private const int NTileTypeLand = TileTypeLandTail - TileTypeLandHead + 1;
        private const int TileTypeSpecialHead = (int) TileType.Special_Begin + 1;
        private const int TileTypeSpecialTail = (int) TileType.Special_End - 1;
        private const int NTileTypeSpecial = TileTypeSpecialTail - TileTypeSpecialHead + 1;
        private const int TileTypeTokenHead = (int) TileType.Token_Begin + 1;
        private const int TileTypeTokenTail = (int) TileType.Token_End - 1;
        private const int NTileTypeToken = TileTypeTokenTail - TileTypeTokenHead + 1;
        private const int TileTypeTail = (int) TileType.End - 1;
        private const int NTileType = TileTypeTail - TileTypeHead + 1;
        private const int TilemapTypeHead = (int) TilemapType.Begin + 1;
        private const int TilemapTypeTail = (int) TilemapType.End - 1;
        private const int NTilemapType = TilemapTypeTail - TilemapTypeHead + 1;

        public static TilemapType GetType(TileType tileType) {
            var iTileType = (int) tileType;
            if (Inside(iTileType, TileTypeLandHead, TileTypeLandTail))
                return TilemapType.Land;
            else if (Inside(iTileType, TileTypeSpecialHead, TileTypeSpecialTail))
                return TilemapType.Special;
            else if (Inside(iTileType, TileTypeTokenHead, TileTypeTokenTail))
                return TilemapType.Token;
            else
                return TilemapType.End;
        }

        /// <summary>
        ///   <para>在同一Tilemap下的所有TileType中所排到的位次。位次最小是1。无效为0。</para>
        /// </summary>
        public static int GetId(TileType tileType) {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (GetType(tileType)) {
                case TilemapType.Land:
                    return tileType - TileType.Land_Begin;
                case TilemapType.Special:
                    return tileType - TileType.Special_Begin;
                case TilemapType.Token:
                    return tileType - TileType.Token_Begin;
                default:
                    return 0;
            }
        }

        /// <summary>
        ///   <para>位次最小是1。无效为0。</para>
        /// </summary>
        public static int GetId(TilemapType tilemapType) {
            if (Inside((int) tilemapType, TilemapTypeHead, TilemapTypeTail))
                return tilemapType - TilemapType.Begin;
            else
                return 0;
        }

        /// <summary>
        ///   <para>在同一Tilemap下TileType中进行切换。如果tileType无效则不切换。</para>
        /// </summary>
        public static TileType Shift(TileType tileType, int offset = 1) {
            var iTileType = (int) tileType;
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (GetType(tileType)) {
                case TilemapType.Land:
                    iTileType = TileTypeLandHead + Mod(iTileType - TileTypeLandHead + offset, NTileTypeLand);
                    break;
                case TilemapType.Special:
                    iTileType = TileTypeSpecialHead + Mod(iTileType - TileTypeSpecialHead + offset, NTileTypeSpecial);
                    break;
                case TilemapType.Token:
                    iTileType = TileTypeTokenHead + Mod(iTileType - TileTypeTokenHead + offset, NTileTypeToken);
                    break;
            }

            return (TileType) iTileType;
        }

        /// <summary>
        ///   <para>在有效的Tilemap中进行切换。</para>
        /// </summary>
        public static TilemapType Shift(TilemapType tilemapType, int offset = 1) {
            var iTilemapType = (int) tilemapType;
            iTilemapType = TilemapTypeHead + Mod(iTilemapType - TilemapTypeHead + offset, NTilemapType);
            return (TilemapType) iTilemapType;
        }

        private static int Mod(int x, int m) {
            return (x % m + m) % m;
        }

        private static bool Inside(int x, int l, int r) {
            return l <= x && x <= r;
        }
    }

    public static class Constants {
        public static Dictionary<TileType, string> resourcePathOfTileType = new Dictionary<TileType, string>() {
            {TileType.Land_Lawn_Green, "Tiles/floor-lawnGreen"},
            {TileType.Special_Portal, "Tiles/special-portal"},
            {TileType.Special_DoubleStep, "Tiles/special-doubleStep"},
            {TileType.Special_BrokenBridge, "Tiles/special-brokenBridge"},
            {TileType.Token_Tank_Blue, "Tiles/token-blueTank"},
            {TileType.Token_Tank_Red, "Tiles/token-redTank"},
        };

        public static Dictionary<TilemapType, List<TileType>> tileTypesOfTilemapType =
            new Dictionary<TilemapType, List<TileType>>() {
                {
                    TilemapType.Land,
                    new List<TileType>() {
                        TileType.Land_Lawn_Green
                    }
                }, {
                    TilemapType.Special,
                    new List<TileType>() {
                        TileType.Special_Portal,
                        TileType.Special_BrokenBridge,
                        TileType.Special_DoubleStep
                    }
                }, {
                    TilemapType.Token,
                    new List<TileType>() {
                        TileType.Token_Tank_Blue,
                        TileType.Token_Tank_Red
                    }
                }
            };
    }
}