using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaText : MonoBehaviour
{
    private Text textObject;

    public Color fullAlpha = new Color32(159, 253, 50, 255);

    private float curAlpha = 0f;

    public float fraction = 0;
    public float speed = 0.1f;

    enum CurState { Full, None, Rest }
    CurState curState = CurState.Rest;

    // Start is called before the first frame update
    void Start()
    {
        textObject = transform.GetChild(0).GetChild(0).GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        switch(curState)
        {
            case CurState.Full:
                changeAlpha(1);
                break;
            case CurState.None:
                changeAlpha(0);
                break;
            case CurState.Rest:
                break;
        }
    }

    ///<Summary>
    /// 0 = rest | 1 = Full Alpha | 2 = Zero Alpha
    ///</Summary>
    public void changeState(int state)
    {
        switch(state)
        {
            case 1:
                curState = CurState.Full;
                break;
            case 2:
                curState = CurState.None;
                break;
        }
        curAlpha = textObject.color.a;
        fraction = 0;
        
    }

    private void changeAlpha(float finalAlpha)
    {
        
        if (fraction <= 1)
        {
            fraction += Time.deltaTime * speed;
            textObject.color = new Color(fullAlpha.r, fullAlpha.g, fullAlpha.b, Mathf.Lerp(curAlpha, finalAlpha, fraction));
        }
        if (textObject.color.a == finalAlpha)
        {
            curState = CurState.Rest;
            fraction = 0;
        }
    }
}
