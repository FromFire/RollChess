using System.Collections.Generic;
using System.Text;
using Structure;
using UnityEngine;
using UnityEngine.Tilemaps; // Type definitions
using Cell = UnityEngine.Vector2Int;
using Types = Structure.Types;

namespace Game.MapEditor {
    public class MapEditor_old : MonoBehaviour {
        // Tile types and prefab resources
        private Dictionary<TileType, TileBase> prefabTiles = new Dictionary<TileType, TileBase>();

        Dictionary<string, TileType> TileType_ByName;

        Dictionary<TileType, string> SpecialName_ByTileType = new Dictionary<TileType, string> {
            {TileType.Special_BrokenBridge, "brokenBridge"},
            {TileType.Special_DoubleStep, "doubleStep"},
        };

        Dictionary<string, TileType> TileType_BySpecialName = new Dictionary<string, TileType>();

        TileType whichTileType(TileBase tile) {
            if (tile is null) return TileType.End;
            return TileType_ByName[tile.name];
        }

        // Get information on cursor
        public Cursor cursor;

        // Tilemaps
        public Tilemap tilemapLand = null;
        public Tilemap tilemapLandPreview = null;
        public Tilemap tilemapSpecial = null;
        public Tilemap tilemapSpecialPreview = null;
        public Tilemap tilemapPortal = null;
        public Tilemap tilemapPortalPreview = null;
        public Tilemap tilemapToken = null;
        public Tilemap tilemapTokenPreview = null;
        Tilemap[] tilemaps = null;
        Tilemap[] tilemapsPreview = null;

        // Cells
        Cell nullCell = new Cell(int.MaxValue, int.MaxValue);
        TileBase nullTile;

        // Selection related and preview related
        Cell lastCell;
        Cell lastPaintedCell;
        TilemapType selectedTilemapType = TilemapType.Land;

        TileType[] selectedTileTypes = {
            TileType.Land_Lawn_Green,
            TileType.Special_BrokenBridge,
            TileType.Token_Tank_Blue
        };

        TileType selectedTileType {
            get { return selectedTileTypes[Types.GetId(selectedTilemapType) - 1]; }
            set { selectedTileTypes[Types.GetId(selectedTilemapType) - 1] = value; }
        }

        TileType lastPaintedTileType;

        Tilemap selectedTilemap {
            get { return tilemaps[Types.GetId(selectedTilemapType) - 1]; }
        }

        Tilemap lastPaintedTilemap;

        Tilemap selectedTilemapPreview {
            get { return tilemapsPreview[Types.GetId(selectedTilemapType) - 1]; }
        }

        // Portal related
        bool buildingPortal = false;

        class Line {
            LineRenderer mRenderer;
            GameObject mObject;
            public static GameObject parent;
            Tilemap mTilemap;
            Cell mFrom;
            Cell mTo;

            public Cell from {
                get { return mFrom; }
                set {
                    mFrom = value;
                    SetPosition(0, value);
                }
            }

            public Cell to {
                get { return mTo; }
                set {
                    mTo = value;
                    SetPosition(1, value);
                }
            }

            public Line(Tilemap tilemap) {
                mObject = new GameObject("line");
                mObject.AddComponent<LineRenderer>();
                mRenderer = mObject.GetComponent<LineRenderer>();
                mRenderer.startWidth = (float) 0.1;
                mRenderer.endWidth = (float) 0.1;
                mRenderer.material = new Material(Shader.Find("Sprites/Default"));
                mRenderer.startColor = Color.cyan;
                mRenderer.endColor = Color.white;
                mRenderer.sortingLayerID = SortingLayer.NameToID("PortalArrows");
                mTilemap = tilemap;
            }

            public void SetPosition(int idx, Cell cell) {
                mRenderer.SetPosition(idx, mTilemap.CellToLocal(new Vector3Int(cell.x, cell.y, 0)));
            }

            public void SetTilemap(Tilemap tilemap) {
                mTilemap = tilemap;
                from = from;
                to = to;
            }

            public void Destroy() {
                GameObject.Destroy(mObject);
            }
        }

        List<Line> lines;
        Line newLine;


        // Start is called before the first frame update
        void Start() {
            nullTile = Tile.CreateInstance<Tile>();
            // nullTile=new Tile();
            nullTile.name = "";
            TileType_ByName = new Dictionary<string, TileType>();

            foreach (KeyValuePair<TileType, string> pair in Constants.resourcePathOfTileType) {
                string path = pair.Value;
                TileBase tile = path.Length > 0 ? Resources.Load<TileBase>(path) : nullTile;
                prefabTiles.Add(pair.Key, tile);
                if (TileType_ByName.ContainsKey(tile.name)) continue;
                TileType_ByName.Add(tile.name, pair.Key);
            }

            foreach (KeyValuePair<TileType, string> pair in SpecialName_ByTileType)
                TileType_BySpecialName.Add(pair.Value, pair.Key);

            tilemaps = new Tilemap[] {tilemapLand, tilemapSpecial, tilemapToken};
            tilemapsPreview = new Tilemap[] {tilemapLandPreview, tilemapSpecialPreview, tilemapTokenPreview};

            lastCell = nullCell;
            lastPaintedCell = nullCell;
            lastPaintedTileType = TileType.End;

            lines = new List<Line>();
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
                    if (cell != newLine.from) {
                        if (whichTileType(selectedTilemap.GetTile((Vector3Int) cell)) != TileType.Special_Portal) {
                            setTile(cell);
                            eraseTile(cell);
                        }
                        else
                            setTile(cell);

                        newLine.to = cell;
                        newLine.SetTilemap(tilemapPortal);
                        lines.Add(newLine);
                        buildingPortal = false;
                    }
                }
                else if (lastPaintedCell != cell
                         || lastPaintedTileType != selectedTileType
                         || lastPaintedTilemap != selectedTilemap) {
                    setTile(cell);
                    if (selectedTileType == TileType.Special_Portal) {
                        newLine = new Line(tilemapPortalPreview);
                        newLine.from = newLine.to = cell;
                        buildingPortal = true;
                    }
                }
            }

            if (buildingPortal) {
                newLine.to = cell;
            }

            if (Input.GetMouseButton(1)) {
                eraseTile(cell);
                if (selectedTileType == TileType.Special_Portal) {
                    for (int i = lines.Count - 1; i >= 0; i--)
                        if (lines[i].from == cell) {
                            lines[i].Destroy();
                            lines.RemoveAt(i);
                        }
                }
            }

            // Shift painter
            if (Input.GetKeyUp(KeyCode.Tab)) {
                shiftSelectedTilemapType(
                    (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        ? -1
                        : 1
                );
                updatePreview(cell);
            }

            // int offset=(int)Input.mouseScrollDelta[1];
            int offset = (Input.GetKeyUp(KeyCode.Space) ? 1 : 0);
            if (offset != 0) {
                shiftSelectedTileType(offset);
                updatePreview(cell);
            }

            // Shortcuts
            if (Input.GetKeyUp(KeyCode.S)) {
                // && (Input.GetKeyDown(KeyCode.LeftControl)||Input.GetKeyDown(KeyCode.RightControl))){
                // `[ [left|right]Ctrl + ] S`: save current map
                saveMap();
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

        void shiftSelectedTilemapType(int offset = 1) {
            selectedTilemapType = Types.Shift(selectedTilemapType, offset);
        }

        void shiftSelectedTileType(int offset = 1) {
            selectedTileType = Types.Shift(selectedTileType, offset);
        }

        void eraseTilePreview(Cell cell) {
            eraseTile(tilemapTokenPreview, cell);
            eraseTile(tilemapSpecialPreview, cell);
            eraseTile(tilemapLandPreview, cell);
        }

        bool eraseTile(Cell cell) {
            return eraseTile(selectedTilemap, cell);
        }

        bool eraseTile(Tilemap tilemap, Cell cell) {
            if (tilemap.GetTile((Vector3Int) cell) is null) return false;
            tilemap.SetTile((Vector3Int) cell, null);
            return true;
        }

        void setTilePreview(Cell cell) {
            setTile(selectedTilemapPreview, cell, selectedTileType);
        }

        void setTilePreview(Cell cell, TileType tileType) {
            setTile(selectedTilemapPreview, cell, tileType);
        }

        void setTile(Cell cell) {
            setTile(selectedTilemap, cell, selectedTileType);
        }

        void setTile(Cell cell, TileType tileType) {
            setTile(selectedTilemap, cell, tileType);
        }

        void setTile(Tilemap tilemap, Cell cell, TileType tileType) {
            tilemap.SetTile((Vector3Int) cell, prefabTiles[tileType]);
            lastPaintedCell = cell;
            lastPaintedTileType = tileType;
            lastPaintedTilemap = tilemap;
        }

        (List<Cell> cells, List<TileType> tileTypes) getTiles(Tilemap tilemap) {
            (List<Cell> cells, List<TileType> tileTypes) ret = (new List<Cell>(), new List<TileType>());
            Vector3Int min = tilemap.cellBounds.min;
            Vector3Int max = tilemap.cellBounds.max;
            Cell cell = Cell.zero;
            for (cell.x = min.x; cell.x < max.x; cell.x++)
            for (cell.y = min.y; cell.y < max.y; cell.y++) {
                TileBase tile = tilemap.GetTile((Vector3Int) cell);
                if (tile is null) continue;
                ret.cells.Add(cell);
                ret.tileTypes.Add(whichTileType(tile));
            }

            return ret;
        }

        string join(string[] stringArray) {
            return string.Join("\n", stringArray) + "\n";
        }

        void saveMap() {
            Debug.Log("Saving...\nDo not shutdown game now.");
            string save = "";
            save += join(new string[] {
                "{",
                "   \"mapName\":\"savedMap\",",
                "   \"player\": {",
                "       \"number\":2",
                "   },",
                "   \"tokens\": [",
            });
            List<Cell> cells = new List<Cell>();
            List<TileType> tileTypes = new List<TileType>();
            (cells, tileTypes) = getTiles(tilemapToken);
            int x, y, player;
            for (int i = 0; i < cells.Count; i++) {
                x = cells[i].x;
                y = cells[i].y;
                player = Types.GetId(tileTypes[i]) - 1;
                save += "       {\"x\":" + x + ", \"y\":" + y + ", \"player\":" + player +
                        (i == cells.Count - 1 ? "}\n" : "},\n");
            }

            save += join(new string[] {
                "   ],",
                "   \"map\": [",
            });
            (cells, tileTypes) = getTiles(tilemapLand);
            for (int i = 0; i < cells.Count; i++) {
                x = cells[i].x;
                y = cells[i].y;
                save += "       {\"x\":" + x + ", \"y\":" + y + (i == cells.Count - 1 ? "}\n" : "},\n");
            }

            save += join(new string[] {
                "   ],",
                "   \"special\": [",
            });
            (cells, tileTypes) = getTiles(tilemapSpecial);
            for (int i = cells.Count - 1; i >= 0; i--)
                if (tileTypes[i] == TileType.Special_Portal) {
                    cells.RemoveAt(i);
                    tileTypes.RemoveAt(i);
                }

            for (int i = 0; i < cells.Count; i++) {
                x = cells[i].x;
                y = cells[i].y;
                save += "       {\"x\":" + x + ", \"y\":" + y + ", \"effect\":\""
                        + SpecialName_ByTileType[tileTypes[i]] + "\"}"
                        + (i == cells.Count - 1 ? "\n" : ",\n");
            }

            save += join(new string[] {
                "   ],",
                "   \"portal\": [",
            });
            for (int i = 0; i < lines.Count; i++) {
                Line line = lines[i];
                save += "       {\"fromX\":" + line.from.x + ", "
                        + "\"fromY\": " + line.from.y + ", "
                        + "\"toX\": " + line.to.x + ", "
                        + "\"toY\": " + line.to.y + "}"
                        + (i == lines.Count - 1 ? "\n" : ",\n");
            }

            save += join(new string[] {
                "   ]",
                "}"
            });
            string filename = "savedMap.json";
            System.IO.File.WriteAllText(filename, save, Encoding.UTF8);
            Debug.Log("Saved as " + filename);
        }

        void loadMap() {
            string filename = "MapSample";
            string json = "";
            TextAsset text = Resources.Load<TextAsset>(filename);
            json = text.text;
            BoardEntity boardEntity = JsonUtility.FromJson<BoardEntity>(json);
            foreach (SingleMapGridEntity cell in boardEntity.map)
                setTile(tilemapLand, new Cell(cell.x, cell.y), TileType.Land_Lawn_Green);
            foreach (SinglePortalEntity portal in boardEntity.portal) {
                newLine = new Line(tilemapPortal);
                newLine.from = new Cell(portal.fromX, portal.fromY);
                newLine.to = new Cell(portal.toX, portal.toY);
                lines.Add(newLine);
                setTile(tilemapSpecial, newLine.from, TileType.Special_Portal);
                // setTile(tilemapSpecial,newLine.to,TileType.Special_Portal);
            }

            foreach (SingleSpecialEntity special in boardEntity.special) {
                setTile(
                    tilemapSpecial, new Cell(special.x, special.y),
                    TileType_BySpecialName[special.effect]
                );
            }

            foreach (TokenEntity token in boardEntity.tokens) {
                setTile(
                    tilemapToken, new Cell(token.x, token.y),
                    token.player == 1 ? TileType.Token_Tank_Blue : TileType.Token_Tank_Red
                );
            }
        }
    }
}