using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPuzzleController : MonoBehaviour
{
    public int[,] currentGrid;
    private int buttonAmount = 4;
    public int[] randOrder;
    public Material[] matList;
    public int[] selectedGrid = new int[2];
    public int[] selectedMatRefs = new int[2];

    // Start is called before the first frame update
    void Start()
    {
        //99 is used as indicator for empty
        for (int i = 0; i < 2; i++)
        {
            selectedGrid[i] = 99;
            selectedMatRefs[i] = 99;
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
           // transform.GetChild(i).gameObject.AddComponent<Outline>();
            transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().matNum = randOrder[i];
            transform.GetChild(i).gameObject.GetComponent<GridPuzzlePiece>().pieceNum = i;
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
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;

            transform.GetChild(4).GetComponent<Camera>().enabled = true;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < buttonAmount; i++)
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;

            transform.GetChild(4).GetComponent<Camera>().enabled = false;
        }

    }

    //If SelectetPiece == 99 then its empty
    public void switchPiece(int peiceNum, int matRef, bool clear)
    {
        if(!clear)
        {
            for (int i = 0; i < 2; i++)
            {
                if (selectedGrid[i] == 99)
                {
                    selectedGrid[i] = peiceNum;
                    selectedMatRefs[i] = matRef;
                    break;
                }

            }

            if (selectedGrid[1] != 99)
            {
                randOrder[selectedGrid[0]] = selectedMatRefs[1];
                randOrder[selectedGrid[1]] = selectedMatRefs[0];

                transform.GetChild(selectedGrid[0]).gameObject.GetComponent<Renderer>().material = matList[selectedMatRefs[1]];
                transform.GetChild(selectedGrid[1]).gameObject.GetComponent<Renderer>().material = matList[selectedMatRefs[0]];


                transform.GetChild(selectedGrid[0]).gameObject.GetComponent<GridPuzzlePiece>().matNum = selectedMatRefs[1];
                transform.GetChild(selectedGrid[1]).gameObject.GetComponent<GridPuzzlePiece>().matNum = selectedMatRefs[0];

                transform.GetChild(selectedGrid[0]).gameObject.GetComponent<GridPuzzlePiece>().clicked = false;
                transform.GetChild(selectedGrid[1]).gameObject.GetComponent<GridPuzzlePiece>().clicked = false;

                for (int i = 0; i < 2; i++)
                {
                    selectedGrid[i] = 99;
                    selectedMatRefs[i] = 99;
                }
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                selectedGrid[i] = 99;
                selectedMatRefs[i] = 99;
            }
        }
        
    }
}
