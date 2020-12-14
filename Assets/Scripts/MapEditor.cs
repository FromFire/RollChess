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
    bool inside(int x,int l,int r){return l<=x && x<=r;}
    string join(string [] stringArray){
        return string.Join("\n",stringArray)+"\n";
    }

    // Tile types and prefab resources
    enum TileType {
        Begin, Land_Begin,
            Land_Lawn_Green,
        Land_End, Special_Begin,
            Special_DoubleStep, Special_BrokenBridge,
        Special_End, Token_Begin,
            Token_Tank_Blue, Token_Tank_Red,
        Token_End, End
    };

    const int iTileTypeHead=(int)TileType.Begin+1;
    const int iTileTypeTail=(int)TileType.End-1;
    const int iTileTypeLandHead=(int)TileType.Land_Begin+1;
    const int iTileTypeLandTail=(int)TileType.Land_End-1;
    const int iTileTypeSpecialHead=(int)TileType.Special_Begin+1;
    const int iTileTypeSpecialTail=(int)TileType.Special_End-1;
    const int iTileTypeTokenHead=(int)TileType.Token_Begin+1;
    const int iTileTypeTokenTail=(int)TileType.Token_End-1;
    const int nTileType=iTileTypeTail-iTileTypeHead+1;
    const int nTileTypeLand=iTileTypeLandTail-iTileTypeLandHead+1;
    const int nTileTypeSpecial=iTileTypeSpecialTail-iTileTypeSpecialHead+1;
    const int nTileTypeToken=iTileTypeTokenTail-iTileTypeTokenHead+1;

    TileType shiftTileType(TileType tileType,int offset=1){
        int iTileType=(int)tileType;
        if(whichTilemapType(tileType)==TilemapType.Land)
            iTileType=iTileTypeLandHead+mod(iTileType-iTileTypeLandHead+offset,nTileTypeLand);
        else if(whichTilemapType(tileType)==TilemapType.Special)
            iTileType=iTileTypeSpecialHead+mod(iTileType-iTileTypeSpecialHead+offset,nTileTypeSpecial);
        else
            iTileType=iTileTypeTokenHead+mod(iTileType-iTileTypeTokenHead+offset,nTileTypeToken);
        return (TileType)iTileType;
    }

    enum TilemapType { Land, Special, Token };
    const int nTilemapType=3;
    TilemapType whichTilemapType(TileType tileType)
    {
        int iTileType=(int)tileType;
        if(inside(iTileType,iTileTypeLandHead,iTileTypeLandTail))
            return TilemapType.Land;
        else if(inside(iTileType,iTileTypeSpecialHead,iTileTypeSpecialTail))
            return TilemapType.Special;
        else
            return TilemapType.Token;
    }

    string[] pathTiles ={
        "","",
            "Tiles/floor-lawnGreen",
        "","",
            "Tiles/special-doubleStep", "Tiles/special-brokenBridge",
        "","",
            "Tiles/token-blueTank", "Tiles/token-redTank",
        "",""
    };
    List<TileBase> prefabTiles = new List<TileBase>();

    Dictionary<string,TileType> TileType_ByName;
    TileType whichTileType(TileBase tile){
        return TileType_ByName[tile.name];
    }

    // Game viewport
    Camera mainCamera;

    // Tilemaps
    Tilemap tilemapLand = null;
    Tilemap tilemapLandPreview = null;
    Tilemap tilemapSpecial = null;
    Tilemap tilemapSpecialPreview = null;
    Tilemap tilemapToken = null;
    Tilemap tilemapTokenPreview = null;
    Tilemap[] tilemaps = null;
    Tilemap[] tilemapsPreview = null;

    // Cells
    Cell nullCell = new Cell(0, 0, -1);
    TileBase nullTile;

    // Selection related and preview related
    Cell lastCell;
    TilemapType selectedTilemapType = TilemapType.Land;
    TileType[] selectedTileTypes = { TileType.Land_Lawn_Green, TileType.Special_BrokenBridge, TileType.Token_Tank_Blue };
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
        nullTile=Tile.CreateInstance<Tile>();
        // nullTile=new Tile();
        nullTile.name="";
        TileType_ByName=new Dictionary<string, TileType>();
        for(int iTileType=0;iTileType<=iTileTypeTail;iTileType++){
            string path=pathTiles[iTileType];
            TileBase tile=path.Length>0 ? Resources.Load<TileBase>(path) : nullTile;
            prefabTiles.Add(tile);
            if(TileType_ByName.ContainsKey(tile.name)) continue;
            TileType_ByName.Add(tile.name,(TileType)iTileType);
        }

        mainCamera = Camera.main;

        tilemapLand = GameObject.Find("/Grid/TilemapLand").GetComponent<Tilemap>();
        tilemapLandPreview = GameObject.Find("/Grid/TilemapLandPreview").GetComponent<Tilemap>();
        tilemapSpecial = GameObject.Find("/Grid/TilemapSpecial").GetComponent<Tilemap>();
        tilemapSpecialPreview = GameObject.Find("/Grid/TilemapSpecialPreview").GetComponent<Tilemap>();
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        tilemapTokenPreview = GameObject.Find("/Grid/TilemapTokenPreview").GetComponent<Tilemap>();
        tilemaps = new Tilemap[] { tilemapLand, tilemapSpecial, tilemapToken };
        tilemapsPreview = new Tilemap[] { tilemapLandPreview, tilemapSpecialPreview, tilemapTokenPreview };

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
        eraseTile(tilemapSpecialPreview, cell);
        eraseTile(tilemapLandPreview, cell);
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
        return tilemapLand.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
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
        (cells,tileTypes)=getTiles(tilemapLand);
        int nLand=0,nSpecial=0;
        for(int i=0;i<cells.Count;i++){
            if(tileTypes[i]==TileType.Land_Lawn_Green)
                nLand++;
            else
                nSpecial++;
        }
        for(int i=0;i<cells.Count;i++) if(tileTypes[i]==TileType.Land_Lawn_Green){
            nLand--;
            x=cells[i].x;
            y=cells[i].y;
            save+="       {\"x\":"+x+", \"y\":"+y+(nLand==0?"}\n":"},\n");
        }
        save+=join(new string []{
            "   ],",
            "   \"special\": [",
        });
        for(int i=0;i<cells.Count;i++) if(tileTypes[i]!=TileType.Land_Lawn_Green){
            nSpecial--;
            x=cells[i].x;
            y=cells[i].y;
            save+="       {\"x\":"+x+", \"y\":"+y+", \"effect\":\""
                    +(tileTypes[i]==TileType.Special_BrokenBridge?"brokenBridge":"doubleStep")+"\"}"
                    +(nSpecial==0?"\n":",\n");
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
