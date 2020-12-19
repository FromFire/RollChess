using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// using Cell = UnityEngine.Vector3Int;

public class Cursor : MonoBehaviour
{
    public Camera mainCamera;
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 获取当前鼠标指向的块
    Vector2Int getPointedCell()
    {
        Vector3Int cell=tilemap.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        return new Vector2Int(cell.x,cell.y);
    }
}
