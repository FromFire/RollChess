using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 显示棋子 </para>
/// </summary>
public class TokenDisplay : MonoBehaviour {
    // Tilemap棋子层
    [SerializeField] private TilemapManager tilemap;

    // // 数字的样式案例
    // [SerializeField] private GameObject numberSample;

    // // 数字所在父对象
    // [SerializeField] private GameObject canvas;

    // // 所有数字对象
    // private Dictionary<Vector2Int, Text> numberTexts = new Dictionary<Vector2Int, Text>();

    void Start() {
        Display();
        // 注册更新
        ModelResource.tokenSubject.Attach(ModelModifyEvent.Token, UpdateSelf);
    }

    /// <summary>
    ///   <para> 显示自身 </para>
    /// </summary>
    public void Display() {
        // 获取所有棋子
        List<int> tokenId = ModelResource.tokenSet.Query(null);
        // 按阵营显示棋子
        foreach (int id in tokenId) {
            Token token = ModelResource.tokenSet.Get(id);
            tilemap.SetTile(token.Position, Transform.tileTypeOfPlayerId[token.Player]);
        }
    }

    /// <summary>
    ///   <para> 响应TokenSet更新 </para>
    /// </summary>
    public void UpdateSelf(Vector2Int pos) {
        // 获取被修改处的所有棋子
        Dictionary<TokenSet.QueryParam, int> param = new Dictionary<TokenSet.QueryParam, int> {
            {TokenSet.QueryParam.PositionX, pos.x},
            {TokenSet.QueryParam.PositionY, pos.y}
        };
        List<int> tokenId = ModelResource.tokenSet.Query(param);

        // 若此处已无棋子，则清空此格
        if(tokenId is null || tokenId.Count == 0)
            tilemap.RemoveTile(pos);

        // 若此处有棋子，更新阵营
        else {
            // 由于一个位置上的棋子都是同一阵营的，所以用第一个为代表
            Token token = ModelResource.tokenSet.Get(tokenId[0]);
            // 重新显示此格
            tilemap.SetTile(token.Position, Transform.tileTypeOfPlayerId[token.Player]);
        }

        // // 若此处棋子数多于1个，在其上方显示数字
        // if(tokenId.Count > 1)
        //     ShowTokenNumber(pos, tokenId.Count);
        // else 
        //     RemoveTokenNumber(pos);
    }

    // // 在棋子头上显示数字
    // void ShowTokenNumber(Vector2Int pos, int number) {
    //     Text text;
    //     // 若此处没有数字，则创建新的
    //     if(!numberTexts.ContainsKey(pos)) {
    //         // 创建新数字
    //         GameObject obj = Object.Instantiate(numberSample);
    //         obj.transform.parent = canvas.transform;
    //         text = obj.GetComponent<Text>();
    //         numberTexts.Add(pos, text);

    //         // 设置显示位置
    //         Vector3 position = tilemap.CellToWorld(pos);
    //         Vector3 cellSize =  tilemap.CellSize();
    //         position += new Vector3(0, 1.6f * cellSize.y, 0);
    //         obj.transform.position = position;
    //     } 
    //     else
    //         text = numberTexts[pos];

    //     // 设置文本
    //     text.text = "" + number;
    // }

    // // 消除棋子头顶的数字
    // void RemoveTokenNumber(Vector2Int pos) {
    //     // 本来就无数字，不影响
    //     if(!numberTexts.ContainsKey(pos))
    //         return;
    //     // 消除
    //     Text text = numberTexts[pos];
    //     Destroy(text.gameObject);
    //     numberTexts.Remove(pos);
    // }
}