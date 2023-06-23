using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CurrentlyUsing curUse = CurrentlyUsing.None;
    public CurrentlyHovered curHov = CurrentlyHovered.None;
    public GameObject[] hoveredText;
    public GameObject[] usingText;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        hoveredText = new GameObject[3];
        usingText = new GameObject[3];

        player = GameObject.FindGameObjectWithTag("Player");
        
        for(int i = 0; i < 3; i++)
        {
            //Pulls gameobject from sub folder /Text/UsingText
            usingText[i] = transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).gameObject;

            //Pulls gameobject from sub folder /Text/HoveredText
            hoveredText[i] = transform.GetChild(0).transform.GetChild(1).transform.GetChild(i).gameObject;
        }

        ClearHoveredText();
        ClearUsingText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (player.GetComponent<playerControl>().curHovered)
        {
            case CurrentlyHovered.None:
                ClearHoveredText();
                break;
            case CurrentlyHovered.Special:
                DisplayHoveredText(CurrentlyHovered.Special, 1);
                break;
            case CurrentlyHovered.Keypad:
                DisplayHoveredText(CurrentlyHovered.Keypad, 1);
                break;
            case CurrentlyHovered.Grid:
                DisplayHoveredText(CurrentlyHovered.Grid, 1);
                break;
            case CurrentlyHovered.LightPuzzle:
                DisplayHoveredText(CurrentlyHovered.LightPuzzle, 1);
                break;
            case CurrentlyHovered.LightBulb:
                DisplayHoveredText(CurrentlyHovered.LightBulb, 1);
                break;
            case CurrentlyHovered.LampBase:
                DisplayHoveredText(CurrentlyHovered.LampBase, 0);
                DisplayHoveredText(CurrentlyHovered.LampBase, 0);
                break;
        }

        switch (player.GetComponent<playerControl>().curUsing)
        {
            case CurrentlyUsing.None:
                ClearUsingText();
                
                break;
            case CurrentlyUsing.Special:
                DisplayUsingText(CurrentlyUsing.Special, 1);
                break;
            case CurrentlyUsing.Keypad:
                DisplayUsingText(CurrentlyUsing.Keypad, 2);
                break;
            case CurrentlyUsing.Grid:
                DisplayUsingText(CurrentlyUsing.Grid, 0);
                break;
            case CurrentlyUsing.LightPuzzle:
                DisplayUsingText(CurrentlyUsing.LightPuzzle, 2);
                break;
            case CurrentlyUsing.LightBulb:
                DisplayUsingText(CurrentlyUsing.LightBulb, 1);
                break;
            case CurrentlyUsing.usingLampBase:
                DisplayUsingText(CurrentlyUsing.usingLampBase, 1);
                break;
        }
    }

    private void ClearUsingText()
    {
        if (curUse != CurrentlyUsing.None)
        {
            Debug.Log("None");
            curUse = CurrentlyUsing.None;
            for (int i = 0; i < usingText.Length; i++)
                usingText[i].SetActive(false);
        }               
    }

    private void ClearHoveredText()
    {
        if (curHov != CurrentlyHovered.None)
        {
            Debug.Log("None");
            curHov = CurrentlyHovered.None;
            for (int i = 0; i < hoveredText.Length; i++)
                hoveredText[i].SetActive(false);
        } 
    }

    private void DisplayHoveredText(CurrentlyHovered hovered, int index)
    {
        GameObject lampBase = GameObject.FindGameObjectWithTag("LampBase");
        if (lampBase.GetComponent<LampBaseController>().hasBulb)
            hoveredText[0].GetComponent<Text>().text = "Press E to REMOVE Bulb";
        else
            hoveredText[0].GetComponent<Text>().text = "Press E to ADD Bulb";

        if (lampBase.GetComponent<LampBaseController>().powerOn)
            hoveredText[1].GetComponent<Text>().text = "Press R to Turn OFF Lamp";
        else
            hoveredText[1].GetComponent<Text>().text = "Press R to Turn ON Lamp";

        if (curHov != hovered)
        {
            curHov = hovered;

            if (index == 0)
            {
                hoveredText[0].SetActive(true);
                hoveredText[1].SetActive(true);
            }
            else
                hoveredText[2].SetActive(true);            
        
            Debug.Log(curHov);
        }
    }

    private void DisplayUsingText(CurrentlyUsing curUsage, int index)
    {
        if (curUse != curUsage)
        {
            curUse = curUsage;
            usingText[index].SetActive(true);
            Debug.Log(curUse);
        }
    }
}
