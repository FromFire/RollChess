// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System.IO;
// using System.Linq;

// public class ChooseMap : MonoBehaviour
// {
//     //grid管理
//     public MenuGridScroller menuGridScroller;

//     //地图预览
//     public Image mapPreview;

//     //地图名称预览
//     public Text mapName;

//     //地图文件名
//     string mapFilename;

//     // 玩家当前选择的地图
//     BoardEntity currentMap;

//     // 主菜单
//     public MenuGUI menuGUI;

//     // Start is called before the first frame update
//     void Start()
//     {
//         List<string> filenames = GetValidMapNames();
//         Debug.Log(filenames.Count);
//         foreach(string filename in filenames) {
//             menuGridScroller.AddButton(Resources.Load<Sprite>("Thumbnails/"+filename), () => {PreviewMap("Thumbnails/"+filename);});
//         }
//     }

//     /// <summary>
//     /// <para> 预览地图，是grid中每项的点击响应函数 </para>
//     /// </summary>
//     public void PreviewMap(string filename) {
//         // 预览地图
//         Sprite image = Resources.Load<Sprite>(filename);
//         mapPreview.sprite = image;
//         mapPreview.color = Color.white;
//         // 通知MenuGUI
//         mapFilename = filename.Split('/').Last();
//         menuGUI.SetCurrentMap(mapFilename);
//     }

//     /// <summary>
//     /// <para> 设置地图，显示地图名称 </para>
//     /// </summary>
//     public BoardEntity CurrentMap {
//         set {
//             currentMap = value;
//             mapName.text = currentMap.mapName;
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
