using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGridDisplay : MonoBehaviour
{
    // 显示网格的Tilemap
    public Tilemap tilemap;

    // 网格tile
    public Tile tile;

    // 是否显示网格
    bool visible;

    // 地图数据，用来读取边界
    public BaseBoard<SingleGrid> map;

    // 已填充的地图边界
    int borderUp, borderDown, borderLeft, borderRight;

    // Start is called before the first frame update
    void Start()
    {

    }

    // 显示和隐藏网格
    public bool Visible {
        get { return visible; }
        set {
            if(value) {
                ShowGrid();
            }
            else {
                HideGrid();
            }
            visible = value;
        }
    }

    // 显示格子
    void ShowGrid() {
        // 记录填充的边界，余量20
        borderUp = map.BorderUp + 20;
        borderDown = map.BorderDown - 20;
        borderLeft = map.BorderLeft - 20;
        borderRight = map.BorderRight + 20;

        // 填充
        for(int i=borderLeft; i<borderRight; i++) {
            for(int j=borderDown; j<borderUp; j++) {
                tilemap.SetTile(new Vector3Int(i,j,0), tile);
            }
        }
    }

    // 隐藏格子
    void HideGrid() {
        // 填充
        for(int i=borderLeft; i<borderRight; i++) {
            for(int j=borderDown; j<borderUp; j++) {
                tilemap.SetTile(new Vector3Int(i,j,0), null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
