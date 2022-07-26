using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float _sensitivity;

    protected Vector3 posLastFrame;
    public bool active = false;

    private bool zoomed = false;

    // Start is called before the first frame update
    void Start()
    {
        _sensitivity = 0.4f;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            rotateObject();
            KeyboardZoom();
        }
    }

    void rotateObject()
    {
        if (Input.GetMouseButtonDown(0))
            posLastFrame = Input.mousePosition;
        

        if (Input.GetMouseButton(0))
        {
            var delta = Input.mousePosition - posLastFrame;
           // Debug.Log($"delta: {delta}"); 
            posLastFrame = Input.mousePosition;

            var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
            transform.rotation = Quaternion.AngleAxis(delta.magnitude * _sensitivity, axis) * transform.rotation;
        }

        Debug.DrawRay(transform.position, transform.forward, Color.blue);
    }

    void KeyboardZoom()
    {
        float x = transform.position.x;
        float y = transform.position.y;

        // Zoom In keyboard input
        if (Input.GetKeyDown(KeyCode.E)) 
            zoomed = true; 
        
        // Zoom Out keyboard input
        if (Input.GetKeyDown(KeyCode.Q)) 
            zoomed = false; 
        

        if (zoomed)
            transform.localPosition = new Vector3(x, y, 0.75f);

        else
            transform.localPosition = new Vector3(x, y, 1f);
    }
}