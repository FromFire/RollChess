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
        tilemap = GameObject.Find("/Grid/Tilemap").GetComponent<Tilemap>();
        Tile tile = Resources.Load<Tile>("Tiles/hex-sliced_114");
        Debug.Assert(tilemap != null);
        Debug.Assert(tile != null);
       // tile.color = Color.yellow;
        tilemap.SetTile(new Vector3Int(0,0,0), tile);
        tilemap.SetTile(new Vector3Int(0,1,1), tile);
        tilemap.SetTile(new Vector3Int(0,5,5), tile);
        tilemap.SetTile(new Vector3Int(0,-1,-1), tile);
        tilemap.SetTile(new Vector3Int(0,1,0), tile);
        tilemap.SetTile(new Vector3Int(0,0,1), tile);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
