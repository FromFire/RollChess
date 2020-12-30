using UnityEngine;

public class JsonHelper {
    /// <summary>
    ///   <para> 转换为json格式文本 </para>
    /// </summary>
    public string ToJson<T> (T entity) {
        return JsonUtility.ToJson(entity);
    }

    /// <summary>
    ///   <para> 从json格式转换 </para>
    /// </summary>
    static public T FromJson<T>(string json) {
        return JsonUtility.FromJson<T>(json);
    }
}