using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//棋子类：单个棋子
public class Token : MonoBehaviour{

    int x;
    int y;

    public Token(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vector2Int getXY() {
        return new Vector2Int(x,y);
    }

    public void setXY(Vector2Int xy) {
        x = xy.x;
        y = xy.y;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
