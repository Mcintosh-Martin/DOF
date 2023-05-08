using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LightBaseUI : MonoBehaviour
{
    public GameObject[] buttons = new GameObject[2];
    private GameObject lampBase;
    private bool buttonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        lampBase = GameObject.FindGameObjectWithTag("LampBase");

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).gameObject;
            buttons[i].GetComponent<Button>().enabled = false;

            int index = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(delegate { ButtonClick(index); });
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Assign()
    {
        buttonPressed = false;

        GameObject[] bulbs = lampBase.gameObject.GetComponent<LampBaseController>().bulbs;

        for (int i = 0; i < bulbs.Length; i++)
        {
            if (bulbs[i])
            {
                buttons[i].GetComponent<Button>().enabled = true;
                buttons[i].transform.GetChild(1).GetComponent<Image>().sprite = bulbs[i].GetComponent<UIDetails>().uiPortrait;
                buttons[i].transform.GetChild(0).GetComponent<Text>().text = bulbs[i].GetComponent<UIDetails>().uiText;
            }
        }
    }

    private void ButtonClick(int index)
    {
        buttonPressed = !buttonPressed;

        Debug.Log($"You Click button: {index + 1} ");

        lampBase.GetComponent<LampBaseController>().bulbSelect(index);
    }
}
