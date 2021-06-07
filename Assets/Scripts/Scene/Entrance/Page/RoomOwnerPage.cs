using UnityEngine;

/// <summary>
///   <para> 房主选地图和AI </para>
/// </summary>
public class RoomOwnerPage : MonoBehaviour
{
    private void Start()
    {
        NetworkResource.networkSubject.Attach(ModelModifyEvent.Server_Success, CreateRoomSuccess);
    }
    
    /// <summary>
    ///   <para> 创建房间，打开服务器 </para>
    /// </summary>
    public void CreateRoom()
    {
        ConnectionController.singleton.CreateRoom();
    }
    
    /// <summary>
    ///   <para> 服务器打开成功，跳转至房间界面 </para>
    /// </summary>
    public void CreateRoomSuccess()
    {
        PanelManager.Get().NowPanel = PanelManager.Get().room;
    }
}