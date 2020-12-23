using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGridDisplay : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile tile;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=-20; i<20; i++) {
            for(int j=-20; j<20; j++) {
                tilemap.SetTile(new Vector3Int(i,j,0), tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
