using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 玩家列表 </para>
/// </summary>
public class PlayerList : MonoBehaviour {
    // item的prefab
    [SerializeField] private GameObject prefab;

    // 列表
    [SerializeField] private VerticalLayoutGroup list;

    // 所有item列表
    Dictionary<uint, PlayerItem> items = new Dictionary<uint, PlayerItem>();

    private void Awake() {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Player_Change, UpdateList);
    }

    void Start() {
        prefab.SetActive(false);
    }

    /// <summary>
    ///   <para> 更新列表 </para>
    /// </summary>
    void UpdateList() {
        // 获取已显示的id（idShowing）和已有id（idAll）
        List<uint> idShowing = new List<uint>(items.Keys);
        List<uint> idAll = new List<uint>(Players.Get().players.Keys);
        
        // 遍历idShowing，若idAll里有则更新显示，若没有则删除此item
        foreach (uint id in idShowing)
            if (idAll.Contains(id))
                items[id].Id = id;
            else
                RemoveItem(id);
        
        // 遍历idAll，若idShowing里没有，则添加此item
        foreach (uint id in idAll)
            if(!idShowing.Contains(id))
                AddItem(id);
    }

    // 删除条目
    void RemoveItem(uint item) {
        items[item].gameObject.SetActive(false);
        items.Remove(item);
    }
    
    // 添加条目
    void AddItem(uint playerId) {
        // 创建新的子节点
        GameObject prefabInstance = Instantiate(prefab);
        prefabInstance.transform.SetParent(list.transform);
        prefabInstance.transform.localScale = new Vector3Int(1, 1, 1);

        // 设置子节点
        PlayerItem item = prefabInstance.GetComponent<PlayerItem>();
        item.Id = playerId;
        items.Add(item.Id, item);

        // 显示
        item.gameObject.SetActive(true);
    }
}