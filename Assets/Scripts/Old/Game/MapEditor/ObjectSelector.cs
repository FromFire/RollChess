// using System.Collections;
// using System.Collections.Generic;
// using Structure_old;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using Widget;

// public class ObjectSelector : MonoBehaviour {
//     public TilemapManager land = null;
//     public TilemapManager landPreview = null;
//     public TilemapManager special = null;
//     public TilemapManager specialPreview = null;
//     public TilemapManager token = null;
//     public TilemapManager tokenPreview = null;
//     private List<TilemapManager> tilemapManagers;
//     private List<TilemapManager> previewTilemapManagers;
//     Dictionary<TilemapType, TilemapManager> tilemapManagerOfTilemapType;
//     Dictionary<TilemapType, TilemapManager> previewTilemapManagerOfTilemapType;

//     void Start() {
//         tilemapManagerOfTilemapType = new Dictionary<TilemapType, TilemapManager>() {
//             {TilemapType.Land, land},
//             {TilemapType.Special, special},
//             {TilemapType.Token, token}
//         };
//         tilemapManagers = new List<TilemapManager>();
//         foreach (KeyValuePair<TilemapType, TilemapManager> pair in tilemapManagerOfTilemapType) {
//             tilemapManagers.Add(pair.Value);
//             pair.Value.type = pair.Key;
//         }

//         previewTilemapManagerOfTilemapType = new Dictionary<TilemapType, TilemapManager>() {
//             {TilemapType.Land, landPreview},
//             {TilemapType.Special, specialPreview},
//             {TilemapType.Token, tokenPreview}
//         };
//         previewTilemapManagers = new List<TilemapManager>();
//         foreach (KeyValuePair<TilemapType, TilemapManager> pair in previewTilemapManagerOfTilemapType) {
//             previewTilemapManagers.Add(pair.Value);
//             pair.Value.type = pair.Key;
//         }
//     }

//     public TilemapManager GetTilemapManager(TilemapType type) {
//         return tilemapManagerOfTilemapType[type];
//     }

//     public List<TilemapManager> GetTilemapManagers() {
//         return tilemapManagers;
//     }

//     public TilemapManager GetPreviewTilemapManager(TilemapType type) {
//         return previewTilemapManagerOfTilemapType[type];
//     }

//     public List<TilemapManager> GetPreviewTilemapManagers() {
//         return previewTilemapManagers;
//     }
// }