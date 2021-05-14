using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class SaveResource : MonoBehaviour  {

    static public SaveManager saveManager;
    static public SaveLoader saveLoader;

    // 下面是用来实例化的

    // 绘制器
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private SaveLoader _saveLoader;

    // 实例化
    void Start() {
        saveManager = _saveManager;
        saveLoader = _saveLoader;
    }
}