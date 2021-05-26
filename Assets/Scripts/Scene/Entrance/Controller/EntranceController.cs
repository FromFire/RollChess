using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
///   <para> 地图选择中的一项 </para>
/// </summary>
public class EntranceController : MonoBehaviour {

    void Start() {
        SaveResource.saveManager.LoadAllSave();
    }

    /// <summary>
    ///   <para> 选择地图 </para>
    /// </summary>
    public void Choose(string filename) {
        EntranceResource.mapChooseState.MapFileName = filename;
    }

    /// <summary>
    ///   <para> 修改操作方式 </para>
    /// </summary>
    public void ShiftPlayerFrom(PlayerID id) {
        // 轮换到下一个
        PlayerForm form = EntranceResource.mapChooseState.GetPlayerForm(id);
        int count = System.Enum.GetNames(typeof(PlayerForm)).Length;
        form = (PlayerForm)( ((int)form+1) % count );
        EntranceResource.mapChooseState.SetPlayerForm(id, form);
        Debug.Log(id + ": " + form);
    }

    /// <summary>
    ///   <para> 开始游戏 </para>
    /// </summary>
    public void StartGame() {
        // 获取players
        List<PlayerForm> player = new List<PlayerForm> {
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Red), 
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Blue), 
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Yellow), 
            EntranceResource.mapChooseState.GetPlayerForm(PlayerID.Green)
        };

        // 检查players的合法性
        // 4个player至少有1个由玩家操控，且player最少为2人
        int totalPlayers = 0;
        bool haveHuman = false;
        foreach(PlayerForm choice in player) {
            if(choice != PlayerForm.Banned) {
                totalPlayers ++;
            }
            if(choice == PlayerForm.Player) {
                haveHuman = true;
            }
        }
        // 错误：若players人数低于最小人数限制
        int minPlayer = EntranceResource.mapChooseState.PlayerLimit.min;
        if(totalPlayers < minPlayer) {
            WarningManager.errors.Add(new WarningModel("角色数量最少为" + minPlayer + "人！"));
            return;
        }
        // 错误：若玩家数为0
        if(!haveHuman) {
            WarningManager.errors.Add(new WarningModel("至少有一个玩家参与游戏！"));
            return;
        }

        //获取maps
        string mapName = EntranceResource.mapChooseState.MapFileName;
        //地图不能为空
        if(mapName.Length <= 1) {
            WarningManager.errors.Add(new WarningModel("请选择地图！"));
            return;
        }

        //符合要求，进入游戏
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    ///   <para> 复制地图 </para>
    /// </summary>
    public string CopyMap() {
        // 复制
        string origin = EntranceResource.mapChooseState.MapFileName;
        string copy = SaveResource.saveManager.Duplicate(origin);
        // 地图名称改为<地图名>-副本
        SaveEntity saveEntity = SaveResource.saveManager.LoadMap(copy);
        saveEntity.mapName = EntranceResource.mapChooseState.MapName + "-副本";
        SaveResource.saveManager.SaveMap(saveEntity, copy);
        return copy;
    }

    /// <summary>
    ///   <para> 编辑地图 </para>
    /// </summary>
    public void EditMap() {
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("MapEditor");
    }

    /// <summary>
    ///   <para> 删除地图 </para>
    /// </summary>
    public void DeleteMap() {
        // 地图为空的情况
        string toDel = EntranceResource.mapChooseState.MapFileName;
        SaveResource.saveManager.Delete(EntranceResource.mapChooseState.MapFileName);
    }

    /// <summary>
    ///   <para> 准备新建地图 </para>
    /// </summary>
    public void PrepareNewMap() {
        EntranceResource.mapChooseState.MapFileName = "";
    }

    /// <summary>
    ///   <para> 新建地图 </para>
    /// </summary>
    public void NewMap(string mapName, int min, int max) {
        // 合法性检查
        if(mapName.Length == 0 || min <= 0  || max < min || max > 4)
            return;
        
        // 创建新地图
        SaveEntity saveEntity = new SaveEntity();
        string filename = SaveResource.saveManager.NewMap(saveEntity, mapName);

        // 修改基本属性
        saveEntity = SaveResource.saveManager.LoadMap(filename);
        saveEntity.mapName = mapName;
        saveEntity.player.min = min;
        saveEntity.player.max = max;
        SaveResource.saveManager.SaveMap(saveEntity, filename);

        // 修改MapChooseState
        EntranceResource.mapChooseState.MapFileName = filename;

        // 进入地图编辑器
        GameObject.DontDestroyOnLoad(EntranceResource.mapChooseState.gameObject);
        SceneManager.LoadScene("MapEditor");
    }

    /// <summary>
    ///   <para> 修改地图基本信息 </para>
    /// </summary>
    public void ModifySetting(string mapName, int min, int max) {
        // 合法性检查
        if(mapName.Length == 0 || min <= 0  || max < min || max > 4)
            return;

        // 修改
        string filename = EntranceResource.mapChooseState.MapFileName;
        SaveEntity saveEntity = SaveResource.saveManager.LoadMap(filename);
        saveEntity.mapName = mapName;
        saveEntity.player.min = min;
        saveEntity.player.max = max;

        // 修改状态
        EntranceResource.mapChooseState.MapName = mapName;
        EntranceResource.mapChooseState.PlayerLimit = (min, max);

        // 保存
        SaveResource.saveManager.SaveMap(saveEntity, filename);
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