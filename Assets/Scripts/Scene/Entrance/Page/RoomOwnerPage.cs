using UnityEngine;

/// <summary>
///   <para> 房主选地图和AI </para>
/// </summary>
public class RoomOwnerPage : MonoBehaviour
{
    /// <summary>
    ///   <para> 创建房间，打开服务器 </para>
    /// </summary>
    public void CreateRoom()
    {
        EntranceResource.entranceController.CreateRoom();
    }
}