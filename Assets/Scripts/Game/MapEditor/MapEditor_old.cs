using System.Collections.Generic;
using System.Text;
using Structure;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Widget; // Type definitions
using Cell = UnityEngine.Vector2Int;

namespace Game.MapEditor {
    public class MapEditor_old : MonoBehaviour {
        // Tile types and prefab resources
        private Dictionary<TileType, TileBase> prefabTiles = new Dictionary<TileType, TileBase>();

        Dictionary<TileType, string> SpecialName_ByTileType = new Dictionary<TileType, string> {
            {TileType.Special_BrokenBridge, "brokenBridge"},
            {TileType.Special_DoubleStep, "doubleStep"},
        };

        Dictionary<string, TileType> TileType_BySpecialName = new Dictionary<string, TileType>();

        // Get information on cursor
        public Cursor cursor;

        // Tilemaps
        public TilemapManager tilemapManagerLand = null;
        public TilemapManager tilemapManagerLandPreview = null;
        public TilemapManager tilemapManagerSpecial = null;
        public TilemapManager tilemapManagerSpecialPreview = null;
        public TilemapManager tilemapManagerToken = null;
        public TilemapManager tilemapManagerTokenPreview = null;
        TilemapManager[] tilemapManagers = null;
        TilemapManager[] tilemapManagersPreview = null;

        // Cells
        Cell nullCell = new Cell(int.MaxValue, int.MaxValue);
        Tile nullTile;

        // Selection related and preview related
        [FormerlySerializedAs("tileSelector")] public TypeSelector typeSelector;
        private Dictionary<TileType, Tile> tileOfTileType = new Dictionary<TileType, Tile>();
        private Dictionary<Tile, TileType> tileTypeOfTile = new Dictionary<Tile, TileType>();

        private Dictionary<TilemapType, TilemapManager> tilemapManagerOfTilemapType = null;

        private Dictionary<TilemapType, TilemapManager> tilemapManagerPreviewOfTilemapType = null;

        TilemapManager selectedTilemapManager =>
            tilemapManagerOfTilemapType[typeSelector.GetSelectedTilemapType()];

        TilemapManager selectedTilemapManagerPreview =>
            tilemapManagerPreviewOfTilemapType[typeSelector.GetSelectedTilemapType()];

        Cell lastCell;
        Cell lastPaintedCell;

        TileType lastPaintedTileType;

        TilemapManager lastPaintedTilemapManager;

        // Portal related
        bool buildingPortal = false;
        public PortalPainter portalPainter = null;

        List<Portal> portals;
        Portal newPortal;

        // Start is called before the first frame update
        void Start() {
            nullTile = ScriptableObject.CreateInstance<Tile>();
            nullTile.name = "";

            foreach (KeyValuePair<TileType, string> pair in Constants.resourcePathOfTileType) {
                string path = pair.Value;
                Tile tile = path.Length > 0 ? Resources.Load<Tile>(path) : nullTile;
                tileOfTileType.Add(pair.Key, tile);
                tileTypeOfTile.Add(tile, pair.Key);
            }

            foreach (KeyValuePair<TileType, string> pair in SpecialName_ByTileType)
                TileType_BySpecialName.Add(pair.Value, pair.Key);

            tilemapManagerOfTilemapType = new Dictionary<TilemapType, TilemapManager>() {
                {TilemapType.Land, tilemapManagerLand},
                {TilemapType.Special, tilemapManagerSpecial},
                {TilemapType.Token, tilemapManagerToken}
            };
            tilemapManagerPreviewOfTilemapType = new Dictionary<TilemapType, TilemapManager>() {
                {TilemapType.Land, tilemapManagerLandPreview},
                {TilemapType.Special, tilemapManagerSpecialPreview},
                {TilemapType.Token, tilemapManagerTokenPreview}
            };

            lastCell = nullCell;
            lastPaintedCell = nullCell;
            lastPaintedTileType = TileType.End;

            portals = new List<Portal>();
        }

        // Update is called once per frame
        void Update() {
            Cell cell = cursor.GetPointedCell();

            // Preview
            if (cell != lastCell)
                updatePreview(cell);

            // Paint
            if (Input.GetMouseButton(0)) {
                if (buildingPortal) {
                    if (cell != newPortal.from) {
                        if (selectedTilemapManager.GetTile(cell) != TileType.Special_Portal) {
                            setTile(cell);
                            eraseTile(cell);
                        }
                        else
                            setTile(cell);

                        newPortal.to = cell;
                        portals.Add(newPortal);
                        buildingPortal = false;
                    }
                }
                else if (lastPaintedCell != cell
                         || lastPaintedTileType != typeSelector.GetSelectedTileType()
                         || lastPaintedTilemapManager != selectedTilemapManager) {
                    setTile(cell);
                    if (typeSelector.GetSelectedTileType() == TileType.Special_Portal) {
                        newPortal = portalPainter.Draw(cell, cell);
                        buildingPortal = true;
                    }
                }
            }

            if (buildingPortal) {
                newPortal.to = cell;
            }

            if (Input.GetMouseButton(1)) {
                eraseTile(cell);
                if (typeSelector.GetSelectedTileType() == TileType.Special_Portal) {
                    for (int i = portals.Count - 1; i >= 0; i--)
                        if (portals[i].from == cell) {
                            portals[i].Destroy();
                            portals.RemoveAt(i);
                        }
                }
            }

            // Shift painter
            if (Input.GetKeyUp(KeyCode.Tab)) {
                typeSelector.ShiftSelectedTilemapType(
                    (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        ? -1
                        : 1
                );
                updatePreview(cell);
            }

            // int offset=(int)Input.mouseScrollDelta[1];
            int offset = (Input.GetKeyUp(KeyCode.Space) ? 1 : 0);
            if (offset != 0) {
                typeSelector.ShiftSelectedTileType(offset);
                updatePreview(cell);
            }

            // Shortcuts
            if (Input.GetKeyUp(KeyCode.S)) {
                // && (Input.GetKeyDown(KeyCode.LeftControl)||Input.GetKeyDown(KeyCode.RightControl))){
                // `[ [left|right]Ctrl + ] S`: save current map
                // saveMap();
            }

            if (Input.GetKeyUp(KeyCode.L)) {
                loadMap();
            }
        }

        void updatePreview(Cell cell) {
            eraseTilePreview(lastCell);
            setTilePreview(cell);
            lastCell = cell;
        }

        void eraseTilePreview(Cell cell) {
            tilemapManagerTokenPreview.EraseTile(cell);
            tilemapManagerSpecialPreview.EraseTile(cell);
            tilemapManagerLandPreview.EraseTile(cell);
        }

        bool eraseTile(Cell cell) {
            return selectedTilemapManager.EraseTile(cell);
        }

        void setTilePreview(Cell cell) {
            setTile(selectedTilemapManagerPreview, cell, typeSelector.GetSelectedTileType());
        }

        void setTile(Cell cell) {
            setTile(selectedTilemapManager, cell, typeSelector.GetSelectedTileType());
        }

        void setTile(TilemapManager tilemapManager, Cell cell, TileType tileType) {
            tilemapManager.SetTile(cell, tileType);
            lastPaintedCell = cell;
            lastPaintedTileType = tileType;
            lastPaintedTilemapManager = tilemapManager;
        }

        // void saveMap() {
        // }

        void loadMap() {
            string filename = "MapSample";
            string json = "";
            TextAsset text = Resources.Load<TextAsset>(filename);
            json = text.text;
            BoardEntity boardEntity = JsonUtility.FromJson<BoardEntity>(json);
            foreach (SingleMapGridEntity cell in boardEntity.map)
                tilemapManagerLand.SetTile(new Vector2Int(cell.x, cell.y), TileType.Land_Lawn_Green);
            foreach (SinglePortalEntity portal in boardEntity.portal) {
                newPortal = portalPainter.Draw(
                    new Vector2Int(portal.fromX, portal.fromY),
                    new Vector2Int(portal.toX, portal.toY)
                );
                portals.Add(newPortal);
                tilemapManagerSpecial.SetTile(newPortal.from, TileType.Special_Portal);
            }

            foreach (SingleSpecialEntity special in boardEntity.special) {
                tilemapManagerSpecial.SetTile(
                    new Vector2Int(special.x, special.y),
                    TileType_BySpecialName[special.effect]
                );
            }

            foreach (TokenEntity token in boardEntity.tokens) {
                tilemapManagerToken.SetTile(
                    new Vector2Int(token.x, token.y),
                    token.player == 1 ? TileType.Token_Tank_Blue : TileType.Token_Tank_Red
                );
            }
        }
    }
}