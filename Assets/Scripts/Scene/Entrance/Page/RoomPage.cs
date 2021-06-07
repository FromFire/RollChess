using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomPage : MonoBehaviour {
    // 开始游戏按钮
    [SerializeField] private Button startGameButton;

    private void Start() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Player_Add, SetButtonEnable);
    }

    /// <summary>
    /// 开始多人游戏
    /// </summary>
    public void StartGame() {
        EntranceController.Get().StartMultipleGame();
    }

    // 禁用Client上的开始游戏按钮
    void SetButtonEnable() {
        if (Players.Get().LocalPlayer() is null) return;
        startGameButton.interactable = (Players.Get().LocalPlayer().isHost);
    }
}