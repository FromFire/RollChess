using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> Entrance整体管理 </para>
/// </summary>
public class EntranceController : MonoBehaviour {

    void Start() {
        SaveResource.saveManager.LoadAllSave();
    }

    /// <summary>
    ///   <para> 开始游戏 </para>
    /// </summary>
    public void StartGame()
    {
        if (!isGameAvalible())
            return;

        //符合要求，进入游戏
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    ///   <para> 判断地图是否合法 </para>
    /// </summary>
    bool isGameAvalible()
    {
        if (!EntranceResource.playerOperationController.IsPlayerValid() ||
            !EntranceResource.mapOperationController.IsMapValid())
            return false;
        return true;
    }

    /// <summary>
    ///   <para> 创建房间 </para>
    /// </summary>
    public void CreateRoom() {
        NetworkResource.networkManager.StartHost();
    }
    
    /// <summary>
    ///   <para> 加入房间 </para>
    /// </summary>
    public void JoinRoom(string ip) {
        // localhost的情况
        if (ip.Equals("localhost") || ip.Length == 0)
        {
            NetworkResource.networkManager.StartClient();
            return;
        }
        
        // 手动输入ip的情况
        if (IpIsValid(ip))
        {
            // 运行客户端
            NetworkResource.networkManager.networkAddress = ip;
            NetworkResource.networkManager.StartClient();
        }
    }
    
    /// <summary>
    ///   <para> 创建房间 </para>
    /// </summary>
    public void CancelJoinRoom() {
        NetworkResource.networkManager.StopClient();
    }
    
    /// <summary>
    ///   <para> 判断ip地址是否合法 </para>
    /// </summary>
    bool IpIsValid(string ip)
    {
        string patternIp = @"^((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))$";
        return Regex.IsMatch(ip, patternIp);
    }
}