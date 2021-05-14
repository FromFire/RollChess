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
        // 创建新的子节点
        GameObject prefabInstance = Instantiate(itemPrefab);
        prefabInstance.transform.SetParent(grid.transform);
        prefabInstance.transform.localScale=new Vector3Int(1,1,1);

        // 设置子节点
        MapItem item = prefabInstance.GetComponent<MapItem>();
        item.Filename = "";
    }
}