using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzlePiece : MonoBehaviour
{
    public int pieceNum = 0;
    public int matNum = 0;
    public int rotation = 0;

    public bool clicked = false;

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
                parentScript.switchPiece();
                parentScript.switchPiece(pieceNum, matNum, rotation);
                parentScript.RotationalCheckCorrect();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                rotation--;
                if (rotation <= -1)
                    rotation = 3;

                Rotate();
                parentScript.switchPiece();
                parentScript.switchPiece(pieceNum, matNum, rotation);
                parentScript.RotationalCheckCorrect();
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
                parentScript.switchPiece(pieceNum, matNum, rotation);
            }
            else
            {
                Debug.Log(pieceNum);
                GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));
                parentScript.switchPiece();
            }
    }

    public void swapAssign(int rot, int mat)
    {
        rotation = rot;
        matNum = mat;
        clicked = false;
        Rotate();
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
