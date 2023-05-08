using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public int LightNum = 0;

    private float countDown = 0;
    private bool count = false;

    private bool turnOn = false;
    private float timeOn = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(count)
        {
            countDown += Time.deltaTime;

            if(countDown >= 0.25f)
            {
                GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                countDown = 0;
                count = false;
            }
        }

        if(turnOn)
        {
            countDown += Time.deltaTime;

            if(countDown >= timeOn)
            {
                GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                timeOn = 0f;
                turnOn = false;
                countDown = 0;
            }
        }
    }
    public void refresh()
    {
        countDown = 0;
        count = false;
        turnOn = false;
        timeOn = 0f;
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
    private void OnMouseDown()
    {
        if(GetComponentInParent<FlashLightPuzzleController>().cutOff)
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponentInParent<FlashLightPuzzleController>().ButtonInput(LightNum);
        }
    }

    private void OnMouseUp()
    {
        if (GetComponentInParent<FlashLightPuzzleController>().cutOff)
            count = true;
        //GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    public void LightUp(float length)
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        turnOn = true;
        timeOn = length;
    }
}
