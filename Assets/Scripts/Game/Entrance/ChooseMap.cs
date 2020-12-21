using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class ChooseMap : MonoBehaviour
{
    //滑动主体
    public ScrollRect scrollRect;

    //grid管理
    public MenuGridScroller menuGridScroller;

    //地图预览
    public Image mapPreview;

    //地图名称预览
    public Text mapName;

    //地图文件名
    string mapFilename;

    // Start is called before the first frame update
    void Start()
    {
        List<string> filenames = GetValidMapNames();
        Debug.Log(filenames.Count);
        foreach(string filename in filenames) {
            Debug.Log(filename);
            menuGridScroller.AddButton(Resources.Load<Sprite>("Thumbnails/"+filename), () => {PreviewMap("Thumbnails/"+filename);});
        }
    }

    /// <summary>
    /// <para> 返回所有合法的地图名称 </para>
    /// <para> 合法：地图文件在Resources/Maps中，预览图片在Resource/Thumbnails中，缺一不可 </para>
    /// </summary>
    public List<string> GetValidMapNames() {
        // 读取Maps和Thumbnails中重合的文件名
        HashSet<string> mapsStrings = new HashSet<string>(GetFilenames("Assets/Resources/Maps", "json"));
        HashSet<string> thumbnailsStrings = new HashSet<string>(GetFilenames("Assets/Resources/Thumbnails", "png"));
        return new List<string>(mapsStrings.Union(thumbnailsStrings));
    }

    /// <summary>
    /// <para> 读取path目录下所有文件的文件名，后缀为extension </para>
    /// </summary>
    List<string> GetFilenames(string path, string extension) {
        List<string> ret = new List<string>();
        Debug.Assert(Directory.Exists(path));
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);
  
        for(int i=0;i<files.Length;i++){
            string filename = files[i].Name;
            if (filename.EndsWith("."+extension)){
                ret.Add(filename.Split('.')[0]);
            }
        }
        return ret;
    }

    /// <summary>
    /// <para> 预览地图，是grid中每项的点击响应函数 </para>
    /// </summary>
    public void PreviewMap(string filename) {
        mapFilename = filename.Split('/').Last();
        mapName.text = mapFilename;
        Sprite image = Resources.Load<Sprite>(filename);
        mapPreview.sprite = image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
