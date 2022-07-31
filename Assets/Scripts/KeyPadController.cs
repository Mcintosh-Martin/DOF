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
    public Text displayText;
    public Material textMaterial;
    private bool wrongCombo = false;
    private bool rightCombo = false;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<KeyPadButton>();
            transform.GetChild(i).GetComponent<KeyPadButton>().buttonNum = i + 1;
        }

        enteredNum = new int[4];

        for (int i = 0; i < 4; i++)
            enteredNum[i] = 0;

        displayText.text = "";
    }

    public void activate(bool active)
    {
        if (active)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {

        }    
    }

    public void addNum(int number)
    {
        if(!rightCombo && !wrongCombo)
        {
            enteredNum[enteredIndex] = number;

            enteredString = string.Join("", enteredString + enteredNum[enteredIndex].ToString());

            if (enteredIndex == 3)
            {
                if (enteredString == correctString)
                {
                    rightCombo = true;
                    transform.GetChild(9).GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                }
                else
                {
                    wrongCombo = true;
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

        if(wrongCombo)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);

            if(timer >= 1.25)
            {
                for (int i = 0; i < 4; i++)
                    enteredNum[i] = 0;

                enteredIndex = 0;
                enteredString = "";

                Debug.Log(enteredString);
                displayText.text = enteredString;

                transform.GetChild(9).GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                wrongCombo = false;
            }
        }
    }
}
