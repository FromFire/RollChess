using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Widget;

// 地图类
// 加载地图、管理地图状态
public class BoardOld : MonoBehaviour{
    //棋盘的显示
    public BoardDisplay boardDisplay;

    // 六边形网格显示
    public HexGridDisplay hexGridDisplay;

    // 不可走格子显示
    public UnwalkableDisplay unwalkableDisplay;

    //初始化
    public void Init() {
        //显示地图
        boardDisplay.Display(map);

        //显示不可走格子
        unwalkableDisplay.map = map;
        unwalkableDisplay.Display();

        //显示六边形网格
        hexGridDisplay.map = map;
        hexGridDisplay.Visible = true;
    }

    //检测路上的危桥并移除危桥
    public void DetectBrokenBridge(List<Vector2Int> route) {
        foreach(Vector2Int grid in route) {
            if(GetEffect(grid) == SingleGrid.Effect.BrokenBridge) {
                map.GetData(grid).SpecialEffect = SingleGrid.Effect.None;
                map.GetData(grid).walkable = false;
                boardDisplay.RemoveGrid(grid);
            }
        }
    }
}


