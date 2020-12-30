using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;


public class Popup : MonoBehaviour {

    // 块的名称
    public Text title;
    // 块的描述
    public Text describe;
    // 块的效果
    public Text effectIntro;

    // 是否开启弹窗
    public bool available = false;

    // 弹窗时间间隔
    public float delay;

    // 用于获取鼠标所在的坐标
    public Cursor cursor;

    // 无穷远处
    Vector3 FAR_AWAY = new Vector3(3000, 3000, 3000);

    private void Start() {
        Hide();
    }

    void Update() {
        if (available && cursor.GetStayDuration() > delay) {
            Show();
        }
        else {
            Hide();
        }
    }

    //显示
    public void Show() {
        SetPosition(cursor.GetMousePosition());
    }

    //隐藏
    public void Hide() {
        gameObject.transform.position = FAR_AWAY;
    }

    // 设置名称
    public string Title {
        get { return title.text; }
        set { title.text = value; }
    }

    // 设置描述
    public string Describe {
        get { return describe.text; }
        set { describe.text = value; }
    }

    // 设置特性介绍
    public string EffectIntro {
        get { return effectIntro.text; }
        set { effectIntro.text = value; }
    }

    // 根据绝对坐标来修改弹窗位置
    public void SetPosition(Vector3 position) {
        transform.position=position;
    }
}