using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnwalkableDisplay : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile tile;

    // 地图数据，用来读取边界
    public BaseBoard<SingleGrid> map;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // TODO：添加协调Board和Unwalkable的函数，例如Board移除格子后Unwalkable自动填充之

    public void Display() {
        for(int i=map.BorderLeft - 20; i<map.BorderRight + 20; i++) {
            for(int j=map.BorderDown - 20; j<map.BorderUp + 20; j++) {
                // 避免冲突
                if(tilemap.GetTile(new Vector3Int(i,j,0)) == null)
                    tilemap.SetTile(new Vector3Int(i,j,0), tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
