using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightPuzzleController : MonoBehaviour
{
    //-------------------------------------------------------------PUBLIC START-----------------------------------------------------
    //public int[] lightRandComboOne = new int[4];
    //public int[] lightRandComboTwo = new int[4];
    //public int[] lightRandComboThree = new int[4];
    //public int[] curArr;
    public int[,] testArr;

    public int[] lightComboEntered = new int[4];

    public int score = 0;
    public GameObject[] gameLights;
    public GameObject[] scoreLights;

    public Camera cam;
    public bool cutOff = true;
    public bool activated = false;
    public bool completed = false;
    //-------------------------------------------------------------PRIVATE START----------------------------------------------------
    private float dt = 1;
    private int index = 0;
    private int enteredIndex = 0;
    private int section = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        testArr = new int[3, 4];
        for(int i = 0; i < 3; i++)
            for(int j = 0; j < 4; j++)
                testArr[i, j] = Random.Range(0, 4);        

        for(int i = 0; i < 4 ; i++)
        {
            gameLights[i].AddComponent<FlashLight>();
            gameLights[i].GetComponent<FlashLight>().LightNum = i;
            gameLights[i].GetComponent<FlashLight>().enabled = false;
            gameLights[i].GetComponent<CapsuleCollider>().enabled = false;

            lightComboEntered[i] = 99;
        }
    }

    public void activate(bool active)
    {
        if (active)
        {
            GetComponent<BoxCollider>().enabled = false;
            cam.enabled = true;

            for (int i = 0; i < 4; i++)
            {
                gameLights[i].GetComponent<CapsuleCollider>().enabled = true;
                gameLights[i].gameObject.GetComponent<FlashLight>().enabled = true;
            }

            cutOff = false;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;
            cam.enabled = true;

            for (int i = 0; i < 4; i++)
            {
                gameLights[i].GetComponent<CapsuleCollider>().enabled = false;
                gameLights[i].gameObject.GetComponent<FlashLight>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activated && !completed)
            SectionLight(); 
        
        DisplayScore();
    }

    //-------------------------------------------------------------PUBLIC START-----------------------------------------------------
    public void ButtonInput(int num)
    {
        lightComboEntered[enteredIndex] = num;
        enteredIndex++;

        if (enteredIndex >= 4)
        {
            bool fullMatch = true;

            for(int i = 0; i < 4; i++)
                if (lightComboEntered[i] != testArr[section, i])
                {
                    fullMatch = false;
                    break;
                }
            
            if(fullMatch)
            {
                section++;
                score++;
                if(score == 3){ completed = true; }
            }
            else
            {
                section = 0;
                score = 0;
            }

            //Refresh all vars to base for next interaton
            cutOff = false;
            enteredIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                gameLights[i].GetComponent<FlashLight>().refresh();
                lightComboEntered[i] = 99;
            }
        }
    }
    //-------------------------------------------------------------PUBLIC END-------------------------------------------------------

    //-------------------------------------------------------------PRIVATE START----------------------------------------------------
    private void SectionLight(/*int section, int[] arr*/)
    {
        if (!cutOff)
        {
            dt += Time.deltaTime;

            //1 sec for light on and 0.25 for time off
            if (dt >= 1.25f)
            {
                gameLights[testArr[section,index]].GetComponent<FlashLight>().LightUp(1);

                index++;
                dt = 0;

                if (index >= 4)
                {
                    index = 0;
                    cutOff = true;
                    dt = 0;
                }
            }
        }
    }
    private void randOrderArray(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            bool match = false;
            while (!match)
            {
                bool breakout = false;
                arr[i] = Random.Range(0, arr.Length);

                for (int j = 0; j < arr.Length; j++)
                    if (i != j)
                        if (arr[i] == arr[j])
                        {
                            breakout = true;
                            break;
                        }

                if (!breakout)
                    match = true;
            }
            Debug.Log(arr[i]);
        }
    }
    private void DisplayScore()
    {
        if (score > 0)
            for (int i = 0; i < score; i++)
                scoreLights[i].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        if (score == 0)
            for (int i = 0; i < 3; i++)
                scoreLights[i].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }
    //-------------------------------------------------------------PRIVATE END------------------------------------------------------
}
