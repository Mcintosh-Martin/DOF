using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CurrentlyUsing curUse = CurrentlyUsing.None;
    public CurrentlyHovered curHov = CurrentlyHovered.None;
    public GameObject[] allText = new GameObject[5];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            allText[i] = transform.GetChild(0).transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
