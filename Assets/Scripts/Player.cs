using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//玩家类，事件触发器
public class Player : MonoBehaviour
{

    Rule rule;

    // Start is called before the first frame update
    void Start()
    {
        rule = GameObject.Find("/Rule").GetComponent<Rule>();
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

            //如果已有棋子被选中，而此次点击的是该棋子能到达的位置，则把棋子移动过去

        }
    }
}
