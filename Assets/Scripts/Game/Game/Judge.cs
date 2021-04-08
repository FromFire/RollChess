using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using PlayerChoices = Structure_old.PlayerChoices;

//裁判类：管理对局信息，包括回合计数、胜负判断、记录操作
public class Judge : MonoBehaviour
{
    // 规则类
    public Rule rule;

    //HUD层
    public HUD hud;

    // 玩家总数
    int totalPlayer;
    // 当前正在操作的玩家
    int nowPlayer;

    //回合数
    int turnCount;

    //是否游戏结束
    bool isGameOver = false;

    // 玩家操控状态
    public List<PlayerChoices> playerChoices;

    // Start is called before the first frame update
    void Start()
    {
        //初始化玩家人数
        totalPlayer = Structure_old.Constants.PLAYERNUMBER;
        nowPlayer = 0;

        //初始化玩家操控状态
        playerChoices = rule.playerChoices;

        //初始化回合数
        turnCount=1;
    }

    // Update is called once per frame
    void Update()
    {
        //游戏已结束，Update失效
        if(isGameOver) {
            return;
        }

        // 避免与UI按键冲突
        if (Cursor.isOverUI()) {
            return;
        }

        //鼠标左键点击，检测点击到的格子
        if (Input.GetMouseButtonDown(0))
        {
            //获取点击坐标（世界坐标）
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);

            //点击高亮格子，再次点击其他格子或空白部分取消高亮
            //若该格子上有棋子，则选中棋子，显示它可以到达的位置
            rule.ChooseGrid(loc);

            //判定是否移动成功
            //若移动成功，切换控制权到下一个玩家
            if(rule.status == Rule.Status.moved) {
                //回合数记录
                int prePlayer = nowPlayer;
                //下一位玩家
                NextPlayer();
                rule.nowPlayer = nowPlayer;
                hud.UpdateActionPlayer(nowPlayer);
                Debug.Log("回合结束，下一位玩家："+nowPlayer);
                //更新rule的状态
                rule.status = Rule.Status.waiting;
                //胜负判定
                int winner = rule.FindWinner();
                if(winner != -1) {
                    GameOver(winner);
                    isGameOver = true;
                    return;
                }
                //更新回合数显示
                if(nowPlayer < prePlayer) {
                    turnCount++;
                    hud.UpdateTurn(turnCount);
                }
            }
        }
    }

    //切换到下一位玩家，跳过状态为Banned的角色
    public void NextPlayer() {
        do {
            nowPlayer = (nowPlayer+1) % totalPlayer;
        } while(playerChoices[nowPlayer] == PlayerChoices.Banned);
    }

    //有玩家获胜，结束游戏
    public void GameOver(int winner) {
        hud.ShowGameReview(winner);
    }
}
