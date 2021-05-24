using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   <para> 存档管理 </para>
///   <para> 面向Model，负责将saveEntity读到内存，或反之 </para>
/// </summary>
public class SaveLoader : MonoBehaviour {
    private byte[] screenShot;

    /// <summary>
    ///   <para> 将saveEntity加载到内存，即初始化model </para>
    /// </summary>
    public void Load(SaveEntity entity) {
        ModelResource.board.Load(entity);
        ModelResource.tokenSet.Load(entity);
    }

    /// <summary>
    ///   <para> 将内存存储到saveEntity中，即输出model </para>
    /// </summary>
    public SaveEntity Save(string filename) {
        SaveEntity entity = SaveResource.saveManager.LoadMap(filename);

        // 清空entity的所有列表
        entity.token.Clear();
        entity.map.Clear();
        entity.special.Clear();
        entity.portal.Clear();

        // 存储board
        Board board = ModelResource.board;
        HashSet<Vector2Int> cells = board.ToPositionSet();
        foreach(Vector2Int pos in cells) {
            Cell cell = board.Get(pos);
            if(cell.Walkable) {
                // land
                entity.map.Add(new LandSaveEntity(pos.x, pos.y));
                // special
                if(cell.Effect != SpecialEffect.None && cell.Effect != SpecialEffect.Portal)
                    entity.special.Add(new SpecialSaveEntity(pos.x, pos.y, Transform.specialNameOfEffect[cell.Effect]));
                // portal
                if(cell.Effect == SpecialEffect.Portal)
                    entity.portal.Add(new PortalSaveEntity(pos.x, pos.y, cell.Target.x, cell.Target.y));
            }
        }

        // 存储token
        TokenSet tokenSet = ModelResource.tokenSet;
        List<Vector2Int> tokens = tokenSet.Query(PlayerID.None, PlayerID.None);
        foreach(Vector2Int pos in tokens) {
            Token token = tokenSet.Get(pos);
            entity.token.Add(new TokenSaveEntity(token.Position.x, token.Position.y, (int)token.Player));
        }

        return entity;
    }

    /// <summary>
    ///   <para> 截图 </para>
    ///   <para> 目前非常不完善，只能截现在屏幕上显示的，考虑将来加一个截图摄像机 </para>
    /// </summary>
    public void Capture() {
        // 使用协程，目的是在地图选然后截图
        StartCoroutine(CaptureCoro());
    }

    // 截图实现
    IEnumerator CaptureCoro()
    {
        // 必须地图渲染后才能截图，否则报错
        yield return new WaitForEndOfFrame();

        // 计算尺寸，截屏幕中间，正方形
        // 截正方形的图
        int length = Mathf.Min(Screen.width, Screen.height);
        int xStart = (Screen.width - length) / 2;
        int yStart = (Screen.height - length) / 2;

        Texture2D tex = new Texture2D(length, length);
        tex.ReadPixels(new Rect(new Vector2Int(xStart, yStart), new Vector2(length, length)), 0, 0);
        screenShot = tex.EncodeToPNG();
    }

    /// <summary>
    ///   <para> 获取截图，只能获取一次 </para>
    /// </summary>
    public byte[] ScreenShot {
        get {
            byte[] ret = screenShot;
            screenShot = null;
            return ret; 
        }
    }

}