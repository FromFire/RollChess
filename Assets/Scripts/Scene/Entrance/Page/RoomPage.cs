using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomPage : MonoBehaviour {
    // 开始游戏按钮
    [SerializeField] private Button startGameButton;

    private void Start() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Client_Success, UpdateEnable);
    }

    public void StartGame() {
        EntranceController.Get().StartSingleGame();
    }

    // 禁用Client上的开始游戏按钮
    private void UpdateEnable() {
        if (!Players.Get().LocalPlayer().isHost)
            startGameButton.interactable = false;
    }
    
    
}