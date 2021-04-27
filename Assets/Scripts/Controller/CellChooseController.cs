using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 用户点击格子后，根据状态不同判断事件类型，分发事件 </para>
///   <para> 本类不直接修改数据！ </para>
/// </summary>
public class CellChooseController {
    // 高光tilemap层
    [SerializeField] private HighlightDisplay highlightDisplay;

    // 棋子选中情况
    // 是否已有棋子被选中
    bool isTokenChoosed = false;
    // 当前选中的棋子坐标
    Vector2Int choosedTokenPos;
    // 当前选中棋子可到达的位置，以及通向它的路线
    Dictionary<Vector2Int, List<Vector2Int>> route;

    /// <summary>
    ///   <para> 格子点击时触发，分发事件 </para>
    ///   <para> 需要调用外界处理的事件：走子、获取可达路径 </para>
    /// </summary>
    public void CellClicked(Vector3 loc) {
        // 获取点击的点在tilemap上的坐标
        Vector2Int pos = PublicResource.tilemapManager.WorldToCell(loc);
        Debug.Log("choose: ("+ pos.x + "." + pos.y + ")");

        // 1. 判定是否可走的格子
        //      否：取消所有选中，清除高亮，退出
        //      是：继续
        Board board = PublicResource.board;
        if(!board.Get(pos).Walkable) {
            ClearTokenChoose();
            return;
        }

        // 2. 判定是否是走子（已有棋子被选中，且此次点击的格子能走）
        //      是：移动棋子，退出
        //      否：继续
        if(isTokenChoosed && route.ContainsKey(pos)) {
            List<Vector2Int> moveRoute = route[pos];
            // 触发走子
            // Move(choosedTokenPos, pos, route);
            // 取消所有选中
            ClearTokenChoose();
            return;
        }

        // 3. 判定该格是否有己方棋子
        //      是：选中该棋子，预览可走位置（高亮它可以到达的所有格子）
        //      否：选中该格子
        // 这一步属于重新选择，故清空先前的选择信息
        ClearTokenChoose();
        // 查找此格的所有棋子
        Dictionary<TokenSet.QueryParam, int> param = new Dictionary<TokenSet.QueryParam, int> {
            {TokenSet.QueryParam.PositionX, pos.x},
            {TokenSet.QueryParam.PositionY, pos.y}
        };
        List<int> tokenId = PublicResource.tokenSet.Query(param);
        // 判断己方棋子
        if( !(tokenId is null) || tokenId.Count != 0 
            || PublicResource.tokenSet.GetToken(tokenId[0]).Player == PublicResource.gameState.nowPlayer) {
                // 选中该棋子，预览可走位置
                ChooseToken(pos);
        } else {
            // 选中格子
            highlightDisplay.HighlightToken(pos);
        }
    }

    // 取消选中棋子
    void ClearTokenChoose() {
        //维护选中信息
        isTokenChoosed = false;
        route.Clear();

        //取消所有高光
        highlightDisplay.CancelHighlight();
    }

    // 选中棋子
    void ChooseToken(Vector2Int pos) {
        //获取该棋子它所有可达位置
        Dictionary<Vector2Int, List<Vector2Int>> route = 
            PublicResource.boardAssistant.GetRoute(pos, PublicResource.gameState.rollResult);

        //维护选中信息
        isTokenChoosed = true;
        choosedTokenPos = pos;
        this.route = route;

        //显示高亮
        foreach(KeyValuePair<Vector2Int, List<Vector2Int>> kvp in route) {
            highlightDisplay.HighlightReachableCell(kvp.Key);
        }
    }
}