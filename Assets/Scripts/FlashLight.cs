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

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                GetComponentInParent<FlashLightPuzzleController>().ButtonInput(LightNum);
                audioSource.Stop();
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
                audioSource.Stop();
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
        audioSource.Stop();
    }
    private void OnMouseDown()
    {
        if(GetComponentInParent<FlashLightPuzzleController>().cutOff)
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            audioSource.Play();
        }                    
    }

    private void OnMouseUp()
    {
        if (GetComponentInParent<FlashLightPuzzleController>().cutOff)
            count = true;
    }

    public void LightUp(float length)
    {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        turnOn = true;
        timeOn = length;
        audioSource.Play();
    }
}
