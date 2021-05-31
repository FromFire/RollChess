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
        NetworkResource.networkSubject.Attach(ModelModifyEvent.New_Client, UpdateSelf);
    }

    void Start() {
        prefab.SetActive(false);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(2))
            UpdateSelf();
    }

    /// <summary>
    ///   <para> 更新自身 </para>
    /// </summary>
    void UpdateSelf() {
        // 获取已显示的id（idShowing）和已有id（idAll）
        List<uint> idShowing = new List<uint>();
        foreach (uint id in items.Keys)
            idShowing.Add(id);
        List<uint> idAll = new List<uint>(NetworkResource.networkInfo.players.Keys);
        
        // 遍历idShowing，若idAll里有则更新显示，若没有则删除此item
        foreach (uint id in idShowing)
            if(idAll.Contains(id))
                UpdateItem(items[id]);
            else
                RemoveItem(id);
        
        // 遍历idAll，若idShowing里没有，则添加此item
        foreach (uint id in idAll)
            if(!idShowing.Contains(id))
                AddItem(id);
    }

    // 更新条目
    void UpdateItem(PlayerItem item) {
        item.Id = item.Id;
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