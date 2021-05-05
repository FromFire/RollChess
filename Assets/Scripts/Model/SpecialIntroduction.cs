using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 提供特殊格子的信息介绍 </para>
/// </summary>
public class SpecialIntroduction : MonoBehaviour {
    /// <summary>
    ///   <para> 特殊格子信息 </para>
    /// </summary>
    public readonly Dictionary<SpecialEffect, SpecialIntroductionItem> introduction 
        = new Dictionary<SpecialEffect, SpecialIntroductionItem>();

    /// <summary>
    ///   <para> 从文件中读取介绍，完成初始化 </para>
    /// </summary>
    void Start() {
        // 读取文件
        TextAsset text = Resources.Load<TextAsset>("Texts/SpecialIntroductions");
        string json = text.text;
        Debug.Log(json);
        SpecialIntroductionEntity specialIntroductionEntity = JsonUtility.FromJson<SpecialIntroductionEntity>(json);

        // 转换为introduction
        foreach(SpecialIntroductionItem item in specialIntroductionEntity.SpecialIntroductions) {
            introduction.Add(Transform.specialEffectOfName[item.name], item);
        }
    }
}

/// <summary>
///   <para> 特殊格子信息 </para>
/// </summary>
[System.Serializable]
public class SpecialIntroductionEntity
{
    public List <SpecialIntroductionItem> SpecialIntroductions;
}

/// <summary>
///   <para> 特殊格子信息·单个格子 </para>
/// </summary>
[System.Serializable]
public class SpecialIntroductionItem
{
    /// <summary>
    ///   <para> 特殊块在json中的名称，通过Transform.specialEffectOfName转换为SpecialType </para>
    ///   <para> 例如：brokenBridge </para>
    /// </summary>
    public string name;

    /// <summary>
    ///   <para> 特殊块在游戏中的名称，要显示 </para>
    ///   <para> 例如：危桥 </para>
    /// </summary>
    public string introTitle;

    /// <summary>
    ///   <para> 特殊块的设定介绍，要显示 </para>
    ///   <para> 例如：年久失修的危桥，小心不要掉下去了。 </para>
    /// </summary>
    public string introText;

    /// <summary>
    ///   <para> 特殊块的特殊效果介绍，要显示 </para>
    ///   <para> 例如：棋子通过危桥格一次后，该格变为不可通过。 </para>
    /// </summary>
    public string effectText;
}

