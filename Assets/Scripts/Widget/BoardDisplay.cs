using System.Collections.Generic;
using Structure;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Widget {
    public class BoardDisplay : MonoBehaviour {
        //显示地板
        public TilemapManager tilemapManagerBoard;

        //显示特殊格子贴图
        public TilemapManager tilemapManagerSpecial;

        //显示传送门的箭头
        public GameObject portalArrows;

        //传送门的样式案例
        public GameObject arrowSample;

        //摄像头
        public CameraController cameraController;

        // 数据
        BaseBoard<SingleGrid> map;

        public Cursor cursor;

        SpecialIntroductionsEntity specialIntroductionsEntity;

        // 弹窗
        public Popup popup;

        //显示自身
        public void Display(BaseBoard<SingleGrid> map) {
            //获取有效数据列表
            this.map = map;
            HashSet<Vector2Int> keyInfo = map.ToPositionsSet();
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
                SingleGrid.Effect effect = map.GetData(pos).SpecialEffect;
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
            foreach (Vector2Int pos in poses) {
                if (map.GetData(pos).SpecialEffect == SingleGrid.Effect.Portal) {
                    //获取起止点的local坐标（相对于Grid）
                    Vector2Int from = new Vector2Int(pos.x, pos.y);
                    Vector2Int to = map.GetData(pos).PortalTarget;
                    Vector3 from3 = tilemapManagerSpecial.tilemap.CellToLocal(new Vector3Int(from.x, from.y, 0));
                    Vector3 to3 = tilemapManagerSpecial.tilemap.CellToLocal(new Vector3Int(to.x, to.y, 0));

                    //绘制箭头
                    GameObject obj = Object.Instantiate(arrowSample);
                    obj.transform.parent = portalArrows.transform;
                    LineRenderer line = obj.GetComponent<LineRenderer>();
                    DrawCurve(from3, (from3+to3)/2 + 2*Vector3.up, to3, line);
                }
            }

            // 获取格子介绍
            TextAsset text = Resources.Load<TextAsset>("Texts/SpecialIntroductions");
            string json = text.text;
            Debug.Log(json);
            specialIntroductionsEntity = JsonHelper.FromJson<SpecialIntroductionsEntity> (json);
        }

        // 画贝塞尔曲线
        void DrawCurve (Vector3 point1,Vector3 point2,Vector3 point3,LineRenderer MyL) {
            int vertexCount = 30;//采样点数量
	        List<Vector3> pointList = new List<Vector3> ();
 
	        for (float ratio = 0; ratio <= 1; ratio +=1.0f/ vertexCount)
	        {
		        Vector3 tangentLineVertex1 = Vector3.Lerp (point1, point2, ratio);
		        Vector3 tangentLineVectex2 = Vector3.Lerp (point2, point3, ratio);
		        Vector3 bezierPoint = Vector3.Lerp (tangentLineVertex1, tangentLineVectex2, ratio);
		        pointList.Add (bezierPoint);
	        }
	        MyL.positionCount = pointList.Count;
	        MyL.SetPositions (pointList.ToArray());
        }

        void Update() {
            // 获取屏幕中心的Tilemap坐标
            Vector3 screenCenterWorld = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/2,Screen.height/2,0));
            Vector2Int screenCenter = tilemapManagerBoard.WorldToCell(screenCenterWorld);

            // 判定是否已超过地图边界，若越界，则不允许向那个方向继续滑动
            cameraController.allowMoveLeft = screenCenter.x > map.BorderLeft;
            cameraController.allowMoveRight = screenCenter.x < map.BorderRight;
            cameraController.allowMoveUp = screenCenter.y < map.BorderUp;
            cameraController.allowMoveDown = screenCenter.y > map.BorderDown;

            // 当鼠标所在格子是特殊格子时，通知PopUp
            SingleGrid.Effect pointedEffect = map.GetData(cursor.GetPointedCell()).SpecialEffect;
            if(pointedEffect != SingleGrid.Effect.None) {
                popup.popupVisible = true;
                string effectName = "";
                switch(pointedEffect) {
                    case SingleGrid.Effect.DoubleStep:
                        effectName = "doubleStep";
                        break;
                    case SingleGrid.Effect.BrokenBridge:
                        effectName = "brokenBridge";
                        break;
                    case SingleGrid.Effect.Portal:
                        effectName = "portal";
                        break;
                }

                for(int i=0; i<specialIntroductionsEntity.SpecialIntroductions.Count; i++) {
                    if(specialIntroductionsEntity.SpecialIntroductions[i].name == effectName) {
                        popup.Title = specialIntroductionsEntity.SpecialIntroductions[i].name;
                        popup.IntroText = specialIntroductionsEntity.SpecialIntroductions[i].introText;
                        popup.EffectIntro = specialIntroductionsEntity.SpecialIntroductions[i].effectText;
                    }
                }
            } else {
                popup.popupVisible = false;
            }
        }

        //移除指定格子
        public void RemoveGrid(Vector2Int pos) {
            tilemapManagerBoard.EraseTile(pos);
            tilemapManagerSpecial.EraseTile(pos);
        }
    }
}