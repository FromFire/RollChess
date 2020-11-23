using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 特效显示
public class SpecialEffectDisplay : MonoBehaviour
{
    Tilemap tilemapSpecialEffect;

    //存储各类Tile的集合
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{floorSteelBlue, floorYellow};

    //供外界传入的颜色参数类
    public enum Color{blue, yellow};

    //正在高亮的格子坐标
    List<Vector3Int> highlightGridPosition;

    // Start is called before the first frame update
    void Start()
    {
        //初始化tilemapTool
        tilemapSpecialEffect = GameObject.Find("/Grid/TilemapSpecialEffect").GetComponent<Tilemap>();

        //初始化空的高亮列表
        highlightGridPosition = new List<Vector3Int>();

        //初始化tileList
        //tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/floor-steelBlue",    //floorSteelBlue
            "Tiles/floor-yellow"        //floorYellow
        };

        // 读取所有tile
        tileList = new List<Tile> ();
        foreach(string name in tileNames) {
            tileList.Add(Resources.Load<Tile>(name));
        }
    }

    // 输入坐标，输出格子的坐标
    public Vector2Int worldToCell(Vector3 loc) {
        Vector3Int vector = tilemapSpecialEffect.WorldToCell(loc);
        return new Vector2Int(vector.x, vector.y);
    }

    // 高亮格子
    public void highlightGrid(Vector2Int pos, Color color) {
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
        tilemapSpecialEffect.SetTile(pos3, highlightTile);
    }

    //取消所有高亮
    public void cancelHighlight() {
        for(int i=0; i<highlightGridPosition.ToArray().Length; i++) {
            tilemapSpecialEffect.SetTile(highlightGridPosition[i], null);
        }
        highlightGridPosition.Clear();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
