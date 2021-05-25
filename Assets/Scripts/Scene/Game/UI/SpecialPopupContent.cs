using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 弹窗内容，用于介绍特殊块的功能 </para>
/// </summary>
public class SpecialPopupContent : MonoBehaviour {

    // 块的名称
    public Text title;
    // 块的描述
    public Text describe;
    // 块的效果
    public Text effectIntro;

    /// <summary>
    ///   <para> 特殊块在游戏中的名称，要显示 </para>
    ///   <para> 例如：危桥 </para>
    /// </summary>
    public string Title {
        get { return title.text; }
        set { title.text = value; }
    }

    /// <summary>
    ///   <para> 特殊块的设定介绍，要显示 </para>
    ///   <para> 例如：年久失修的危桥，小心不要掉下去了。 </para>
    /// </summary>
    public string Describe {
        get { return describe.text; }
        set { describe.text = value; }
    }

    /// <summary>
    ///   <para> 特殊块的特殊效果介绍，要显示 </para>
    ///   <para> 例如：棋子通过危桥格一次后，该格变为不可通过。 </para>
    /// </summary>
    public string EffectIntro {
        get { return effectIntro.text; }
        set { effectIntro.text = value; }
    }
}