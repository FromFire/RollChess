using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using Widget;

public class MapEditor : MonoBehaviour {
    public Cursor cursor;
    public PortalPainter portalPainter;
    public Tilemap tilemapBoard;
    public Tilemap tilemapBoardPreview;
    public Tilemap tilemapSpecial;
    public Tilemap tilemapSpecialPreview;
    public Tilemap tilemapToken;
    public Tilemap tilemapTokenPreview;

    private BaseBoard<SingleGrid> map = new BaseBoard<SingleGrid>();
    private List<TokenEntity> tokens = new List<TokenEntity>();
    private PlayersEntity player = new PlayersEntity(0, 0);

    private enum Action {
        Nop,
        Move,
        SwitchLayer,
        SwitchTile,
        Paint,
        Erase,
        Save,
        Load
    }

    private enum State {
        Init,
        Idle,
        BuildingPortal,
        BuiltPortal
    }

    private State state=State.Init;

    private Vector2Int lastPos = Vector2Int.zero;
    private Vector3Int lastPos3 = Vector3Int.zero;

    private enum Layer {
        Board,
        Tokens
    }

    private Layer layer = Layer.Board;
    private BoardDisplay.TileKeys boardTileKey = BoardDisplay.TileKeys.floorLawnGreen;
    private List<Tile> boardTileList;
    private TokensDisplay.TileKeys tokensTileKey = TokensDisplay.TileKeys.tokenRedAlien;
    private List<Tile> tokensTileList;

    private List<Portal> portals = new List<Portal>();
    private Portal newPortal = null;

    // Start is called before the first frame update
    void Start() {
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
    }

    // Update is called once per frame
    void Update() {
        Queue<Action> actions = new Queue<Action>();

        actions.Enqueue(Action.Nop);
        Vector2Int pos = cursor.GetPointedCell();
        Vector3Int pos3 = (Vector3Int) pos;
        if (lastPos != pos)
            actions.Enqueue(Action.Move);
        if (Input.GetKeyUp(KeyCode.Tab))
            actions.Enqueue(Action.SwitchLayer);
        if (Input.GetKeyUp(KeyCode.Space))
            actions.Enqueue(Action.SwitchTile);
        if (Input.GetMouseButton(1))
            actions.Enqueue(Action.Erase);
        if ((state == State.Idle && Input.GetMouseButton(0))
            || (state == State.BuildingPortal && Input.GetMouseButtonDown(0))
            || (state == State.BuiltPortal && Input.GetMouseButtonUp(0)))
            actions.Enqueue(Action.Paint);
        if (Input.GetKeyUp(KeyCode.S))
            actions.Enqueue(Action.Save);
        if (Input.GetKeyUp(KeyCode.L))
            actions.Enqueue(Action.Load);


        while (actions.Count > 0) {
            Action action = actions.Dequeue();
            switch (state) {
                case State.Init:
                    switch (action) {
                        default:
                            state = State.Idle;
                            tilemapBoardPreview.SetTile(
                                pos3,
                                boardTileList[(int) BoardDisplay.TileKeys.floorLawnGreen]
                            );
                            break;
                    }

                    break;
                case State.Idle:
                    switch (action) {
                        case Action.Move:
                            switch (layer) {
                                case Layer.Board:
                                    tilemapBoardPreview.SetTile(pos3, tilemapBoardPreview.GetTile(lastPos3));
                                    tilemapBoardPreview.SetTile(lastPos3, null);
                                    tilemapSpecialPreview.SetTile(pos3, tilemapSpecialPreview.GetTile(lastPos3));
                                    tilemapSpecialPreview.SetTile(lastPos3, null);
                                    break;
                                case Layer.Tokens:
                                    tilemapTokenPreview.SetTile(pos3, tilemapTokenPreview.GetTile(lastPos3));
                                    tilemapTokenPreview.SetTile(lastPos3, null);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Action.SwitchLayer:
                            layer = (layer == Layer.Board ? Layer.Tokens : Layer.Board);
                            switch (layer) {
                                case Layer.Board:
                                    tilemapBoardPreview.SetTile(
                                        pos3,
                                        boardTileList[(int) BoardDisplay.TileKeys.floorLawnGreen]
                                    );
                                    tilemapSpecialPreview.SetTile(
                                        pos3,
                                        (
                                            boardTileKey == BoardDisplay.TileKeys.floorLawnGreen
                                                ? null
                                                : boardTileList[(int) boardTileKey]
                                        )
                                    );
                                    tilemapTokenPreview.SetTile(pos3, null);
                                    break;
                                case Layer.Tokens:
                                    tilemapBoardPreview.SetTile(pos3, null);
                                    tilemapSpecialPreview.SetTile(pos3, null);
                                    tilemapTokenPreview.SetTile(pos3, tokensTileList[(int) tokensTileKey]);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Action.SwitchTile:
                            switch (layer) {
                                case Layer.Board:
                                    if (boardTileKey == BoardDisplay.TileKeys.special_portal)
                                        boardTileKey = BoardDisplay.TileKeys.floorLawnGreen;
                                    else
                                        boardTileKey++;
                                    tilemapSpecialPreview.SetTile(
                                        pos3,
                                        (
                                            boardTileKey == BoardDisplay.TileKeys.floorLawnGreen
                                                ? null
                                                : boardTileList[(int) boardTileKey]
                                        )
                                    );
                                    break;
                                case Layer.Tokens:
                                    if (tokensTileKey == TokensDisplay.TileKeys.tokenNeutralAlien)
                                        tokensTileKey = TokensDisplay.TileKeys.tokenRedAlien;
                                    else
                                        tokensTileKey++;
                                    tilemapTokenPreview.SetTile(pos3, tokensTileList[(int) tokensTileKey]);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Action.Paint:
                            switch (layer) {
                                case Layer.Board:
                                    tilemapBoard.SetTile(
                                        pos3,
                                        boardTileList[(int) BoardDisplay.TileKeys.floorLawnGreen]
                                    );
                                    SingleGrid singleGrid = new SingleGrid(true);
                                    if (boardTileKey != BoardDisplay.TileKeys.floorLawnGreen) {
                                        tilemapSpecial.SetTile(pos3, boardTileList[(int) boardTileKey]);
                                        switch (boardTileKey) {
                                            case BoardDisplay.TileKeys.special_brokenBridge:
                                                singleGrid.SpecialEffect = SingleGrid.Effect.BrokenBridge;
                                                break;
                                            case BoardDisplay.TileKeys.special_doubleStep:
                                                singleGrid.SpecialEffect = SingleGrid.Effect.DoubleStep;
                                                break;
                                            case BoardDisplay.TileKeys.special_portal:
                                                singleGrid.SpecialEffect = SingleGrid.Effect.Portal;
                                                break;
                                            default:
                                                break;
                                        }
                                    }

                                    if (boardTileKey == BoardDisplay.TileKeys.special_portal) {
                                        newPortal = portalPainter.Draw(pos, pos);
                                        state = State.BuildingPortal;
                                    }

                                    map.Add(pos, singleGrid);

                                    break;
                                case Layer.Tokens:
                                    tilemapToken.SetTile(pos3, tokensTileList[(int) tokensTileKey]);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Action.Erase:
                            switch (layer) {
                                case Layer.Board:
                                    tilemapBoard.SetTile(pos3, null);
                                    tilemapSpecial.SetTile(pos3, null);
                                    break;
                                case Layer.Tokens:
                                    tilemapToken.SetTile(pos3, null);
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Action.Load:
                            Load("Untitled.json");
                            break;
                        case Action.Save:
                            Save("Untitled.json");
                            break;
                        default:
                            break;
                    }

                    break;
                case State.BuildingPortal:
                    switch (action) {
                        case Action.Move:
                            switch (layer) {
                                case Layer.Board:
                                    tilemapBoardPreview.SetTile(pos3, tilemapBoardPreview.GetTile(lastPos3));
                                    tilemapBoardPreview.SetTile(lastPos3, null);
                                    tilemapSpecialPreview.SetTile(pos3, tilemapSpecialPreview.GetTile(lastPos3));
                                    tilemapSpecialPreview.SetTile(lastPos3, null);
                                    break;
                                case Layer.Tokens:
                                    tilemapTokenPreview.SetTile(pos3, tilemapTokenPreview.GetTile(lastPos3));
                                    tilemapTokenPreview.SetTile(lastPos3, null);
                                    break;
                                default:
                                    break;
                            }

                            newPortal.to = pos;

                            break;
                        case Action.Paint:
                            if (pos == newPortal.from)
                                break;
                            tilemapBoard.SetTile(pos3, boardTileList[(int) BoardDisplay.TileKeys.floorLawnGreen]);
                            tilemapSpecial.SetTile(pos3, boardTileList[(int) BoardDisplay.TileKeys.special_portal]);
                            newPortal.to = pos;
                            portals.Add(newPortal);
                            state = State.BuiltPortal;
                            break;
                        case Action.Erase:
                            if (pos == newPortal.from) {
                                newPortal.Destroy();
                                newPortal = null;
                                state = State.BuiltPortal;
                            }

                            tilemapBoard.SetTile(pos3, null);
                            tilemapSpecial.SetTile(pos3, null);
                            break;
                        default:
                            break;
                    }

                    break;
                case State.BuiltPortal:
                    switch (action) {
                        case Action.Nop:
                            break;
                        default:
                            state = State.Idle;
                            break;
                    }

                    break;
                default:
                    break;
            }
        }

        lastPos = pos;
        lastPos3 = pos3;
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
        foreach (Vector2Int pos in map.ToPositionsSet()) {
            SingleGrid singleGrid = map.GetData(pos);
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
            map.Add(
                new Vector2Int(singleMapGridEntity.x, singleMapGridEntity.y),
                new SingleGrid(true)
            );
        }

        foreach (SingleSpecialEntity singleSpecialEntity in boardEntity.special) {
            SingleGrid singleGrid = map.GetData(
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
            SingleGrid singleGridFrom = map.GetData(
                new Vector2Int(singlePortalEntity.fromX, singlePortalEntity.fromY)
            );
            singleGridFrom.PortalTarget = new Vector2Int(singlePortalEntity.toX, singlePortalEntity.toY);
            singleGridFrom.SpecialEffect = SingleGrid.Effect.Portal;
        }
    }
}