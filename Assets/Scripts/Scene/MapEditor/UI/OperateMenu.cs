using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 一级操作菜单 </para>
/// </summary>
public class OperateMenu : MonoBehaviour {
    void Start() {}
    public void Undo() {
        MapEditResource.momentoController.Undo();
    }
    public void Redo() {
        MapEditResource.momentoController.Redo();
    }
}