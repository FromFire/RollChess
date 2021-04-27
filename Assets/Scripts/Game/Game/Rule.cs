using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using PlayerChoices = Structure_old.PlayerChoices;

//规则类：管理棋子和棋盘/其他棋子的互动，包括高亮可到达格子、吃子判断等等
public class Rule : MonoBehaviour {
    //棋子数量默认是4
    int PLAYERNUMBER = Structure_old.Constants.PLAYERNUMBER;

    //HUD层
    public HUD hud;

    // 初始化全局
    void Start()
    {
        //无Entrance情况下的默认值，用于调试Game场景
        string filename = "Maps/FourPlayers";
        // 默认2玩家，一定不会出错
        playerChoices = new List<PlayerChoices> {PlayerChoices.Player, PlayerChoices.Player, PlayerChoices.Banned, PlayerChoices.Banned};

        //读取Entrance传来的Message
        if(GameObject.Find("MessageToGame") != null) {
            Message message = GameObject.Find("MessageToGame").GetComponent<Message>();
            // 获取地图
            filename = "Maps/" + message.GetMessage<string> ("mapFilename");
            // 获取玩家数量
            playerChoices = message.GetMessage<List<PlayerChoices>> ("playerChoice");
            // 销毁Message，避免它被重复创建
            GameObject.Destroy(message.gameObject);
        }
        
        //读取地图json文件
        BoardEntity boardEntity = LoadMapFromJson(filename);

        //初始化玩家信息
        nowPlayer = 0;

        //初始化board
        board.Init(boardEntity.map, boardEntity.special, boardEntity.portal);

        //初始化tokenSet
        tokenSet.Init(boardEntity.tokens);
        for(int i=0; i<playerChoices.Count; i++) {
            if(playerChoices[i] == PlayerChoices.Banned) {
                tokenSet.removePlayer(i);
            }
        }
    }

    //查询是否有获胜的玩家，若没有，返回-1
    //若只有一个玩家棋子数不为0，则该玩家获胜
    public int FindWinner() {
        int winnerCandidate = -1; //可能赢（棋子数）的玩家序号
        for(int i=0; i<PLAYERNUMBER; i++) {
            if(tokenSet.GetTokenNumber(i) != 0) {
                //若已查询的所有玩家棋子数均为0，此玩家可能会赢
                if(winnerCandidate == -1) {
                    winnerCandidate = i;
                } 
                //若在此人之前有玩家棋子数不为0，则无人获胜
                else {
                    return -1;
                }
            }
        }
        return winnerCandidate;
    }

    //掷骰子
    //是RollButton的OnClick函数
    public void RollDice() {
        //生成随机数
        step = new System.Random().Next(6)+1;
        Debug.Log(step);

        //隐藏按钮
        hud.ShowRollStep(step);
    }
    
    // TODO: 回调RollDice给HUD


    //移动棋子
    public void Move(Vector2Int from, Vector2Int to, List<Vector2Int> route) {
        Debug.Log("move: ("+ from.x + "." + from.y + ") -> (" + to.x + "." + to.y + ") ");

        //若目的点是传送门，将其传送
        //传送门须放在走子前面，否则会有BUG：B站在传送站处，A踩到传送站，传送前会吃掉B
        if(board.GetEffect(to) == SingleGrid.Effect.Portal) {
            Vector2Int target = board.GetPortalTarget(to);
            tokenSet.MoveToken(from, target);
        } else {
            tokenSet.MoveToken(from, to);
        }

        //检测危桥
        board.DetectBrokenBridge(route);

        //修改状态为moved
        status = Status.moved;

        //显示roll点按钮，隐藏步数按钮
        hud.ShowRollButton();
    }

   
    
    // TODO: 统一命名 ChooseGrid{ ChooseTokenSource(AddChoose), ChooseTokenTarget(Move) }

    


}
