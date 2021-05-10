using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 负责页面的滚动处理，目前只能横向滚动
/// </summary>
public class Scroller : MonoBehaviour
{
    //滑动主体
    public ScrollRect scrollRect;

    //是否需要滑动页面
    bool needSlide = false;

    //每次滑动的距离
    float slideDistanceEachFrame;

    //滑动所需帧数
    int slideRestFrames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(needSlide) {
            SlidePerFrame();
        }
    }

    //ScrollView滑动到指定位置
    public void SlideTo(float position) {
        needSlide = true;
        slideRestFrames = 60;
        slideDistanceEachFrame = (position - scrollRect.horizontalNormalizedPosition) / slideRestFrames;
    }

    //滑动时每帧的处理函数
    void SlidePerFrame() {
        if(slideRestFrames >= 1) {
            slideRestFrames -= 1;
            scrollRect.horizontalNormalizedPosition += slideDistanceEachFrame;
        } else {
            needSlide = false;
        }
    }
}
