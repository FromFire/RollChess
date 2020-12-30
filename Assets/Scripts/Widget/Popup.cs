using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;


public class Popup : MonoBehaviour {

    // 块的名称
    public Text title;
    // 块的介绍
    public Text introText;
    // 块的效果
    public Text effectIntro;

    // 是否开启弹窗
    public bool popupVisible = false;

    // 弹窗时间间隔
    public float popupDelay;

    // 用于获取鼠标所在的坐标
    public Cursor cursor;

    Vector3 FAR_AWAY = new Vector3(3000, 3000, 3000);

    private void Start() {
        Hide();
    }

    void Update() {
        if (popupVisible && cursor.GetStayDuration() > popupDelay) {
            Show();
        }
        else {
            Hide();
        }
    }

    public void Show() {
        SetPosition(cursor.GetMousePosition());
    }

    public void Hide() {
        gameObject.transform.position = FAR_AWAY;
    }

    public string Title {
        get { return title.text; }
        set { title.text = value; }
    }

    public string IntroText {
        get { return introText.text; }
        set { introText.text = value; }
    }

    public string EffectIntro {
        get { return effectIntro.text; }
        set { effectIntro.text = value; }
    }

    // 根据绝对坐标来修改弹窗位置
    public void SetPosition(Vector3 position) {
        transform.position=position;
    }
}