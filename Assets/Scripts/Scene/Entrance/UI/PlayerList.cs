using System;
using System.Collections.Generic;
using System.Linq;
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
    List<PlayerItem> items = new List<PlayerItem>();

    void Start() {
        prefab.SetActive(false);
        NetworkResource.networkSubject.Attach(ModelModifyEvent.New_Client, NewItem);
        NewItem();
    }

    /// <summary>
    ///   <para> 新player加入时触发 </para>
    /// </summary>
    public void NewItem() {
        HashSet<uint> ids = new HashSet<uint>(NetworkResource.networkInfo.players.Keys);

        // 没有增加新player
        if (ids.Count == items.Count)
            return;
        // 寻找新的player
        foreach (PlayerItem item in items) {
            if (!ids.Contains(item.Id)) {
                ids.Remove(item.Id);
            }
        }
        
        // 新增条目
        foreach (uint id in ids) {
            AddItem(id);
        }
    }

    /// <summary>
    ///   <para> 添加项目 </para>
    /// </summary>
    void AddItem(uint playerId) {
        // 创建新的子节点
        GameObject prefabInstance = Instantiate(prefab);
        prefabInstance.transform.SetParent(list.transform);
        prefabInstance.transform.localScale = new Vector3Int(1, 1, 1);

        // 设置子节点
        PlayerItem item = prefabInstance.GetComponent<PlayerItem>();
        item.Id = playerId;
        items.Add(item);

        // 显示
        item.gameObject.SetActive(true);
    }
}