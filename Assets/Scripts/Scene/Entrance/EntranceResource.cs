using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class EntranceResource : MonoBehaviour  {
    // Model
    static public MapChooseState mapChooseState;

    // Controller
    static public EntranceController entranceController;
    static public MapOperationController mapOperationController;
    static public PlayerOperationController playerOperationController;

    // 下面是用来实例化的
    // Model
    [SerializeField] private MapChooseState _mapChooseState;

    // Controller
    [SerializeField] private EntranceController _entranceController;
    [SerializeField] private MapOperationController _mapOperationController;
    [SerializeField] private PlayerOperationController _playerOperationController;

    // 实例化
    void Start() {
        // Model
        mapChooseState = _mapChooseState;
        
        // Controller
        entranceController = _entranceController;
        mapOperationController = _mapOperationController;
        playerOperationController = _playerOperationController;
    }
}