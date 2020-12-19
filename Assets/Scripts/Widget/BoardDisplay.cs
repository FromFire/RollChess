﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardDisplay : MonoBehaviour
{
    //显示地板
    Tilemap tilemapBoard;

    //显示特殊格子贴图
    Tilemap tilemapSpecial;

    //显示传送门的箭头
    GameObject portalArrows;

    //存储各类Tile的集合
    List<Tile> tileList;

    //Tile在tileList中存储的顺序
    enum TileKeys{floorLawnGreen, //绿色地板
        special_brokenBridge, //危桥
        special_doubleStep, //步数翻倍
        special_portal //传送门
    };

    // Start is called before the first frame update
    void Start()
    {
        // tile顺序按照enum tileKeys中规定的来
        List<string> tileNames = new List<string> {
            "Tiles/floor-lawnGreen",    //floorLawnGreen
            "Tiles/special-brokenBridge",  //special_brokenBridge
            "Tiles/special-doubleStep",    //special_doubleStep
            "Tiles/special-portal",    //special_portal
        };

        // 读取所有tile
        tileList = new List<Tile> ();
        foreach(string name in tileNames) {
            tileList.Add(Resources.Load<Tile>(name));
        }
    }

    //显示自身
    public void Display(BaseBoard<SingleGrid> map) {
        //获取有效数据列表
        HashSet<Vector2Int> keyInfo = map.ToList();
        HashSet<Vector2Int> poses = new HashSet<Vector2Int>();
        foreach(Vector2Int pos in keyInfo) {
            //去除不可走的坐标
            if(map.GetData(pos).walkable == true) {
                poses.Add(pos);
            }
        }

        //显示TilemapBoard层，地图格子
        tilemapBoard = GameObject.Find("/Grid/TilemapBoard").GetComponent<Tilemap>();
        foreach(Vector2Int pos in poses) {
            tilemapBoard.SetTile(new Vector3Int(pos.x, pos.y, 0), tileList[(int)TileKeys.floorLawnGreen]);
        }

        //显示TilemapSpecial层，特殊格子效果
        tilemapSpecial = GameObject.Find("/Grid/TilemapSpecial").GetComponent<Tilemap>();
        foreach(Vector2Int pos in poses) {
            SingleGrid.Effect effect = map.GetData(pos).effect;
            switch(effect) {
                case SingleGrid.Effect.doubleStep:
                    tilemapSpecial.SetTile(new Vector3Int(pos.x, pos.y, 0), tileList[(int)TileKeys.special_doubleStep]);
                    break;
                case SingleGrid.Effect.brokenBridge:
                    tilemapSpecial.SetTile(new Vector3Int(pos.x, pos.y, 0), tileList[(int)TileKeys.special_brokenBridge]);
                    break;
                case SingleGrid.Effect.portal:
                    tilemapSpecial.SetTile(new Vector3Int(pos.x, pos.y, 0), tileList[(int)TileKeys.special_portal]);
                    break;
            }
        }

        //显示传送门之间的箭头
        portalArrows = GameObject.Find("/Grid/PortalArrows");
        foreach(Vector2Int pos in poses) {
            if( map.GetData(pos).effect == SingleGrid.Effect.portal) {
                //获取起止点的local坐标（相对于Grid）
                Vector2Int from = new Vector2Int(pos.x, pos.y);
                Vector2Int to = map.GetData(pos).GetPortal();
                Vector3 from3 = tilemapSpecial.CellToLocal(new Vector3Int(from.x, from.y, 0));
                Vector3 to3 = tilemapSpecial.CellToLocal(new Vector3Int(to.x, to.y, 0));

                //绘制箭头
                GameObject obj = (GameObject)Instantiate(GameObject.Find("Grid/PortalArrows/LineSample"));
                LineRenderer line = obj.GetComponent<LineRenderer>();
                line.SetPosition(0, from3);
                line.SetPosition(1, to3);
            }
        }
    }

    //移除指定格子
    public void RemoveGrid(Vector2Int pos) {
        tilemapBoard.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
        tilemapSpecial.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}