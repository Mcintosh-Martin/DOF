using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulbControl : MonoBehaviour
{
    //Begin Of Testing rotation integrations
    private bool MatTime = false;
    private float targetTime = 0.05f;
    public GameObject rotatableVersion;
    public GameObject player;
    //End Of Testing rotation integrations

    public bool startRot = false;
    public float lowestPoint;
    public float highestPoint;
    public float fraction = 0;
    public float speed = 0.5f;

    public GameObject glowText;

    public bool isBlackLight = false;

    //Will determin if in base or out of base functionality runs
    public bool inBase;

    enum Movement { Upwards, Downwards, Resting}
    Movement movement = Movement.Resting;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<BoxCollider>().enabled = !inBase;
    }

    // Update is called once per frame
    void Update()
    {
        //Begin Of Testing rotation integrations
        if(!inBase)
        {
            if (MatTime)
            {
                targetTime -= Time.deltaTime;
            }

            if (targetTime <= 0)
            {
                ToggleOutlineOff();
            }
        }
        //End Of Testing rotation integrations

        switch (movement)
        {
            case Movement.Downwards:
                moveBulb(highestPoint, lowestPoint);
                break;

            case Movement.Upwards:
                moveBulb(lowestPoint, highestPoint);
                ToggleLight(false);
                break;

            case Movement.Resting:
                break;
        }
    }

    /// <summary>
    /// 0 = rest | 1 = upwards | 2 = Downwards
    /// This is an inBase = True Function
    /// </summary>
    public void setMovement(int direction)
    {
        switch (direction)
        {
            case 1:
                movement = Movement.Upwards;
                
                break;
            case 2:
                movement = Movement.Downwards;
                break;
        }
    }
  
    /// <summary>
    /// Handle the Upwards or downwards movement of the gameobject
    /// This is an inBase = True Function
    /// </summary>
    private void moveBulb(float start, float dest)
    {
        if (fraction < 1)
        {
            fraction += Time.deltaTime * speed;

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(start, dest, fraction), transform.position.z);

            //Track the movement of the bulb with the player camera
            if(movement == Movement.Upwards)
            {
                player.GetComponentInChildren<Camera>().transform.LookAt(transform);
            }
        }

        //Check the bulb is at final postion
        if (transform.position.y == dest)
        {
            if(movement == Movement.Downwards)
            {
                if(GameObject.FindGameObjectWithTag("LampBase").GetComponent<LampBaseController>().powerOn)
                    ToggleLight(true);
            }
            if(movement == Movement.Upwards)
            {
                toggleRotate(true);
            }

            movement = Movement.Resting;
            fraction = 0;
        }
    }

    //Begin Of Testing rotation integrations

    /// <summary>
    /// Handle the start up or end of the rotation of the gameobject
    /// This is an inBase = True & False Function
    /// </summary>
    public void toggleRotate(bool active)
    {
        rotatableVersion.GetComponent<RotateObject>().active = active;
        rotatableVersion.GetComponent<MeshRenderer>().enabled = active;

        player.GetComponent<playerControl>().RenderText.enabled = active;
        player.GetComponent<playerControl>().text.enabled = false;
        player.GetComponent<playerControl>().LockCursor(!active);
    }

    /// <summary>
    /// Resets needed values to toggle off the outline
    /// This is an inBase = False Function
    /// </summary>
    public void SetOutlineTimer()
    {
        targetTime = 0.05f;
        MatTime = true;
    }

    /// <summary>
    /// Toggles off the outline of the gameobject
    /// This is an inBase = False Function
    /// </summary>
    private void ToggleOutlineOff()
    {
        MatTime = false;
        var outline = gameObject.GetComponent<Outline>().OutlineWidth = 0f;
    }

    ///<summary
    /// Toggles The Light and Text On/Off
    /// True = On, False = Off
    /// </summary>
    public void ToggleLight(bool power)
    {
        GetComponent<Light>().enabled = power;

        //Do not allow alpha change in glow text if not a black light
        if(isBlackLight)
        {
            if(power)
                glowText.GetComponent<AlphaText>().changeState(1);
            else
                glowText.GetComponent<AlphaText>().changeState(2);
        }
    }
    //End Of Testing rotation integrations
}
