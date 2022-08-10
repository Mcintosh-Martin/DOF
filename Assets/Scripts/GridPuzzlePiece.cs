using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzlePiece : MonoBehaviour
{
    public int pieceNum = 0;
    public int matNum = 0;

    public bool clicked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        clicked = !clicked;

        if(clicked)
        {
            Debug.Log(pieceNum);
            GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 130, 255));
            transform.GetComponentInParent<GridPuzzleController>().switchPiece(pieceNum, matNum, false);
        }
        else
        {
            Debug.Log(pieceNum);
            GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));
            transform.GetComponentInParent<GridPuzzleController>().switchPiece(pieceNum, matNum, true);
        }

        
    }
}
