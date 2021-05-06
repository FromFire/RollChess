using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 用户点击格子后，根据状态不同判断事件类型，分发事件 </para>
///   <para> 作为用户输入，只会主动调用其他类，不被其他类调用，也不直接修改数据。 </para>
/// </summary>
public class CellChooseController : MonoBehaviour {
    // 高光tilemap层
    [SerializeField] private HighlightDisplay highlightDisplay;

    // 棋子选中情况
    // 是否已有棋子被选中
    private bool isTokenChoosed = false;
    // 当前选中的棋子坐标
    private Vector2Int choosedTokenPos;
    // 当前选中棋子可到达的位置，以及通向它的路线
    private Dictionary<Vector2Int, List<Vector2Int>> route = new Dictionary<Vector2Int, List<Vector2Int>>();

    // 是否已有路径在高光中
    private bool isRouteHighlighted = false;
    // 当前高光的路径终点
    private Vector2Int highlightedRouteEnd;

    // 检测鼠标点击和停留
    void Update()
    {
        // 避免与UI按键冲突
        if (CursorMonitor.CursorIsOverUI())
            return;

        // 获取鼠标所在点的点在tilemap上的坐标
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector2Int pos = PublicResource.tilemapManager.WorldToCell(loc);

        // 若已有己方棋子被选中，则预览路线
        if(isTokenChoosed)
            PreviewRoute(pos);

        // 若鼠标左键有点击 + 游戏未结束 + 不是正在处理自己的操作
        // 则触发选中
        if(Input.GetMouseButtonDown(0) && PublicResource.gameState.Stage != GameStage.Game_Over 
            && PublicResource.gameState.Stage != GameStage.Self_Operation_Processing)
            CellClicked(pos);
    }

    /// <summary>
    ///   <para> 格子点击时触发，识别玩家操作权限，分发事件 </para>
    /// </summary>
    public void CellClicked(Vector2Int pos) {
        Debug.Log("choose: ("+ pos.x + "." + pos.y + ")");

        // 1. 判定是否可走的格子
        //      否：取消所有选中，清除高亮，退出
        //      是：继续
        Board board = PublicResource.board;
        if(!board.Contains(pos) || !board.Get(pos).Walkable) {
            ClearTokenChoose(); 
            return;
        }

        // 1.5 判断己方是否出于可走子，或可预览的状态（简称可操作）
        // 须满足条件：是本机控制的回合 + 已roll点
        bool opeartingAvailable = ( PublicResource.gameState.Stage == GameStage.Self_Operating 
            && PublicResource.gameState.RollResult != -1 );

        // 2. 判定是否是走子（已有棋子被选中 + 此次点击的格子能走 + 可操作）
        //      是：移动棋子，退出
        //      否：继续
        if(isTokenChoosed && route.ContainsKey(pos) && opeartingAvailable) {
            // 触发走子
            PublicResource.gameController.Move(choosedTokenPos, route[pos]);
            // 取消所有选中
            ClearTokenChoose();
            return;
        }

        // 2.5 以下所有情况都会重新选择格子，故清空先前的选择信息
        ClearTokenChoose();

        // 3. 判定是否选中己方棋子（点击的是己方棋子 + 可操作）
        //      是：选中该棋子，预览可走位置（高亮它可以到达的所有格子）
        //      否：继续
        // 查找此格的所有棋子
        Dictionary<TokenSet.QueryParam, int> param = new Dictionary<TokenSet.QueryParam, int> {
            {TokenSet.QueryParam.PositionX, pos.x},
            {TokenSet.QueryParam.PositionY, pos.y}
        };
        List<int> tokenId = PublicResource.tokenSet.Query(param);
        // 判断己方棋子 + 可操作
        if( !(tokenId is null) && tokenId.Count != 0 
            && PublicResource.tokenSet.GetToken(tokenId[0]).Player == PublicResource.gameState.NowPlayer
            && opeartingAvailable ) {
            // 选中该棋子，获取可走位置
            ChooseToken(pos);
        }

        // 4. 选中该格子
        highlightDisplay.HighlightToken(pos);
    }

    /// <summary>
    ///   <para> 判断和预览到此格的路线 </para>
    /// </summary>
    void PreviewRoute(Vector2Int pos) {
        // 若鼠标已在正高亮中的格子上，直接返回
        if(isRouteHighlighted && highlightedRouteEnd == pos)
            return;

        // 是否在可走的格子上，若在，高亮此路径
        if(route.ContainsKey(pos)) {
            highlightDisplay.HighlightRoute(route[pos]);
            // 维护数据
            isRouteHighlighted = true;
            highlightedRouteEnd = pos;
        }
        // 若不在，取消路径高亮
        else if(isRouteHighlighted){
            highlightDisplay.CancelRouteHighlight();
            isRouteHighlighted = false;
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
            PublicResource.boardAssistant.GetRoute(pos, PublicResource.gameState.RollResult);

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