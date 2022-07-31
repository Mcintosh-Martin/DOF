using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadButton : MonoBehaviour
{
    public int buttonNum;

    private void OnMouseDown()
    {
        transform.GetComponentInParent<KeyPadController>().addNum(buttonNum);
    }
    public void SetButtonNum(int value)
    {
        buttonNum = value;
    }
}
