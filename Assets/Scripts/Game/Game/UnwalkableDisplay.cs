using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnwalkableDisplay : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile tile;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=-20; i<20; i++) {
            for(int j=-20; j<20; j++) {
                // 避免冲突
                if(tilemap.GetTile(new Vector3Int(i,j,0)) == null)
                    tilemap.SetTile(new Vector3Int(i,j,0), tile);
            }
        }
    }
    // TODO：添加协调Board和Unwalkable的函数，例如Board移除格子后Unwalkable自动填充之

    // Update is called once per frame
    void Update()
    {
        
    }
}
