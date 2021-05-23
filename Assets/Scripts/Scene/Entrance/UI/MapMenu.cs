using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///   <para> 选择地图的菜单 </para>
/// </summary>
public class MapMenu : MonoBehaviour {

    // 网格
    [SerializeField] private GridLayoutGroup grid;

    // 地图一项的prefab
    [SerializeField] private GameObject itemPrefab;

    void Start() {
        itemPrefab.SetActive(false);
        // 获取所有地图
        foreach(KeyValuePair<string, SaveEntity> kvp in SaveResource.saveManager.saveEntities) {
            AddItem(kvp.Key);
        }
    }

    /// <summary>
    ///   <para> 添加地图项 </para>
    /// </summary>
    public void AddItem(string filename) {
        // 创建新的子节点
        GameObject prefabInstance = Instantiate(itemPrefab);
        prefabInstance.transform.SetParent(grid.transform);
        prefabInstance.transform.localScale=new Vector3Int(1,1,1);

        // 设置子节点
        MapItem item = prefabInstance.GetComponent<MapItem>();
        item.Filename = filename;

        // 显示
        item.gameObject.SetActive(true);
    }
}