using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家类，事件触发器
public class Player : MonoBehaviour
{
    // 规则类
    Rule rule;

    // 玩家总数
    int totalPlayer;
    // 当前正在操作的玩家
    int nowPlayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        //初始化Rule
        rule = GameObject.Find("/Rule").GetComponent<Rule>();

        //读取地图json文件
        string filename = "MapSample";
        BoardEntity boardEntity = rule.loadMapFromJson(filename);

        //初始化玩家人数
        totalPlayer = boardEntity.players.number;
    }

    // Update is called once per frame
    void Update()
    {
        //鼠标左键点击，检测点击到的格子
        if (Input.GetMouseButtonDown(0))
        {
            //获取点击坐标（世界坐标）
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);

            //点击高亮格子，再次点击其他格子或空白部分取消高亮
            //若该格子上有棋子，则选中棋子，显示它可以到达的位置
            rule.chooseGrid(loc);

            //判定是否移动成功
            //若移动成功，切换控制权到下一个玩家
            if(rule.status == Rule.Status.moved) {
                //下一位玩家
                nextPlayer();
                rule.nowPlayer = nowPlayer;
                Debug.Log("回合结束，下一位玩家："+nowPlayer);
                //更新rule的状态
                rule.status = Rule.Status.waiting;
            }
        }
    }

    public void nextPlayer() {
        nowPlayer = (nowPlayer+1) % totalPlayer;
    }
}
