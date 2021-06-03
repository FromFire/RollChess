using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 显示特殊格子浮窗介绍 </para>
/// </summary>
// todo：三个显示部分关系不大，考虑拆分
public class PopupController : MonoBehaviour {
    // 弹窗
    [SerializeField] private Popup popup;

    void Start() {
    }

    void Update() {
        // 当鼠标所在格子是特殊格子时，在画面左上角显示该格子的介绍
        Board board = Board.Get();
        Vector2Int pointedCell = ToolResource.tilemapManager.CursorPointingCell();
        // 避免空格子
        if(!board.Contains(pointedCell))
            return;
        // 获取内容，弹出Popup
        SpecialEffect pointedEffect = board.Get(pointedCell).Effect;
        if(pointedEffect != SpecialEffect.None) {
            // 设置SpecialPopupContent的内容
            SpecialIntroductionItem item = SpecialIntroduction.Get().introduction[pointedEffect];
            SpecialPopupContent content = popup.Content.GetComponent<SpecialPopupContent>();
            content.Title = item.introTitle;
            content.Describe = item.introText;
            content.EffectIntro = item.effectText;

            //设置PopUp显示
            popup.Show();
        } else {
            popup.Hide();
        }
    }
}