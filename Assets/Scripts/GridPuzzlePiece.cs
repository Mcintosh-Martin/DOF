using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzlePiece : MonoBehaviour
{
    public int pieceNum = 0;
    public int matNum = 0;
    public int rotation = 0;

    public bool clicked = false;

    Vector3 startPosition = new Vector3();
    Vector3 endPosition = new Vector3();

    float translationTimerInSeconds = 0f;

    bool startTranslation = false;

    private GridPuzzleController parentScript;
    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.GetComponentInParent<GridPuzzleController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(clicked && !parentScript.rotationCorrect)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                rotation++;
                if (rotation >= 4)
                    rotation = 0;

                Rotate();
                parentScript.CheckCorrect();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotation--;
                if (rotation <= -1)
                    rotation = 3;

                Rotate();
                parentScript.CheckCorrect();
            }
        }

        if(startTranslation)
        {
            if (translationTimerInSeconds < 1)
            {
                translationTimerInSeconds += Time.deltaTime * 1.25f;

                transform.position = Vector3.Lerp(startPosition, endPosition, translationTimerInSeconds);

                if (translationTimerInSeconds >= 1)
                {
                    translationTimerInSeconds = 0f;
                    startPosition = new Vector3();
                    endPosition = new Vector3();

                    startTranslation = false;
                    Reset();
                }

            }
        }
    }

    private void OnMouseDown()
    {
        clicked = !clicked;
        if(!parentScript.rotationCorrect || !parentScript.orderCorrect)
            if(clicked)
            {
                Debug.Log(pieceNum);
                GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 130, 255));
                
                
                transform.localPosition = new Vector3(2, transform.localPosition.y, transform.localPosition.z);
                parentScript.switchPiece(pieceNum, gameObject);                
            }
            else
            {
                Debug.Log(pieceNum);
                GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));
                transform.localPosition = new Vector3(0.39f, transform.localPosition.y, transform.localPosition.z);
                parentScript.ClearSelectedPieces();
            }
    }

    public void Reset()
    {
        clicked = false;
        transform.localPosition = new Vector3(0.39f, transform.localPosition.y, transform.localPosition.z);
        GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));
    }

    public void StartTranslation(Vector3 endPos)
    {
        startPosition = transform.position;
        endPosition = endPos;

        startTranslation = true;
    }

    public void Rotate()
    {
        transform.rotation = Quaternion.identity;

        switch(rotation)
        {
            case 1:
                transform.Rotate(90, 0, 0);
                break;
            case 2:
                transform.Rotate(180, 0, 0);
                break;
            case 3:
                transform.Rotate(270, 0, 0);
                break;
        }
    }
}
