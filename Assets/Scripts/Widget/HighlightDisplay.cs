using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

// 特效显示
public class HighlightDisplay : MonoBehaviour
{
    //高亮自身格子和可到达格子
    public Tilemap tilemapReachableHighlight;
    //高亮路径
    public Tilemap tilemapRouteHighlight;


    //存储各类Tile的集合
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{floorSteelBlue, floorYellow};

    //供外界传入的颜色参数类
    public enum Color{blue, yellow};

    //正在高亮的格子坐标（Reachable层）
    List<Vector3Int> highlightGridPosition;
    //正在高亮的路径（Route层）
    List<Vector3Int> highlightRoute;
    //正在高亮的路径的终点
    [HideInInspector]
    public Vector3Int highlightRouteEnd;

    // Start is called before the first frame update
    void Start()
    {
        //初始化空的高亮列表
        highlightGridPosition = new List<Vector3Int>();
        highlightRoute = new List<Vector3Int>();
        highlightRouteEnd = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);

        //初始化tileList
        //tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/highlight-blue",
            "Tiles/highlight-red"
        };

        // 读取所有tile
        tileList = new List<Tile> ();
        foreach(string name in tileNames) {
            tileList.Add(Resources.Load<Tile>(name));
        }
    }

    // 输入坐标，输出格子的坐标
    public Vector2Int WorldToCell(Vector3 loc) {
        Vector3Int vector = tilemapReachableHighlight.WorldToCell(loc);
        return new Vector2Int(vector.x, vector.y);
    }

    // 高亮格子
    public void HighlightGrid(Vector2Int pos, Color color) {
        Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);
        highlightGridPosition.Add(pos3);

        //将格子显示为指定的高亮色
        Tile highlightTile = tileList[(int)TileKeys.floorSteelBlue];
        switch(color) {
            case Color.blue:
                highlightTile = tileList[(int)TileKeys.floorSteelBlue];
                break;
            case Color.yellow:
                highlightTile = tileList[(int)TileKeys.floorYellow];
                break;
        }
        tilemapReachableHighlight.SetTile(pos3, highlightTile);
    }

    //取消所有高亮
    public void CancelHighlight() {
        for(int i=0; i<highlightGridPosition.ToArray().Length; i++) {
            tilemapReachableHighlight.SetTile(highlightGridPosition[i], null);
        }
        highlightGridPosition.Clear();
    }

    //高亮一条路径
    public void HighlightRoute(List<Vector2Int> route) {
        foreach(Vector2Int pos in route) {
            Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);
            highlightRoute.Add(pos3);
            Tile highlightTile = tileList[(int)TileKeys.floorSteelBlue];
            tilemapRouteHighlight.SetTile(pos3, highlightTile);
        }
        highlightRouteEnd = new Vector3Int(route.Last().x, route.Last().y, 0);
    }

    //取消高亮路径
    public void CancelRouteHightlight() {
        foreach(Vector2Int pos in highlightRoute) {
            Vector3Int pos3 = new Vector3Int(pos.x, pos.y, 0);
            tilemapRouteHighlight.SetTile(pos3, null);
        }
        highlightRouteEnd = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        highlightRoute.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
