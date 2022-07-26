using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryUIUpdate : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject player;

    private bool buttonPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        buttons = new GameObject[3];

        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = transform.GetChild(i).gameObject;
            buttons[i].GetComponent<Button>().enabled = false;
        }

        buttons[0].GetComponent<Button>().onClick.AddListener(delegate { buttonOneClick(0); });
        buttons[1].GetComponent<Button>().onClick.AddListener(delegate { buttonOneClick(1); });
        buttons[2].GetComponent<Button>().onClick.AddListener(delegate { buttonOneClick(2); });

        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Assign()
    {
        buttonPressed = false;

        GameObject[] inventory = player.gameObject.GetComponent<playerControl>().inventory;

        for (int i = 0; i < inventory.Length; i++)
        {
            if(inventory[i])
            {
                buttons[i].GetComponent<Button>().enabled = true;
                buttons[i].GetComponent<Image>().sprite = inventory[i].GetComponent<MatChange>().uiPortrait;
            }
        }
    }

    private void buttonOneClick(int index)
    {
        buttonPressed = !buttonPressed;

        Debug.Log($"You Click button: {index + 1} ");

        player.GetComponent<playerControl>().useInventoryItem(index, buttonPressed);

        //enable/disable non clicked buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i != index)
                buttons[i].GetComponent<Button>().enabled = !buttonPressed;

        }
    }
}
