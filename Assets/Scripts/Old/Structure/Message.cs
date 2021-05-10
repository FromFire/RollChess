using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Object = System.Object;

/// <summary>
///   <para> 以string为键，存储各个类型的消息 </para>
/// </summary>
public class Message : MonoBehaviour
{
    // 可以存储不同类型的字典
    Dictionary<string, Object> dic;

    /// <summary>
    ///   <para> 获取消息 </para>
    /// </summary>
    public T GetMessage<T> (string key) {
        return (T) dic[key];
    }

    /// <summary>
    ///   <para> 设置消息 </para>
    ///   <para> 若该键已存在，则覆写，否则添加新的 </para>
    /// </summary>
    public void SetMessage<T> (string key, T value) {
        dic[key] = (Object)value;
    }

    /// <summary>
    ///   <para> 删除消息 </para>
    /// </summary>
    public void RemoveMessage(string key) {
        dic.Remove(key);
    }

    /// <summary>
    ///   <para> 删除所有消息 </para>
    /// </summary>
    public void Clear() {
        dic.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        dic = new Dictionary<string, Object>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
