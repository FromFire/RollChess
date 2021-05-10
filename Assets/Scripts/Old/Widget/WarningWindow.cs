using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WarningWindow : MonoBehaviour {
    [SerializeField]
    private Text text;    //NGUI
    //private Text text;     //UGUI
    //用于接收model的result方法
    WarningResult result;
    //使Window显示出来  如果有需要延迟消失   就delay后消失
    public void active(WarningModel value)
    {
        text.text=value.value;
        this.result=value.result;
        //如果WarningModel设置了延迟时间
        if(value.delay>0)
        {
            //delay时间后执行close函数
            Invoke("close",value.delay);
        }   
        gameObject.SetActive(true);
    }
    //关闭Window   如果有需要运行的方法就运行
    public void close()
    {
        //close函数是否正待等候调用   很明显他已经调用了  现在要删除它
        if(IsInvoking("close"))
        {
            //取消调用
            CancelInvoke("close");
        }
        gameObject.SetActive(false);
        //看看是否有需要执行的函数
        if(result!=null)
        {
            result();
        }
    }
}


//声明一个警告委托  用于在弹出警告的同时运行其他程序的方法  
public delegate void WarningResult();
public class WarningModel {
    //声明该方法委托
    public WarningResult result;

    //需要显示的文字
    public string value;

    //延迟多久自动关闭
    public float delay;

    //警告模型
    public WarningModel(string value,WarningResult result=null,float delay=-1)
    {
        this.value=value;
        this.result=result;
        this.delay=delay;
    }
}