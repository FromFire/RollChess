using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;


public class Popup : MonoBehaviour {
    public Text text;

    private void Start() {
        Hide();
        // GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f, 0.1f);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetText(string str) {
        text.text = str;
    }

    // 根据绝对坐标来修改弹窗位置
    public void SetPosition(Vector3 position) {
        transform.position=position;
    }
}