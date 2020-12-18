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
            Special_Portal, Special_DoubleStep, Special_BrokenBridge,
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
            "Tiles/special-portal", "Tiles/special-doubleStep", "Tiles/special-brokenBridge",
        "","",
            "Tiles/token-blueTank", "Tiles/token-redTank",
        "",""
    };
    List<TileBase> prefabTiles = new List<TileBase>();

    Dictionary<string,TileType> TileType_ByName;
    Dictionary<TileType,string> SpecialName_ByTileType=new Dictionary<TileType, string>{
        {TileType.Special_BrokenBridge,"brokenBridge"},
        {TileType.Special_DoubleStep,"doubleStep"},
    };
    Dictionary<string,TileType> TileType_BySpecialName=new Dictionary<string, TileType>();
    TileType whichTileType(TileBase tile){
        if(tile is null) return TileType.End;
        return TileType_ByName[tile.name];
    }

    // Game viewport
    Camera mainCamera;

    // Tilemaps
    Tilemap tilemapLand = null;
    Tilemap tilemapLandPreview = null;
    Tilemap tilemapSpecial = null;
    Tilemap tilemapSpecialPreview = null;
    Tilemap tilemapPortal = null;
    Tilemap tilemapPortalPreview = null;
    Tilemap tilemapToken = null;
    Tilemap tilemapTokenPreview = null;
    Tilemap[] tilemaps = null;
    Tilemap[] tilemapsPreview = null;

    // Cells
    Cell nullCell = new Cell(0, 0, -1);
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
    TileType selectedTileType{
        get{return selectedTileTypes[(int)selectedTilemapType];}
        set{selectedTileTypes[(int)selectedTilemapType]=value;}
    }
    TileType lastPaintedTileType;
    Tilemap selectedTilemap{
        get{return tilemaps[(int)selectedTilemapType];}
    }
    Tilemap lastPaintedTilemap;
    Tilemap selectedTilemapPreview{
        get{return tilemapsPreview[(int)selectedTilemapType];}
    }

    // Portal related
    bool buildingPortal=false;
    class Line{
        LineRenderer mRenderer;
        GameObject mObject;
        public static GameObject parent;
        Tilemap mTilemap;
        Cell mFrom;
        Cell mTo;
        public Cell from{
            get{return mFrom;}
            set{mFrom=value;SetPosition(0,value);}
        }
        public Cell to{
            get{return mTo;}
            set{mTo=value;SetPosition(1,value);}
        }
        public Line(Tilemap tilemap){
            // mObject=parent.gameObject("line");
            mObject=new GameObject("line");
            // mObject=(GameObject)Instantiate(GameObject.Find("Grid/TilemapPortal/SamplePortal"));
            mObject.AddComponent<LineRenderer>();
            mRenderer=mObject.GetComponent<LineRenderer>();
            mRenderer.startWidth=(float)0.1;
            mRenderer.endWidth=(float)0.1;
            mRenderer.material=new Material(Shader.Find("Sprites/Default"));
            mRenderer.startColor=Color.cyan;
            mRenderer.endColor=Color.white;
            mRenderer.sortingLayerID=SortingLayer.NameToID("PortalArrows");
            mTilemap=tilemap;
        }
        public void SetPosition(int idx,Cell cell){
            mRenderer.SetPosition(idx,mTilemap.CellToLocal(cell));
        }
        public void SetTilemap(Tilemap tilemap){
            mTilemap=tilemap;
            from=from;
            to=to;
        }
        public void Destroy(){
            GameObject.Destroy(mObject);
        }
    }
    List<Line> lines;
    Line newLine;


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
        foreach(KeyValuePair<TileType,string> pair in SpecialName_ByTileType)
            TileType_BySpecialName.Add(pair.Value,pair.Key);

        mainCamera = Camera.main;

        tilemapLand = GameObject.Find("/Grid/TilemapLand").GetComponent<Tilemap>();
        tilemapLandPreview = GameObject.Find("/Grid/TilemapLandPreview").GetComponent<Tilemap>();
        tilemapSpecial = GameObject.Find("/Grid/TilemapSpecial").GetComponent<Tilemap>();
        tilemapSpecialPreview = GameObject.Find("/Grid/TilemapSpecialPreview").GetComponent<Tilemap>();
        tilemapPortal = GameObject.Find("/Grid/TilemapPortal").GetComponent<Tilemap>();
        tilemapPortalPreview = GameObject.Find("/Grid/TilemapPortalPreview").GetComponent<Tilemap>();
        Line.parent=GameObject.Find("/Grid/TilemapPortal");
        tilemapToken = GameObject.Find("/Grid/TilemapToken").GetComponent<Tilemap>();
        tilemapTokenPreview = GameObject.Find("/Grid/TilemapTokenPreview").GetComponent<Tilemap>();
        tilemaps = new Tilemap[] { tilemapLand, tilemapSpecial, tilemapToken };
        tilemapsPreview = new Tilemap[] { tilemapLandPreview, tilemapSpecialPreview, tilemapTokenPreview };

        lastCell = nullCell;
        lastPaintedCell = nullCell;
        lastPaintedTileType = TileType.End;

        lines=new List<Line>();
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
            if(buildingPortal){
                if(cell!=newLine.from){
                    if(whichTileType(selectedTilemap.GetTile(cell))!=TileType.Special_Portal){
                        setTile(cell);
                        eraseTile(cell);
                    }
                    else
                        setTile(cell);
                    newLine.to=cell;
                    newLine.SetTilemap(tilemapPortal);
                    lines.Add(newLine);
                    buildingPortal=false;
                }
            }
            else if(lastPaintedCell!=cell
                || lastPaintedTileType!=selectedTileType
                || lastPaintedTilemap!=selectedTilemap){
                setTile(cell);
                if(selectedTileType==TileType.Special_Portal){
                    newLine=new Line(tilemapPortalPreview);
                    newLine.from=newLine.to=cell;
                    buildingPortal=true;
                }
            }
        }
        
        if(buildingPortal){
            newLine.to=cell;
        }

        if (Input.GetMouseButton(1)) {
            eraseTile(cell);
            if(selectedTileType==TileType.Special_Portal){
                for(int i=lines.Count-1;i>=0;i--) if(lines[i].from==cell){
                    lines[i].Destroy();
                    lines.RemoveAt(i);
                }
            }
        }

        // Shift painter
        if (Input.GetKeyUp(KeyCode.Tab)) {
            shiftSelectedTilemapType(
                (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))
                ? -1 : 1
            );
            updatePreview(cell);
        }
        // int offset=(int)Input.mouseScrollDelta[1];
        int offset=(Input.GetKeyUp(KeyCode.Space) ? 1 : 0);
        if(offset!=0){
            shiftSelectedTileType(offset);
            updatePreview(cell);
        }

        // Shortcuts
        if (Input.GetKeyUp(KeyCode.S)){// && (Input.GetKeyDown(KeyCode.LeftControl)||Input.GetKeyDown(KeyCode.RightControl))){
            // `[ [left|right]Ctrl + ] S`: save current map
            saveMap();
        }
        if (Input.GetKeyUp(KeyCode.L)){
            loadMap();
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
    void setTile(Tilemap tilemap, Cell cell, TileType tileType)
    {
        tilemap.SetTile(cell, prefabTiles[(int)tileType]);
        lastPaintedCell=cell;
        lastPaintedTileType=tileType;
        lastPaintedTilemap=tilemap;
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
        for(int i=0;i<cells.Count;i++) {
            x=cells[i].x;
            y=cells[i].y;
            save+="       {\"x\":"+x+", \"y\":"+y+(i==cells.Count-1?"}\n":"},\n");
        }
        save+=join(new string []{
            "   ],",
            "   \"special\": [",
        });
        (cells,tileTypes)=getTiles(tilemapSpecial);
        for(int i=cells.Count-1;i>=0;i--) if(tileTypes[i]==TileType.Special_Portal){
            cells.RemoveAt(i);
            tileTypes.RemoveAt(i);
        }
        for(int i=0;i<cells.Count;i++) {
            x=cells[i].x;
            y=cells[i].y;
            save+="       {\"x\":"+x+", \"y\":"+y+", \"effect\":\""
                    +SpecialName_ByTileType[tileTypes[i]]+"\"}"
                    +(i==cells.Count-1?"\n":",\n");
        }
        save+=join(new string []{
            "   ],",
            "   \"portal\": [",
        });
        for(int i=0;i<lines.Count;i++){
            Line line=lines[i];
            save+="       {\"fromX\":"+line.from.x+", "
                +"\"fromY\": "+line.from.y+", "
                +"\"toX\": "+line.to.x+", "
                +"\"toY\": "+line.to.y+"}"
                +(i==lines.Count-1?"\n":",\n");
        }
        save+=join(new string []{
            "   ]",
            "}"
        });
        string filename="savedMap.json";
        System.IO.File.WriteAllText(filename, save, Encoding.UTF8);
        Debug.Log("Saved as "+filename);
    }

    void loadMap(){
        string filename="MapSample";
        string json = "";
        TextAsset text = Resources.Load<TextAsset>(filename);
        json = text.text;
        BoardEntity boardEntity = JsonUtility.FromJson<BoardEntity>(json);
        foreach(SingleMapGridEntity cell in boardEntity.map)
            setTile(tilemapLand,new Cell(cell.x,cell.y,0),TileType.Land_Lawn_Green);
        foreach(SinglePortalEntity portal in boardEntity.portal){
            newLine=new Line(tilemapPortal);
            newLine.from=new Cell(portal.fromX,portal.fromY,0);
            newLine.to=new Cell(portal.toX,portal.toY,0);
            lines.Add(newLine);
            setTile(tilemapSpecial,newLine.from,TileType.Special_Portal);
            // setTile(tilemapSpecial,newLine.to,TileType.Special_Portal);
        }
        foreach(SingleSpecialEntity special in boardEntity.special){
            setTile(
                tilemapSpecial,new Cell(special.x,special.y,0),
                TileType_BySpecialName[special.effect]
            );
        }
        foreach(TokenEntity token in boardEntity.tokens){
            setTile(
                tilemapToken,new Cell(token.x,token.y,0),
                token.player==1?TileType.Token_Tank_Blue:TileType.Token_Tank_Red
            );
        }
    }
}
