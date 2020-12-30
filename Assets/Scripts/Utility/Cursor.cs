using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

// using Cell = UnityEngine.Vector3Int;

public class Cursor : MonoBehaviour {
    public Camera mainCamera;

    public Tilemap tilemap;
    private Vector3Int lastCell = Vector3Int.zero;
    private float duration = 0;
    private float maxDuration = 500;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        Vector3Int cell = tilemap.WorldToCell(mainCamera.ScreenToWorldPoint(GetMousePosition()));
        if ((cell - lastCell).magnitude <= 2.1) {
            if (duration < maxDuration)
                duration += Time.deltaTime;
        }
        else {
            duration = 0;
        }
        lastCell = cell;
    }

    // 判断鼠标当前是否位于UI上
    static public bool isOverUI() {
        return ( EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null );
    }

    // 获取当前鼠标指向的块
    public Vector2Int GetPointedCell() {
        return (Vector2Int) lastCell;
    }

    // 获取当前鼠标在视图中的位置
    public Vector2 GetPointedPosition() {
        return mainCamera.ScreenToWorldPoint(GetMousePosition());
    }

    // 获取当前鼠标的绝对坐标
    public Vector3 GetMousePosition() {
        return Input.mousePosition;
    }

    public float GetStayDuration() {
        return duration;
    }

    public void ResetStayDuration() {
        duration = 0;
    }
}