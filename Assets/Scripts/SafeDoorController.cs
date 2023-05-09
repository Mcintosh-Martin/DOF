using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeDoorController : MonoBehaviour
{
    enum CurrentState { rest, spinHandle, openDoor }
    CurrentState curState = CurrentState.rest;

    //Timer calculated by adding up DeltaTime
    private float timerInSeconds = 0f;

    //Spin handle variables
    private readonly float SPINTIMEINSECONDS = 1.5f;
    private readonly float SPINROTATIONSPEED = 2.5f;

    //Open Door Variables
    private readonly Quaternion STARTROTATION = Quaternion.Euler(270, 270, 0);
    private readonly Quaternion ENDROTATION = Quaternion.Euler(270, 270, 148);
    private readonly float DOOROPENSPEED = 0.5f;

    // Start is called before the first frame update
    void Start()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        switch(curState)
        {
            case CurrentState.spinHandle:
                SpinHandle();
                break;
            case CurrentState.openDoor:
                OpenSafeDoor();
                break;
        }
    }

    public void OpenSafe()
    {
        curState = CurrentState.spinHandle;
    }

    //Spin door handle for set amount of time
    private void SpinHandle()
    {
        timerInSeconds += Time.deltaTime;

        if(timerInSeconds < SPINTIMEINSECONDS)
            gameObject.transform.GetChild(1).transform.Rotate(0,0, SPINROTATIONSPEED, Space.Self);
        else
        {
            curState = CurrentState.openDoor;
            timerInSeconds = 0f;
        }
    }

    private void OpenSafeDoor()
    {
        timerInSeconds += Time.deltaTime;
        transform.rotation = Quaternion.Lerp(STARTROTATION, ENDROTATION, timerInSeconds * DOOROPENSPEED);
    }
}
