using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 选中笔刷时使之高亮，取消选中时使之恢复 </para>
/// </summary>
public class HighlightBrushButton : MonoBehaviour {
    // 当前高亮的按钮
    private int choose = -1;

    // 高亮颜色
    Color highlightColor;

    // 高亮颜色
    Color defaultColor;

    // 笔刷按钮
    [SerializeField] private List<Button> buttons;

    // 初始化
    void Start() {
        // 高亮颜色是浅绿色
        ColorUtility.TryParseHtmlString("#66FF55", out highlightColor);
        // 默认颜色是白色
        ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor);

        // 为所有按钮绑定点击事件
        for(int i=0; i<buttons.Count; i++) {
            int index = i;
            buttons[i].onClick.AddListener(()=>highlightButton(index));
        }

        // 开局默认选中陆地，即第1个
        highlightButton(0);
    }

    /// <summary>
    ///   <para> 选中笔刷时使之高亮，同时使之前的高亮按钮恢复 </para>
    /// </summary>
    void highlightButton(int i) {
        // 取消之前的高亮
        if(choose != -1)
            buttons[choose].gameObject.GetComponent<Image>().color = defaultColor;
        
        // 加新高亮
        buttons[i].gameObject.GetComponent<Image>().color = highlightColor;
        choose = i;
    }
}