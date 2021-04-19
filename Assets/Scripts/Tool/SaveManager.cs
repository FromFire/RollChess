using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structure;

/// <summary>
///   <para> 存档管理 </para>
///   <para> 本类是static类 </para>
/// </summary>
public class SaveManager {

    /// <summary>
    ///   <para> 路径对应的存档 </para>
    /// </summary>  
    static public Dictionary<string, SaveEntity> entities {get;}

    /// <summary>
    ///   <para> 读取所有存档 </para>
    /// </summary>  
    static public void LoadAllSave(){

    }

    /// <summary>
    ///   <para> 读取单个存档 </para>
    /// </summary>  
    static public SaveEntity LoadSave(string path){
        return null;
    }

    /// <summary>
    ///   <para> 写入单个存档 </para>
    /// </summary>  
    static public void WriteSave(string path, SaveEntity saveEntity){
        
    }

    /// <summary>
    ///   <para> 存档路径获取略缩图 </para>
    /// </summary>  
    static public Sprite GetThumb(string path){
        return null;
    }

    /// <summary>
    ///   <para> 存储略缩图 </para>
    /// </summary>  
    static public void SetThumb(string path){
        
    }

    /// <summary>
    ///   <para> 复制存档 </para>
    /// </summary>  
    static public string Duplicate(string path){
        return null;
    }

    /// <summary>
    ///   <para> 删除存档 </para>
    /// </summary>  
    static public void Delete(string path){
        
    }
}