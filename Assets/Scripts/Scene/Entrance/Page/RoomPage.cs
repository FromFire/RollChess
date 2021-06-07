using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomPage : MonoBehaviour {
    // 开始游戏按钮
    [SerializeField] private Button startGameButton;

    private void Start() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Player_Change, UpdateEnable);
    }

    public void StartGame() {
        EntranceController.Get().StartMultipleGame();
    }

    // 禁用Client上的开始游戏按钮
    private void UpdateEnable() {
            startGameButton.interactable = (Players.Get().LocalPlayer().isHost);
    }
}