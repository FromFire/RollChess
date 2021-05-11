using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 切换棋子笔刷 </para>
/// </summary>
public class TokenMenu : MonoBehaviour {

    // 切换玩家
    [SerializeField] private Button redButton;
    [SerializeField] private Button blueButton;
    [SerializeField] private Button yellowButton;
    [SerializeField] private Button greenButton;
    [SerializeField] private Button eraseButton;

    void Start() {}

    /// <summary>
    ///   <para> 切换玩家 </para>
    /// </summary>
    public void SwitchToRed() {
        MapEditResource.tokenEditor.Player = PlayerID.Red;
        MapEditResource.paintController.EditObject = MapEditObject.Token;
    }
    public void SwitchToBlue() {
        MapEditResource.tokenEditor.Player = PlayerID.Blue;
        MapEditResource.paintController.EditObject = MapEditObject.Token;
    }
    public void SwitchToYellow() {
        MapEditResource.tokenEditor.Player = PlayerID.Yellow;
        MapEditResource.paintController.EditObject = MapEditObject.Token;
    }
    public void SwitchToGreen() {
        MapEditResource.tokenEditor.Player = PlayerID.Green;
        MapEditResource.paintController.EditObject = MapEditObject.Token;
    }
    public void SwitchToErase() {
        MapEditResource.tokenEditor.Player = PlayerID.None;
        MapEditResource.paintController.EditObject = MapEditObject.Token;
    }
}