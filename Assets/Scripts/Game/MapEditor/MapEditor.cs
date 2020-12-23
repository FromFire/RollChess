using System;
using System.Collections.Generic;
using System.Text;
using Structure;
using UnityEngine;
using UnityEngine.Serialization;
using Widget;

namespace Game.MapEditor {
    public class MapEditor : MonoBehaviour {
        // 用于获取鼠标所在的坐标
        public Cursor cursor = null;

        // 用于获取用户切换的层、块
        public TypeSelector typeSelector = null;

        // 用于获取游戏中的实际对象
        public ObjectSelector objectSelector = null;

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

        // 手上选择的块
        private TileType selectedTileType {
            get { return typeSelector.GetSelectedTileType(); }
        }

        // 选择的层（一个TilemapManager为一层）
        private TilemapManager selectedTilemapManager {
            get { return objectSelector.GetTilemapManager(typeSelector.GetSelectedTilemapType()); }
        }

        // 选择的层对应的预览（一个TilemapManager为一层）
        private TilemapManager selectedPreviewTilemapManager {
            get { return objectSelector.GetPreviewTilemapManager(typeSelector.GetSelectedTilemapType()); }
        }

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

        // 地图数据
        private BoardEntity boardEntity;

        private void Start() {
            boardEntity = new BoardEntity();
            boardEntity.mapName = "Untitled";
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
                typeSelector.ShiftSelectedTilemapType(reversed ? -1 : 1);
                UpdatePreview(true);
            }

            // 键盘Space/S-Space切换块
            if (Input.GetKeyUp(KeyCode.Space)) {
                bool reversed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                typeSelector.ShiftSelectedTileType(reversed ? -1 : 1);
                UpdatePreview(true);
            }

            // 键盘S存档
            if (Input.GetKeyUp(KeyCode.S)) {
                SaveMap();
            }

            // 键盘L读档
            if (Input.GetKeyUp(KeyCode.L)) {
                LoadMap();
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
                    selectedTilemapManager.EraseTile(selectedCell);
                    newPortal.Destroy();
                }
                // 否则，完成传送通道（不用清除终点位置的其他块）
                else {
                    portals.Add(newPortal.from, newPortal);
                }

                editorState = EditorState.Normal;
            }
            // 如果在画传送站起点
            else if (selectedTileType == TileType.Special_Portal) {
                EraseTile();
                // 画上传送站起点
                selectedTilemapManager.SetTile(selectedCell, selectedTileType);
                // 创建传送通道预览
                newPortal = portalPainter.Draw(selectedCell, selectedCell);
                editorState = EditorState.Portal;
            }
            // 一般情况下
            else {
                EraseTile();
                // 直接把块放上去
                selectedTilemapManager.SetTile(selectedCell, selectedTileType);
            }
        }

        // 将选定坐标上的块给清除掉
        void EraseTile() {
            // 如果即将被清除的块是某个传送站的起点，那么破坏掉它所对应的传送通道
            if (selectedTilemapManager.GetTile(selectedCell) == TileType.Special_Portal) {
                portals[selectedCell].Destroy();
                portals.Remove(selectedCell);
            }

            // 清除目标块
            selectedTilemapManager.EraseTile(selectedCell);
        }

        // 更新预览
        void UpdatePreview(bool force = false) {
            // 只要鼠标没动，不必刷新，就尽量不刷新
            if (!force && lastSelectedCell == selectedCell) return;

            // 预览手上的块
            foreach (TilemapManager tilemapManager in objectSelector.GetPreviewTilemapManagers())
                tilemapManager.EraseTile(lastSelectedCell);
            selectedPreviewTilemapManager.SetTile(selectedCell, selectedTileType);

            // 预览正在构建的传送通道
            if (editorState == EditorState.Portal) {
                newPortal.to = selectedCell;
            }
        }

        // 保存地图
        void SaveMap() {
            Debug.Log("Saving...");

            boardEntity.map = new List<SingleMapGridEntity>();
            foreach (Vector2Int cell in objectSelector.land.cells)
                boardEntity.map.Add(new SingleMapGridEntity(cell.x, cell.y));

            boardEntity.special = new List<SingleSpecialEntity>();
            foreach (Vector2Int cell in objectSelector.special.cells) {
                TileType type = objectSelector.special.GetTile(cell);
                if (type == TileType.Special_Portal) continue;
                boardEntity.special.Add(new SingleSpecialEntity(
                    cell.x, cell.y, Constants.specialNameOfTileType[type]
                ));
            }

            boardEntity.portal = new List<SinglePortalEntity>();
            foreach (Portal portal in portals.Values) {
                boardEntity.portal.Add(new SinglePortalEntity(
                    portal.from.x, portal.from.y,
                    portal.to.x, portal.to.y
                ));
            }

            boardEntity.tokens = new List<TokenEntity>();
            HashSet<int> players = new HashSet<int>();
            foreach (Vector2Int cell in objectSelector.token.cells) {
                int playerId = Constants.playerIdOfTileType[objectSelector.token.GetTile(cell)];
                players.Add(playerId);
                boardEntity.tokens.Add(new TokenEntity(cell.x, cell.y, playerId));
            }

            boardEntity.player = new PlayersEntity(Math.Min(2, players.Count), players.Count);
            string serialized = boardEntity.ToJson();
            string filename = boardEntity.mapName + ".json";
            System.IO.File.WriteAllText(filename, serialized, Encoding.UTF8);
            Debug.Log("Saved as " + filename);
        }

        // 加载地图
        void LoadMap() {
            Debug.Log("Loading...");
            string filename = "MapToLoad";
            string serialized;
            TextAsset text = Resources.Load<TextAsset>(filename);
            serialized = text.text;
            boardEntity = BoardEntity.FromJson(serialized);
            foreach (SingleMapGridEntity grid in boardEntity.map)
                objectSelector.land.SetTile(new Vector2Int(grid.x, grid.y), TileType.Land_Lawn_Green);
            foreach (SinglePortalEntity portal in boardEntity.portal) {
                newPortal = portalPainter.Draw(
                    new Vector2Int(portal.fromX, portal.fromY),
                    new Vector2Int(portal.toX, portal.toY)
                );
                portals.Add(newPortal.from, newPortal);
                objectSelector.special.SetTile(newPortal.from, TileType.Special_Portal);
            }

            foreach (SingleSpecialEntity special in boardEntity.special) {
                objectSelector.special.SetTile(
                    new Vector2Int(special.x, special.y),
                    Constants.tileTypeOfSpecialName[special.effect]
                );
            }

            foreach (TokenEntity token in boardEntity.tokens) {
                objectSelector.token.SetTile(
                    new Vector2Int(token.x, token.y),
                    token.player == 1 ? TileType.Token_Tank_Blue : TileType.Token_Tank_Red
                );
            }

            Debug.Log(boardEntity.mapName + " is loaded");
        }
    }
}