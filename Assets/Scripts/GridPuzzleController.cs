using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleController : MonoBehaviour
{
    public int[,] currentGrid;
    private int buttonAmount = 9;
    public int[] randOrder;

    public Material[] matList;

    public int[] selectedGrid = new int[2];

    public bool orderCorrect = false;
    public bool rotationCorrect = false;

    public GameObject[] pieces;
    public GameObject[] selectGameobject = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        //99 is used as indicator for empty
        for (int i = 0; i < 2; i++)
        {
            selectedGrid[i] = 99;
        }   

        randOrder = new int[buttonAmount];
        pieces = new GameObject[buttonAmount];
        
        //Generate a 9 digit long array full of non repeting random numbers
        for(int i = 0; i < randOrder.Length; i++)
        {
            bool match = false;
            while(!match)
            {
                bool breakout = false;
                randOrder[i] = Random.Range(0, buttonAmount);

                for (int j = 0; j < randOrder.Length; j++)
                    if(i != j)
                        if (randOrder[i] == randOrder[j])
                        {
                            breakout = true;
                            break;
                        }

                if(!breakout)
                    match = true;  
            }
        }

        for (int i = 0; i < buttonAmount; i++)
        {
            pieces[i] = transform.GetChild(i).gameObject;

            pieces[i].AddComponent<GridPuzzlePiece>();
            pieces[i].GetComponent<GridPuzzlePiece>().matNum = randOrder[i];
            pieces[i].GetComponent<GridPuzzlePiece>().pieceNum = i;

            pieces[i].GetComponent<GridPuzzlePiece>().rotation = Random.Range(0, 4);
            pieces[i].GetComponent<GridPuzzlePiece>().Rotate();

            pieces[i].GetComponent<Renderer>().material = matList[randOrder[i]];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate(bool active)
    {
        if (active)
        {
            GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < buttonAmount; i++)
            {
                pieces[i].GetComponent<BoxCollider>().enabled = true;
                pieces[i].GetComponent<GridPuzzlePiece>().enabled = true;
            }

            transform.GetChild(9).GetComponent<Camera>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < buttonAmount; i++)
            {
                pieces[i].GetComponent<BoxCollider>().enabled = false;
                pieces[i].GetComponent<GridPuzzlePiece>().enabled = false;
            }

            transform.GetChild(9).GetComponent<Camera>().enabled = false;
        }
    }
    public void ClearSelectedPieces()
    {
        for (int i = 0; i < 2; i++)
        {
            selectedGrid[i] = 99;
            selectGameobject[i] = null;
        }
    }

    //If SelectetPiece == 99 then its empty
    public void switchPiece(int peiceNum, GameObject selected)
    {
        for (int i = 0; i < 2; i++)
            if (selectedGrid[i] == 99)
            {
                selectedGrid[i] = peiceNum;
                selectGameobject[i] = selected;
                break;
            }

        //Makes sure The array is full before carrying out a switch
        if (selectedGrid[1] != 99)
        {
            swapElements();

            ClearSelectedPieces();
            CheckCorrect();
        }
    }

    private void swapElements()
    {
        int FIRST = 0;
        int SECOND = 1;

        //Switch Array Elements
        pieces[selectedGrid[FIRST]] = selectGameobject[SECOND];
        pieces[selectedGrid[SECOND]] = selectGameobject[FIRST];

        //Switch Piece number
        pieces[selectedGrid[FIRST]].GetComponent<GridPuzzlePiece>().pieceNum = selectedGrid[FIRST];
        pieces[selectedGrid[SECOND]].GetComponent<GridPuzzlePiece>().pieceNum = selectedGrid[SECOND];

        //Switch mat order
        randOrder[selectedGrid[FIRST]] = pieces[selectedGrid[FIRST]].GetComponent<GridPuzzlePiece>().matNum;
        randOrder[selectedGrid[SECOND]] = pieces[selectedGrid[SECOND]].GetComponent<GridPuzzlePiece>().matNum;

        //Switch postions

        Vector3 firstPos = selectGameobject[FIRST].transform.position;
        Vector3 secondPos = selectGameobject[SECOND].transform.position;

        //pieces[selectedGrid[FIRST]].transform.position = firstPos;
        //pieces[selectedGrid[SECOND]].transform.position = secondPos;

        pieces[selectedGrid[FIRST]].GetComponent<GridPuzzlePiece>().StartTranslation(firstPos);
        pieces[selectedGrid[SECOND]].GetComponent<GridPuzzlePiece>().StartTranslation(secondPos);

        //Needs to me moved to individual peices code
       // pieces[selectedGrid[FIRST]].GetComponent<GridPuzzlePiece>().clicked = false;
        //pieces[selectedGrid[SECOND]].GetComponent<GridPuzzlePiece>().clicked = false;

        //pieces[selectedGrid[FIRST]].GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));
        //pieces[selectedGrid[SECOND]].GetComponent<Renderer>().material.SetColor("_Color", new Color32(255, 255, 255, 0));

        //pieces[selectedGrid[FIRST]].GetComponent<GridPuzzlePiece>().Reset();
        //pieces[selectedGrid[SECOND]].GetComponent<GridPuzzlePiece>().Reset();
    }
   
    /// <summary>
    /// Check That both order and rotation is correct
    /// </summary>
    public void CheckCorrect()
    {
        //Check the order is correct
        for (int i = 0; i < randOrder.Length; i++)
        {
            if (randOrder[i] != i)
                break;
            else if (i == 8 && randOrder[(randOrder.Length - 1)] == i)
            {
                Debug.Log("Order Match check obj");
                orderCorrect = true;
            }
        }

        //Check the rotation is correct
        if (orderCorrect)
            for (int i = 0; i < randOrder.Length; i++)
            {
                if (pieces[i].GetComponent<GridPuzzlePiece>().rotation != 0)
                    break;                
                else if (i == 8 && pieces[randOrder.Length - 1].GetComponent<GridPuzzlePiece>().rotation == 0)
                {
                    rotationCorrect = true;
                    Debug.Log("Rot Match check obj");
                }
            }

        if(orderCorrect && rotationCorrect)
        {
            for(int i = 0; i < randOrder.Length - 1; i ++)
            {
                pieces[i].GetComponent<GridPuzzlePiece>().Reset();
            }
        }
    }
}
