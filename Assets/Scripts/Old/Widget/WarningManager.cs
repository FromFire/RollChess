using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WarningManager : MonoBehaviour {
    //存放所有警告Model的列表
    public static List<WarningModel> errors=new List<WarningModel> ();
 
    [SerializeField]
    private WarningWindow window;   //警告窗口
 
    void Update()
    {
        if(errors.Count>0)
        {
            //取出列表的第一个
            WarningModel err=errors[0];
            //然后删除
            errors.RemoveAt(0);
            //最后显示
            window.active(err);
        }
    }
}