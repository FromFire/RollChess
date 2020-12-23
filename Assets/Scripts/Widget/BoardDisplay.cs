using System.Collections.Generic;
using Structure;
using UnityEngine;

namespace Widget {
    public class BoardDisplay : MonoBehaviour {
        //显示地板
        public TilemapManager tilemapManagerBoard;

        //显示特殊格子贴图
        public TilemapManager tilemapManagerSpecial;

        //显示传送门的箭头
        GameObject portalArrows;

        //显示自身
        public void Display(BaseBoard<SingleGrid> map) {
            //获取有效数据列表
            HashSet<Vector2Int> keyInfo = map.ToList();
            HashSet<Vector2Int> poses = new HashSet<Vector2Int>();
            foreach (Vector2Int pos in keyInfo) {
                //去除不可走的坐标
                if (map.GetData(pos).walkable == true) {
                    poses.Add(pos);
                }
            }

            //显示TilemapBoard层，地图格子
            foreach (Vector2Int pos in poses)
                tilemapManagerBoard.SetTile(pos, TileType.Land_Lawn_Green);

            //显示TilemapSpecial层，特殊格子效果
            foreach (Vector2Int pos in poses) {
                SingleGrid.Effect effect = map.GetData(pos).effect;
                switch (effect) {
                    case SingleGrid.Effect.DoubleStep:
                        tilemapManagerSpecial.SetTile(pos, TileType.Special_DoubleStep);
                        break;
                    case SingleGrid.Effect.BrokenBridge:
                        tilemapManagerSpecial.SetTile(pos, TileType.Special_BrokenBridge);
                        break;
                    case SingleGrid.Effect.Portal:
                        tilemapManagerSpecial.SetTile(pos, TileType.Special_Portal);
                        break;
                }
            }

            //显示传送门之间的箭头
            portalArrows = GameObject.Find("/Grid/PortalArrows");
            foreach (Vector2Int pos in poses) {
                if (map.GetData(pos).effect == SingleGrid.Effect.Portal) {
                    //获取起止点的local坐标（相对于Grid）
                    Vector2Int from = new Vector2Int(pos.x, pos.y);
                    Vector2Int to = map.GetData(pos).GetPortal();
                    Vector3 from3 = tilemapManagerSpecial.tilemap.CellToLocal(new Vector3Int(from.x, from.y, 0));
                    Vector3 to3 = tilemapManagerSpecial.tilemap.CellToLocal(new Vector3Int(to.x, to.y, 0));

                    //绘制箭头
                    GameObject obj = (GameObject) Instantiate(GameObject.Find("Grid/PortalArrows/LineSample"));
                    LineRenderer line = obj.GetComponent<LineRenderer>();
                    line.SetPosition(0, from3);
                    line.SetPosition(1, to3);
                }
            }
        }

        //移除指定格子
        public void RemoveGrid(Vector2Int pos) {
            tilemapManagerBoard.EraseTile(pos);
            tilemapManagerSpecial.EraseTile(pos);
        }
    }
}