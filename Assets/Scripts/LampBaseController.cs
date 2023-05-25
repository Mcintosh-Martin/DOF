using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LampBaseController : MonoBehaviour
{
    public GameObject player;
    
    public bool hasBulb = false;
    public Canvas pickBulb;

    public GameObject[] bulbs = new GameObject[2];

    public GameObject lightBulb;

    public bool powerOn = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// True starts functionality to add a bulb to base, False is for removing bulb
    /// </summary>
    public void AddRemoveBulb()
    {
        hasBulb = !hasBulb;
        if(hasBulb)
        {
            CheckInvForItems();
            pickBulb.transform.GetChild(0).GetComponent<LightBaseUI>().Assign();
            pickBulb.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            lightBulb.GetComponent<BulbControl>().setMovement(1);
            lightBulb.GetComponent<BulbControl>().GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void CheckInvForItems()
    {   
        GameObject[] invTemp = player.GetComponent<playerControl>().inventory;

        int index = 0;
        for (int i = 0; i < invTemp.Length; i++)
        {
            if (invTemp[i] && invTemp[i].tag == "Bulb")
            {
                bulbs[index] = invTemp[i];
                index++;
            }             
        }
        
    }

    public void bulbSelect(int index)
    {
        //Disable The canvas
        pickBulb.GetComponent<Canvas>().enabled = false;

        //Add the bulb gameobject to lamp storage
        lightBulb = bulbs[index];

        //Remove from the players inv
        for (int i = 0; i < player.GetComponent<playerControl>().inventory.Length; i++)
            if (player.GetComponent<playerControl>().inventory[i] && player.GetComponent<playerControl>().inventory[i] == lightBulb)
                player.GetComponent<playerControl>().inventory[i] = null;
                
        //Repostion the light
        lightBulb.transform.position = new Vector3(-22.66643f, -1.42f, -26.33217f);

        //Carry out the animation
        lightBulb.GetComponent<BulbControl>().setMovement(2);

        //Disable Bulbs Collider
        lightBulb.GetComponent<Collider>().enabled = false;

        //allow player movement and hide cursor
        player.GetComponent<playerControl>().curUsing = CurrentlyUsing.None;
        player.GetComponent<playerControl>().LockCursor(true);
    }

    public void TogglePower()
    {
        powerOn = !powerOn;

        if (powerOn)
            lightBulb.GetComponent<BulbControl>().ToggleLight(powerOn);
        else
            lightBulb.GetComponent<BulbControl>().ToggleLight(powerOn);
    }
}
