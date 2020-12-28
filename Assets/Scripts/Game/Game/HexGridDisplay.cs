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
    bool showHexGrid;

    // Start is called before the first frame update
    void Start()
    {
        ShowHexGrid = true;
    }

    // 显示和隐藏网格
    public bool ShowHexGrid {
        get { return showHexGrid; }
        set {
            Tile tileToShow = value ? tile : null;
            for(int i=-20; i<20; i++) {
                for(int j=-20; j<20; j++) {
                    tilemap.SetTile(new Vector3Int(i,j,0), tileToShow);
                }
            }
            showHexGrid = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
