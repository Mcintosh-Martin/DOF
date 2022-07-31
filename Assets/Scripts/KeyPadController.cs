using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyPadController : MonoBehaviour
{

    private int[] enteredNum;
    private int enteredIndex = 0;

    private string enteredString = "";
    private string correctString = "1234";
    
    //RenderTexture Text
    public Text displayText;

    //0 == not assessed, 1 == Wrong, 2 == Correct
    private int comboResults = 0;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<KeyPadButton>();
            transform.GetChild(i).GetComponent<KeyPadButton>().SetButtonNum(i + 1);
            transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }

        enteredNum = new int[4];

        ClearDisplay();
    }

    public void activate(bool active)
    {
        if (active)
        {
            GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < 9; i++)
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;

            transform.GetChild(11).GetComponent<Camera>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < 9; i++)
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;

            transform.GetChild(11).GetComponent<Camera>().enabled = false;
        }    
    }

    public void addNum(int number)
    {
        if(comboResults == 0)
        {
            enteredNum[enteredIndex] = number;

            enteredString = string.Join("", enteredString + enteredNum[enteredIndex].ToString());

            if (enteredIndex == 3)
            {
                if (enteredString == correctString)
                {                 
                    comboResults = 2;
                    transform.GetChild(9).GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                }
                else
                {
                    comboResults = 1;
                    transform.GetChild(9).GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                    timer = 0;
                }
            }
            else
            {
                enteredIndex++;
            }

            Debug.Log(enteredString);
            displayText.text = enteredString;  
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if(comboResults == 1)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);

            if(timer >= 1.25)
            {
                ClearDisplay();

                transform.GetChild(9).GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                comboResults = 0;
            }
        }
        else
            if(Input.GetMouseButton(1))
                ClearDisplay();           
    }

    private void ClearDisplay()
    {
        for (int i = 0; i < 4; i++)
            enteredNum[i] = 0;

        enteredIndex = 0;
        enteredString = "";

        //Debug.Log(enteredString);
        displayText.text = enteredString;
    }
}
