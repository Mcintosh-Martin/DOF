using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class MatChange : MonoBehaviour
{
    private bool MatTime = false;
    private float targetTime = 0.05f;
    public GameObject rotatableVersion;
    public Sprite uiPortrait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(MatTime)
        {
            targetTime -= Time.deltaTime;
            //Debug.Log($"Time: {targetTime}");
        }

        if(targetTime <= 0)
        {
            switchToBaseMat();
        }

    }

    public void toggleRotate(bool active)
    {
        rotatableVersion.GetComponent<RotateObject>().active = active;
        rotatableVersion.GetComponent<MeshRenderer>().enabled = active;
    }

    public void switchm()
    {
        targetTime = 0.05f;
        MatTime = true;
    }

    private void switchToBaseMat()
    {
        MatTime = false;
        var outline = gameObject.GetComponent<Outline>().OutlineWidth = 0f;
    }
}
