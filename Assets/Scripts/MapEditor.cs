using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

// Type definitions
using Cell = UnityEngine.Vector3Int;

public class MapEditor : MonoBehaviour
{
    // Utilities
    int mod(int x, int m) { return (x % m + m) % m; }
    string join(string [] stringArray){
        return string.Join("\n",stringArray)+"\n";
    }

    // Tile types and prefab resources
    enum TileType { BoardLand, TokenBlue, TokenRed };
    const int iTileTypeBoardHead=(int)TileType.BoardLand;
    const int iTileTypeBoardTail=(int)TileType.BoardLand;
    const int iTileTypeTokenHead=(int)TileType.TokenBlue;
    const int iTileTypeTokenTail=(int)TileType.TokenRed;
    const int nTileType=3;
    const int nTileTypeBoard=iTileTypeBoardTail-iTileTypeBoardHead+1;
    const int nTileTypeToken=iTileTypeTokenTail-iTileTypeTokenHead+1;
    TileType shiftTileType(TileType tileType,int offset=1){
        int iTileType=(int)tileType;
        if(whichTilemapType(tileType)==TilemapType.Board)
            iTileType=iTileTypeBoardHead+mod(iTileType-iTileTypeBoardHead+offset,nTileTypeBoard);
        else
            iTileType=iTileTypeTokenHead+mod(iTileType-iTileTypeTokenHead+offset,nTileTypeToken);
        return (TileType)iTileType;
    }
    enum TilemapType { Board, Token };
    const int nTilemapType=2;
    TilemapType whichTilemapType(TileType tileType)
    {
        if ((int)tileType <= iTileTypeBoardTail) return TilemapType.Board;
        else return TilemapType.Token;
    }
    string[] pathTiles ={
        "Tiles/floor-lawnGreen",
        "Tiles/token-blueTank",
        "Tiles/token-redTank"
    };
    List<TileBase> prefabTiles = new List<TileBase>();

    TileType whichTileType(TileBase tile){
        for(int i=0;i<nTileType;i++) if(tile.name==prefabTiles[i].name) return (TileType)i;
        return TileType.BoardLand;
    }

    // Game viewport
    Camera mainCamera;

    // Tilemaps
    Tilemap tilemapBoard = null;
    Tilemap tilemapBoardPreview = null;
    Tilemap tilemapToken = null;
    Tilemap tilemapTokenPreview = null;
    Tilemap[] tilemaps = null;
    Tilemap[] tilemapsPreview = null;

    // Cells
    Cell nullCell = new Cell(0, 0, -1);

    // Selection related and preview related
    Cell lastCell;
    TilemapType selectedTilemapType = TilemapType.Board;
    TileType[] selectedTileTypes = { TileType.BoardLand, TileType.TokenBlue };
    TileType selectedTileType{
        get{return selectedTileTypes[(int)selectedTilemapType];}
        set{selectedTileTypes[(int)selectedTilemapType]=value;}
    }
    Tilemap selectedTilemap{
        get{return tilemaps[(int)selectedTilemapType];}
    }
    Tilemap selectedTilemapPreview{
        get{return tilemapsPreview[(int)selectedTilemapType];}
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach (string path in pathTiles)
            prefabTiles.Add(Resources.Load<TileBase>(path));

        mainCamera = Camera.main;

        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        tilemapBoardPreview = GameObject.Find("/Grid/TilemapBoardPreview").GetComponent<Tilemap>();
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        tilemapTokenPreview = GameObject.Find("/Grid/TilemapTokenPreview").GetComponent<Tilemap>();
        tilemaps = new Tilemap[] { tilemapBoard, tilemapToken };
        tilemapsPreview = new Tilemap[] { tilemapBoardPreview, tilemapTokenPreview };

        // foreach(Tilemap tilemap in tilemaps){
        //     Cell l=tilemap.cellBounds.min;
        //     Cell r=tilemap.cellBounds.max;
        //     Debug.Log("tilemap: "+tilemap.name);
        //     Debug.Log("x: ["+l.x+","+r.x+"]");
        //     Debug.Log("y: ["+l.y+","+r.y+"]");
        //     Debug.Log("z: ["+l.z+","+r.z+"]");
        //     for(int x=l.x;x<r.x;x++) for(int y=l.y;y<r.y;y++) for(int z=l.z;z<r.z;z++){
        //         TileBase tile=tilemap.GetTile(new Cell(x,y,z));
        //         if(tile!=null){
        //             Debug.Log("x=" + x + ",y=" + y + ",z=" + z);
        //             Debug.Log(tile.name);
        //         }
        //     }
        // }

        lastCell = nullCell;
    }

    // Update is called once per frame
    void Update()
    {
        Cell cell = getPointedCell();

        // Preview
        if(cell!=lastCell)
            updatePreview(cell);

        // Paint
        if (Input.GetMouseButton(0))
        {
            setTile(cell);
        }
        if (Input.GetMouseButton(1))
        {
            eraseTile(cell);
        }

        // Shift painter
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            shiftSelectedTilemapType();
            updatePreview(cell);
        }
        int offset=(int)Input.mouseScrollDelta[1];
        if(offset!=0){
            shiftSelectedTileType(offset);
            updatePreview(cell);
        }

        // Shortcuts
        if (Input.GetKeyUp(KeyCode.S)){// && (Input.GetKeyDown(KeyCode.LeftControl)||Input.GetKeyDown(KeyCode.RightControl))){
            // `[ [left|right]Ctrl + ] S`: save current map
            saveMap();
        }
    }

    void updatePreview(Cell cell){
        eraseTilePreview(lastCell);
        setTilePreview(cell);
        lastCell=cell;
    }

    void shiftSelectedTilemapType(int offset = 1)
    {
        int newSelection = (int)selectedTilemapType + offset;
        newSelection = (newSelection % nTilemapType + nTilemapType) % nTilemapType;
        selectedTilemapType = (TilemapType)newSelection;
    }
    void shiftSelectedTileType(int offset=1){
        selectedTileType=shiftTileType(selectedTileType,offset);
    }

    void eraseTilePreview(Cell cell)
    {
        eraseTile(tilemapTokenPreview, cell);
        eraseTile(tilemapBoardPreview, cell);
    }
    bool eraseTile(Cell cell)
    {
        return eraseTile(selectedTilemap, cell);
    }
    bool eraseTile(Tilemap tilemap, Cell cell)
    {
        if (tilemap.GetTile(cell) == null) return false;
        tilemap.SetTile(cell, null);
        return true;
    }

    void setTilePreview(Cell cell)
    {
        setTile(selectedTilemapPreview, cell, selectedTileType);
    }
    void setTilePreview(Cell cell, TileType tileType)
    {
        setTile(selectedTilemapPreview, cell, tileType);
    }
    void setTile(Cell cell)
    {
        setTile(selectedTilemap,cell,selectedTileType);
    }
    void setTile(Cell cell, TileType tileType)
    {
        setTile(selectedTilemap, cell, tileType);
    }
    void setTile(Tilemap tilemap, Cell cell, TileType tiletype)
    {
        tilemap.SetTile(cell, prefabTiles[(int)tiletype]);
    }

    Cell getPointedCell()
    {
        return tilemapBoard.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
    }

    (List<Cell> cells,List<TileType> tileTypes) getTiles(Tilemap tilemap){
        (List<Cell> cells,List<TileType> tileTypes) ret=(new List<Cell>(),new List<TileType>());
        Vector3Int min=tilemap.cellBounds.min;
        Vector3Int max=tilemap.cellBounds.max;
        Cell cell=Cell.zero;
        for(cell.x=min.x;cell.x<max.x;cell.x++) for(cell.y=min.y;cell.y<max.y;cell.y++) for(cell.z=min.z;cell.z<max.z;cell.z++){
            TileBase tile=tilemap.GetTile(cell);
            if(tile==null) continue;
            ret.cells.Add(cell);
            ret.tileTypes.Add(whichTileType(tile));
        }
        return ret;
    }

    void saveMap(){
        Debug.Log("Saving...\nDo not shutdown game now.");
        string save="";
        save+=join(new string []{
            "{",
            "   \"mapName\":\"savedMap\",",
            "   \"player\": {",
            "       \"number\":2",
            "   },",
            "   \"tokens\": [",
        });
        List<Cell> cells=new List<Cell>();
        List<TileType> tileTypes=new List<TileType>();
        (cells,tileTypes)=getTiles(tilemapToken);
        int x,y,player;
        for(int i=0;i<cells.Count;i++){
            x=cells[i].x;
            y=cells[i].y;
            player=(int)tileTypes[i]-iTileTypeTokenHead;
            save+="       {\"x\":"+x+", \"y\":"+y+", \"player\":"+player+(i==cells.Count-1?"}\n":"},\n");
        }
        save+=join(new string []{
            "   ],",
            "   \"map\": [",
        });
        (cells,tileTypes)=getTiles(tilemapBoard);
        for(int i=0;i<cells.Count;i++){
            x=cells[i].x;
            y=cells[i].y;
            save+="       {\"x\":"+x+", \"y\":"+y+(i==cells.Count-1?"}\n":"},\n");
        }
        save+=join(new string []{
            "   ]",
            "}"
        });
        string filename="savedMap.json";
        System.IO.File.WriteAllText(filename, save, Encoding.UTF8);
        Debug.Log("Saved as "+filename);
    }
}
