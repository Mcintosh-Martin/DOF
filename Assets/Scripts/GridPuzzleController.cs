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
    public int[] selectedMatRefs = new int[2];
    public int[] selectedRots = new int[2];
    public bool orderCorrect = false;
    public bool rotationCorrect = false;

    // Start is called before the first frame update
    void Start()
    {
        //99 is used as indicator for empty
        for (int i = 0; i < 2; i++)
        {
            selectedGrid[i] = 99;
            selectedMatRefs[i] = 99;
            selectedRots[i] = 99;
        }   

        randOrder = new int[buttonAmount];
        
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
            Debug.Log(randOrder[i]);
        }

        //currentGrid = new int[2, 2];

        for (int i = 0; i < buttonAmount; i++)
        {
            transform.GetChild(i).gameObject.AddComponent<GridPuzzlePiece>();
            transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().matNum = randOrder[i];
            transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().pieceNum = i;

            transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().rotation = Random.Range(0,4);
            transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().Rotate();

            transform.GetChild(i).gameObject.GetComponent<Renderer>().material = matList[randOrder[i]];
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
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().enabled = true;
            }

            transform.GetChild(9).GetComponent<Camera>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < buttonAmount; i++)
            {
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().enabled = false;
            }

            transform.GetChild(9).GetComponent<Camera>().enabled = false;
        }

    }
    public void switchPiece()
    {
        for (int i = 0; i < 2; i++)
        {
            selectedGrid[i] = 99;
            selectedMatRefs[i] = 99;
            selectedRots[i] = 99;
        }
    }

    //If SelectetPiece == 99 then its empty
    public void switchPiece(int peiceNum, int matRef,int rot)
    {
        for (int i = 0; i < 2; i++)
            if (selectedGrid[i] == 99)
            {
                selectedGrid[i] = peiceNum;
                selectedMatRefs[i] = matRef;
                selectedRots[i] = rot;
                break;
            }

        if (selectedGrid[1] != 99)
        {
            swap(0, 1);
            swap(1, 0);

            switchPiece();

            for (int i = 0; i < randOrder.Length; i++)
            {
                if (randOrder[i] != i)
                    break;
                else if (i == 8 && randOrder[(randOrder.Length - 1)] == i)
                {
                    Debug.Log("Order Match check obj");
                    orderCorrect = true;

                    RotationalCheckCorrect();
                }
            }
        }
    }

    private void swap(int first, int second)
    {
        randOrder[selectedGrid[first]] = selectedMatRefs[second];
        transform.GetChild(selectedGrid[first]).gameObject.GetComponent<Renderer>().material = matList[selectedMatRefs[second]];
        transform.GetChild(selectedGrid[first]).gameObject.GetComponent<GridPuzzlePiece>().swapAssign(selectedRots[second], selectedMatRefs[second]);
    }
    
    public void RotationalCheckCorrect()
    {
        if(orderCorrect)
            for(int i = 0; i < randOrder.Length; i++)
            {
                if(transform.GetChild(i).GetComponent<GridPuzzlePiece>().rotation != 0)
                {
                    break;
                }
                else if (i == 8 && transform.GetChild(randOrder.Length - 1).GetComponent<GridPuzzlePiece>().rotation == 0)
                {
                    rotationCorrect = true;
                    Debug.Log("Rot Match check obj");
                }
            }
    }
}
