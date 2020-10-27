using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDisplay : MonoBehaviour
{
    Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void display(List<MapEntity> map) {
        tilemap = GameObject.Find("/Grid/Tilemap").GetComponent<Tilemap>();
        Tile tile = Resources.Load<Tile>("Tiles/hex-sliced_114");
        foreach(MapEntity grid in map) {
            tilemap.SetTile(new Vector3Int(grid.x, grid.y, 0), tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
