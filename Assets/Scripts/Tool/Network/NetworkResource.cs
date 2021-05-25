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
    public static NetworkManager networkManager;
    
    [SerializeField] NetworkManager _networkManager;

    private void Start()
    {
        networkManager = _networkManager;
    }
}