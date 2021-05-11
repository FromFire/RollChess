using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

/// <summary>
///   <para> 获取公有资源 </para>
/// </summary>
public class GameResource : MonoBehaviour  {
    // Model
    static public GameState gameState;
    static public SpecialIntroduction specialIntroduction;
    static public MapChooseState mapChooseState;

    // Controller
    static public BoardAssistant boardAssistant;
    static public BoardController boardController;
    static public TokenController tokenController;
    static public GameController gameController;
    static public MoveProcessor moveProcessor;

    /// <summary>
    ///   <para> 游戏状态的推送 </para>
    /// </summary>
    static public Subject gameStateSubject = new Subject();

    // 下面是用来实例化的
    // Model
    [SerializeField] private GameState _gameState;
    [SerializeField] private SpecialIntroduction _specialIntroduction;

    // Controller
    [SerializeField] private BoardAssistant _boardAssistant;
    [SerializeField] private BoardController _boardController;
    [SerializeField] private TokenController _tokenController;
    [SerializeField] private GameController _gameController;
    [SerializeField] private MoveProcessor _moveProcessor;

    // 实例化
    void Start() {
        // Model
        gameState = _gameState;
        specialIntroduction = _specialIntroduction;

        // Controller
        boardAssistant = _boardAssistant;
        boardController = _boardController;
        tokenController = _tokenController;
        gameController = _gameController;
        moveProcessor = _moveProcessor;

        // 此类来自Entrance类
        GameObject mapChooseObject = GameObject.Find("MapChooseState");
        if(mapChooseObject)
            mapChooseState = mapChooseObject.GetComponent<MapChooseState>();
    }
}