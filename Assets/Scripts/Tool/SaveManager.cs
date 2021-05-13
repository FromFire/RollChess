using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
///   <para> 存档管理 </para>
///   <para> 本类是static类 </para>
/// </summary>
public class SaveManager {

    /// <summary>
    ///   <para> 存档所在路径 </para>
    /// </summary> 
    static string savePath = Application.persistentDataPath;

    /// <summary>
    ///   <para> 地图数据所在路径 </para>
    /// </summary> 
    static string savePathMap;

    /// <summary>
    ///   <para> 略缩图数据所在路径 </para>
    /// </summary> 
    static string savePathThumb;

    /// <summary>
    ///   <para> 路径对应的存档 </para>
    /// </summary>  
    static public Dictionary<string, SaveEntity> saveEntities = new Dictionary<string, SaveEntity>();

    static SaveManager() {
        // 初始化路径
        savePathMap = Path.Combine(savePath, "Maps");
        savePathThumb = Path.Combine(savePath, "Thumbs");

        // 判断路径是否存在，如果不存在，则新建文件夹
        if(!Directory.Exists(savePathMap))
            Directory.CreateDirectory(savePathMap);
        if(!Directory.Exists(savePathThumb))
            Directory.CreateDirectory(savePathThumb);
    }

    /// <summary>
    ///   <para> 将saveEntity加载到内存，即初始化model </para>
    /// </summary>
    static public void Load(SaveEntity entity) {
        ModelResource.board.Load(entity);
        ModelResource.tokenSet.Load(entity);
    }

    /// <summary>
    ///   <para> 将内存存储到saveEntity中，即输出model </para>
    /// </summary>
    static public SaveEntity Save() {
        SaveEntity entity = new SaveEntity();

        // 存储board
        Board board = ModelResource.board;
        HashSet<Vector2Int> cells = board.ToPositionSet();
        foreach(Vector2Int pos in cells) {
            Cell cell = board.Get(pos);
            if(cell.Walkable) {
                // land
                entity.map.Add(new LandSaveEntity(pos.x, pos.y));
                // special
                if(cell.Effect != SpecialEffect.None && cell.Effect != SpecialEffect.Portal)
                    entity.special.Add(new SpecialSaveEntity(pos.x, pos.y, Transform.specialNameOfEffect[cell.Effect]));
                // portal
                if(cell.Effect == SpecialEffect.Portal)
                    entity.portal.Add(new PortalSaveEntity(pos.x, pos.y, cell.Target.x, cell.Target.y));
            }
                
        }

        // 存储token
        TokenSet tokenSet = ModelResource.tokenSet;
        List<Vector2Int> tokens = tokenSet.Query(PlayerID.None, PlayerID.None);
        foreach(Vector2Int pos in tokens) {
            Token token = tokenSet.Get(pos);
            entity.token.Add(new TokenSaveEntity(token.Position.x, token.Position.y, (int)token.Player));
        }

        return entity;
    }

    /// <summary>
    ///   <para> 已知地图名，返回完整路径 </para>
    /// </summary>
    static public string MapNameToPath(string name) {
        return Path.Combine(savePathMap, name) + ".json"; 
    }

    /// <summary>
    ///   <para> 读取所有存档 </para>
    /// </summary>
    static public void LoadAllSave(){
        // 获取所有合法的地图名称
        // 要求地图文件在/Maps中，预览图片在/Thumbs中，缺一不可
        HashSet<string> mapsStrings = new HashSet<string>(GetFilenames(savePathMap, "json"));
        HashSet<string> thumbnailsStrings = new HashSet<string>(GetFilenames(savePathThumb, "png"));
        List<string> mapNames = new List<string>(mapsStrings.Union(thumbnailsStrings));

        // 将合法地图名称作为键存入saveEntities，值为空
        foreach(string mapName in mapNames) {
            saveEntities[mapName] = null;
        }
    }

    /// <summary>
    ///   <para> 读取单个存档 </para>
    /// </summary>  
    static public SaveEntity LoadMap(string filename){
        // 若已有存储，直接返回
        if(saveEntities.ContainsKey(filename) && !(saveEntities[filename] is null))
            return saveEntities[filename];

        // 读取文件内容
        byte[] bytes = ReadFile(Path.Combine(savePathMap, filename) + ".json");
        string json = System.Text.Encoding.Default.GetString(bytes);

        // 将json字符串转换为SaveEntity类
        SaveEntity saveEntity = SaveEntity.FromJson(json);
        
        // 在saveEntities中缓存
        saveEntities[filename] = saveEntity;
        return saveEntity;
    }

    /// <summary>
    ///   <para> 写入单个地图文件 </para>
    /// </summary>
    static public void SaveMap(string filename, SaveEntity saveEntity){
        string path = Path.Combine(savePath, filename) + ".json";
        byte[] bytes = Encoding.UTF8.GetBytes(saveEntity.ToJson());
        WriteFile(path, bytes);
    }

    /// <summary>
    ///   <para> 存档路径获取略缩图 </para>
    /// </summary>  
    static public Sprite LoadThumb(string filename){
        // 以byte[]形式读取图片
        byte[] imgByte = ReadFile(Path.Combine(savePathThumb, filename) + ".png");

        // 将byte[]转换为Texture2D
        Texture2D texture = new Texture2D(10, 10);
        texture.LoadImage(imgByte);

        // 将Texture2D转换为Sprite
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sp;
    }

    /// <summary>
    ///   <para> 存储略缩图，png格式 </para>
    /// </summary>  
    static public void SaveThumb(byte[] image, string filename){
        WriteFile(Path.Combine(savePathMap, filename) + ".png", image);
    }

    /// <summary>
    ///   <para> 复制地图和略缩图 </para>
    ///   <returns> 返回副本的文件名 </returns>
    /// </summary>  
    static public string Duplicate(string filename){
        // 为副本起文件名
        // 格式是"原地图-2"，若文件名重复则继续增加数字
        string filenameDup = "";
        for(int i=2; ; i++) {
            if( !File.Exists(Path.Combine(savePathMap, filename) + "-" + i + ".json") 
                && !File.Exists(Path.Combine(savePathThumb, filename) + "-" + i + ".png") ) {
                filenameDup = filename + "-" + i;
                break;
            }
        }

        // 复制文件，地图和略缩图各复制一次
        File.Copy(Path.Combine(savePathMap, filename) + ".json", Path.Combine(savePathMap, filenameDup) + ".json");
        File.Copy(Path.Combine(savePathThumb, filename) + ".png", Path.Combine(savePathThumb, filenameDup) + ".png");

        return filenameDup;
    }

    /// <summary>
    ///   <para> 删除地图和略缩图 </para>
    /// </summary>  
    static public void Delete(string filename){
        File.Delete(Path.Combine(savePathMap, filename) + ".json");
        File.Delete(Path.Combine(savePathThumb, filename) + ".png");
    }

    /// <summary>
    /// <para> 读取path目录下所有文件的文件名，后缀为extension </para>
    /// <para> 返回的文件名不包括文件夹路径和后缀 </para>
    /// </summary>
    static private List<string> GetFilenames(string path, string extension) {
        // 获取所有文件名
        List<string> ret = new List<string>();
        Debug.Assert(Directory.Exists(path));
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);
  
        // 筛选指定后缀名
        for(int i=0;i<files.Length;i++){
            string filename = files[i].Name;
            if (filename.EndsWith("."+extension)){
                ret.Add(filename.Split('.')[0]);
            }
        }
        return ret;
    }

    /// <summary>
    /// <para> 存储文件，路径为path，内容为bytes </para>
    /// </summary>
    static public void WriteFile(string path, byte[] bytes) {
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Flush();
        fileStream.Close();
    }

    /// <summary>
    /// <para> 读取文件，路径为path，byte[]格式返回 </para>
    /// </summary>
    static public byte[] ReadFile(string path) {
        FileStream fs = new FileStream(path, FileMode.Open);
        byte[] bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        fs.Close();
        return bytes;
    }
}