using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Structure;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Widget;

namespace Game.MapEditor {
    public class MapEditor : MonoBehaviour {
        // 用于动态维护地图数据
        private BaseBoard<SingleGrid> board = new BaseBoard<SingleGrid>();
        private List<TokenEntity> tokens = new List<TokenEntity>();
        private HashSet<int> players;
        private PlayersEntity player = new PlayersEntity(0, 0);

        // Tilemap们
        public Tilemap tilemapBoard;
        public Tilemap tilemapBoardPreview;
        public Tilemap tilemapSpecial;
        public Tilemap tilemapSpecialPreview;
        public Tilemap tilemapToken;
        public Tilemap tilemapTokenPreview;
        private Tilemap selectedTilemap;
        private Tilemap selectedTilemapPreview;

        // Tile们
        private BoardDisplay.TileKeys boardTileKey = BoardDisplay.TileKeys.floorLawnGreen;
        private List<Tile> boardTileList;
        private TokensDisplay.TileKeys tokensTileKey = TokensDisplay.TileKeys.tokenRedAlien;
        private List<Tile> tokensTileList;
        private Tile selectedTile;

        // 用于获取鼠标所在的坐标
        public Cursor cursor = null;

        // 用于绘制传送通道
        public PortalPainter portalPainter = null;

        // 选定的坐标
        private Vector2Int _selectedCell;

        private Vector2Int selectedCell {
            get { return _selectedCell; }
            set {
                lastSelectedCell = _selectedCell;
                _selectedCell = value;
            }
        }

        // 上一个选定的坐标
        private Vector2Int lastSelectedCell;

        // 已建的传送通道
        private Dictionary<Vector2Int, Portal> portals = new Dictionary<Vector2Int, Portal>();

        // 正在构建的传送通道
        private Portal newPortal = null;

        // 编辑器状态集
        enum EditorState {
            Normal, // 一般模式
            Portal // 正在绘制传送通道
        };

        // 编辑器当前的状态
        private EditorState editorState = EditorState.Normal;

        // 弹窗
        public Popup popup;

        // 弹窗时间间隔
        public float popupDelay = 0.6f;

        private void Start() {
            selectedTilemap = tilemapBoard;
            selectedTilemapPreview = tilemapBoardPreview;
            // tile顺序按照enum tileKeys中规定的来
            List<string> tokensTileNames = new List<string> {
                "Tiles/token-redAlien", //tokenRedAlien
                "Tiles/token-blueAlien", //tokenBlueAlien
                "Tiles/token-yellowAlien", //tokenYellowAlien
                "Tiles/token-greenAlien", //tokenGreenAlien
                "Tiles/token-neutralAlien" //tokenNeutralAlien
            };
            List<string> boardTileNames = new List<string> {
                "Tiles/floor-lawnGreen", //floorLawnGreen
                "Tiles/special-brokenBridge", //special_brokenBridge
                "Tiles/special-doubleStep", //special_doubleStep
                "Tiles/special-portal", //special_portal
            };

            // 读取所有tile
            tokensTileList = new List<Tile>();
            foreach (string name in tokensTileNames) {
                tokensTileList.Add(Resources.Load<Tile>(name));
            }

            boardTileList = new List<Tile>();
            foreach (string name in boardTileNames) {
                boardTileList.Add(Resources.Load<Tile>(name));
            }

            selectedTile = boardTileList[(int) BoardDisplay.TileKeys.floorLawnGreen];
        }

        void Update() {
            // 更新预览
            UpdatePreview();

            // 更新鼠标选中的坐标
            selectedCell = cursor.GetPointedCell();

            // 鼠标左键绘制
            if (Input.GetMouseButtonUp(0)) {
                PaintTile();
            }

            // 鼠标右键清除
            if (Input.GetMouseButton(1)) {
                EraseTile();
            }

            // 键盘Tab/S-Tab切换层
            if (Input.GetKeyUp(KeyCode.Tab)) {
                bool reversed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                int i;
                if (selectedTilemap == tilemapBoard)
                    i = 0;
                else if (selectedTilemap == tilemapSpecial)
                    i = 1;
                else
                    i = 2;
                i = (i + (reversed ? -1 : 1) + 3) % 3;
                switch (i) {
                    case 0:
                        selectedTilemap = tilemapBoard;
                        selectedTilemapPreview = tilemapBoardPreview;
                        break;
                    case 1:
                        selectedTilemap = tilemapSpecial;
                        selectedTilemapPreview = tilemapSpecialPreview;
                        break;
                    default:
                        selectedTilemap = tilemapToken;
                        selectedTilemapPreview = tilemapTokenPreview;
                        break;
                }

                UpdatePreview(true);
            }

            // 键盘Space/S-Space切换块
            if (Input.GetKeyUp(KeyCode.Space)) {
                bool reversed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                List<Tile> tileList = (boardTileList.Contains(selectedTile) ? boardTileList : tokensTileList);
                int i = tileList.IndexOf(selectedTile);
                i = (i + (reversed ? -1 : 1) + tileList.Count) % tileList.Count;
                selectedTile = tileList[i];
                UpdatePreview(true);
            }

            // 键盘S存档
            if (Input.GetKeyUp(KeyCode.S)) {
                Save("Untitled.json");
            }

            // 键盘L读档
            if (Input.GetKeyUp(KeyCode.L)) {
                Load("Untitled.json");
            }

            // 更新预览
            UpdatePreview();
        }

        // 将手上选择的块放到选定的坐标上
        void PaintTile() {
            // 如果在画传送站终点
            if (editorState == EditorState.Portal) {
                // 如果尝试使起点和终点相同，那么终止此次传送站的建立
                if (selectedCell == newPortal.from) {
                    selectedTilemap.SetTile((Vector3Int) selectedCell, null);
                    newPortal.Destroy();
                }
                // 否则，完成传送通道（不用清除终点位置的其他块）
                else {
                    portals.Add(newPortal.from, newPortal);
                }

                editorState = EditorState.Normal;
            }
            // 如果在画传送站起点
            else if (selectedTile == boardTileList[(int) BoardDisplay.TileKeys.special_portal]) {
                EraseTile();
                // 画上传送站起点
                selectedTilemap.SetTile((Vector3Int) selectedCell, selectedTile);
                // 创建传送通道预览
                newPortal = portalPainter.Draw(selectedCell, selectedCell);
                editorState = EditorState.Portal;
            }
            // 一般情况下
            else {
                EraseTile();
                // 直接把块放上去
                selectedTilemap.SetTile((Vector3Int) selectedCell, selectedTile);
                if (selectedTilemap == tilemapBoard) {
                    SingleGrid singleGrid = new SingleGrid(true);
                    board.SetData(selectedCell, singleGrid);
                    // TODO
                }
                else if (selectedTilemap == tilemapSpecial) {
                    SingleGrid singleGrid = new SingleGrid(true);
                    board.SetData(selectedCell, singleGrid);
                    // TODO
                }
                else {
                    int playerId = tokensTileList.IndexOf(selectedTile);
                    TokenEntity tokenEntity = new TokenEntity(selectedCell.x, selectedCell.y, playerId);
                    players.Add(playerId);
                    player.max = Math.Max(player.max, players.Count);
                    player.min = Math.Min(2, Math.Max(player.min, players.Count));
                    tokens.Add(tokenEntity);
                }
            }
        }

        // 将选定坐标上的块给清除掉
        void EraseTile() {
            // 如果即将被清除的块是某个传送站的起点，那么破坏掉它所对应的传送通道
            if (board.GetData(selectedCell).SpecialEffect == SingleGrid.Effect.Portal) {
                if (portals.ContainsKey(selectedCell)) {
                    // 已画好的传送通道
                    portals[selectedCell].Destroy();
                    portals.Remove(selectedCell);
                }
                else {
                    // 正在画的传送通道
                    newPortal.Destroy();
                    editorState = EditorState.Normal;
                }
            }

            // 清除目标块
            selectedTilemap.SetTile((Vector3Int) selectedCell, null);
        }

        // 更新预览
        void UpdatePreview(bool force = false) {
            // 只要鼠标没动，不必刷新，就尽量不刷新
            if (!force && lastSelectedCell == selectedCell) return;

            // 预览手上的块
            tilemapBoardPreview.SetTile((Vector3Int) lastSelectedCell, null);
            tilemapSpecialPreview.SetTile((Vector3Int) lastSelectedCell, null);
            tilemapTokenPreview.SetTile((Vector3Int) lastSelectedCell, null);
            selectedTilemapPreview.SetTile((Vector3Int) selectedCell, selectedTile);

            // 预览正在构建的传送通道
            if (editorState == EditorState.Portal) {
                newPortal.to = selectedCell;
            }
        }

        /// <summary>
        ///   <para>将map、tokens、player中的数据存到filename文件中</para>
        /// </summary>
        void Save(string filename) {
            BoardEntity boardEntity = new BoardEntity {
                map = new List<SingleMapGridEntity>(),
                special = new List<SingleSpecialEntity>(),
                portal = new List<SinglePortalEntity>(),
                tokens = tokens,
                player = player
            };
            foreach (Vector2Int pos in board.ToPositionsSet()) {
                SingleGrid singleGrid = board.GetData(pos);
                if (singleGrid.walkable)
                    boardEntity.map.Add(new SingleMapGridEntity(pos.x, pos.y));
                if (singleGrid.SpecialEffect == SingleGrid.Effect.BrokenBridge)
                    boardEntity.special.Add(new SingleSpecialEntity(pos.x, pos.y, "brokenBridge"));
                if (singleGrid.SpecialEffect == SingleGrid.Effect.DoubleStep)
                    boardEntity.special.Add(new SingleSpecialEntity(pos.x, pos.y, "doubleStep"));
                if (singleGrid.SpecialEffect == SingleGrid.Effect.Portal)
                    boardEntity.portal.Add(new SinglePortalEntity(
                        pos.x, pos.y,
                        singleGrid.PortalTarget.x, singleGrid.PortalTarget.y
                    ));
            }

            File.WriteAllText(
                "Assets/Resources/" + filename,
                boardEntity.ToJson()
            );
        }

        /// <summary>
        ///   <para>将filename文件中的数据加载到map、tokens、player上</para>
        /// </summary>
        void Load(string filename) {
            BoardEntity boardEntity = BoardEntity.FromJson(
                Resources.Load<TextAsset>(filename).text
            );
            foreach (SingleMapGridEntity singleMapGridEntity in boardEntity.map) {
                board.Add(
                    new Vector2Int(singleMapGridEntity.x, singleMapGridEntity.y),
                    new SingleGrid(true)
                );
            }

            foreach (SingleSpecialEntity singleSpecialEntity in boardEntity.special) {
                SingleGrid singleGrid = board.GetData(
                    new Vector2Int(singleSpecialEntity.x, singleSpecialEntity.y)
                );
                if (singleSpecialEntity.effect == "brokenBridge")
                    singleGrid.SpecialEffect = SingleGrid.Effect.BrokenBridge;
                else if (singleSpecialEntity.effect == "doubleStep")
                    singleGrid.SpecialEffect = SingleGrid.Effect.DoubleStep;
                else
                    singleGrid.SpecialEffect = SingleGrid.Effect.None;
            }

            foreach (SinglePortalEntity singlePortalEntity in boardEntity.portal) {
                SingleGrid singleGridFrom = board.GetData(
                    new Vector2Int(singlePortalEntity.fromX, singlePortalEntity.fromY)
                );
                singleGridFrom.PortalTarget = new Vector2Int(singlePortalEntity.toX, singlePortalEntity.toY);
                singleGridFrom.SpecialEffect = SingleGrid.Effect.Portal;
            }
        }
    }
}