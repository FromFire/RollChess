using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

/// <summary>
///   <para> 玩家 </para>
/// </summary>
public class NetworkResource : MonoBehaviour
{
    public static MyNetworkManager networkManager;
    public static NetworkInfo networkInfo;

    // 赋值用
    [SerializeField] MyNetworkManager _networkManager;
    [SerializeField] NetworkInfo _networkInfo;
    
    /// <summary>
    ///   <para> 地图选择的推送 </para>
    /// </summary>
    static public Subject networkSubject;

    private void Start()
    {
        networkManager = _networkManager;
        networkInfo = _networkInfo;
        networkSubject = new Subject();
    }
}