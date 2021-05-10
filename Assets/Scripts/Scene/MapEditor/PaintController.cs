using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 绘制器，接受用户输入，并将操作分发给绘制器 </para>
/// </summary>
public class PaintController : MonoBehaviour {

    // 当前绘制的类型
    private MapEditObject editObject;

    // 当前的绘制器
    private Paint paint;

    // 当前的块绘制器
    private BlockPaint blockPaint;

    /// <summary>
    ///   <para> 分析用户输入 </para>
    /// </summary>
    void Update() {

    }
}