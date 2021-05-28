using System;
using System.Collections.Generic;
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
        HashSet<Player> players = new HashSet<Player>(NetworkResource.networkInfo.players);

        // 没有增加新player
        if (players.Count == items.Count)
            return;
        // 寻找新的player
        foreach (PlayerItem item in items) {
            if (!players.Contains(item.player)) {
                players.Remove(item.player);
            }
        }
        
        // 新增条目
        foreach (Player player in players) {
            AddItem(player);
        }
    }

    /// <summary>
    ///   <para> 添加项目 </para>
    /// </summary>
    void AddItem(Player player) {
        // 创建新的子节点
        GameObject prefabInstance = Instantiate(prefab);
        prefabInstance.transform.SetParent(list.transform);
        prefabInstance.transform.localScale = new Vector3Int(1, 1, 1);

        // 设置子节点
        PlayerItem item = prefabInstance.GetComponent<PlayerItem>();
        item.Display(player);
        items.Add(item);

        // 显示
        item.gameObject.SetActive(true);
    }
}