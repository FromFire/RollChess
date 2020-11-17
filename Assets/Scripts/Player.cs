﻿using System.Collections;
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
            //点击高亮格子，再次点击其他格子或空白部分取消高亮
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 loc = ray.GetPoint(-ray.origin.z / ray.direction.z);
            rule.chooseGrid(loc);
        }
    }
}
