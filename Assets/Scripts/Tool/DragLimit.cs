using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 地图范围拖拽控制 </para>
/// </summary>
public class DragLimit : MonoBehaviour {
    void Update() {
        // 获取屏幕中心的Tilemap坐标
        Vector3 screenCenterWorld = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2,Screen.height/2,0));
        Vector2Int screenCenter = PublicResource.tilemapManager.WorldToCell(screenCenterWorld);
        
        // 判定是否已超过地图边界，若越界，则不允许向那个方向继续滑动
        VisionController visionController = PublicResource.visionController;
        Board board = PublicResource.board;
        visionController.allowMoveLeft = screenCenter.x > board.BorderLeft;
        visionController.allowMoveRight = screenCenter.x < board.BorderRight;
        visionController.allowMoveUp = screenCenter.y < board.BorderUp;
        visionController.allowMoveDown = screenCenter.y > board.BorderDown;
    }
}