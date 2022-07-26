using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    //Character Controls
    private CharacterController control;
    public float movementSpeed = 2.0f;
    public bool grounded;
    private Rigidbody rigidBody;
   
    //Camera controls
    private const float sensitivity = 10f;
    private const float maxYAngle = 80f;
    private Camera mycam;
    private Vector2 curRot;

    //UI
    public Text text;
    public Canvas RenderText;
    public bool hooverdSpecial = false;
    public bool clickedSpecial = false;
    //public Canvas TestCanvas;
    public Canvas invCanvas;

    //Item that is active in scene thats being interacted with
    private GameObject interactableGameObject;

    //All itens within Player inventory
    public GameObject[] inventory;
    public int curInvItem = 0;


    // Start is called before the first frame update
    void Start()
    {
        control = gameObject.GetComponent<CharacterController>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        mycam = Camera.main;

        text.enabled = false;

        inventory = new GameObject[3];

        //Disable character controller collider
        control.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = control.isGrounded;

        CameraMove();

        if (clickedSpecial)
            MoveItem();

        UIIput();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        FireRay();
        //UIIput();
    }

    //Pass item hit by raycast to inventory
    void addToInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (!inventory[i]) 
            {
                inventory[i] = interactableGameObject;
                break;
            }
        }
    }

    void FireRay()
    {
        RaycastHit hit;
        Ray ray = new Ray(mycam.transform.position, mycam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (Physics.Raycast(ray, out hit, 2.5f))
        {
            Transform objectHit = hit.transform;

            if (hit.collider != null)
            {

                if (hit.collider.CompareTag("Special"))
                {
                    interactableGameObject = hit.collider.gameObject;
                    //Debug.Log($"s{interactableGameObject}");
                    interactableGameObject.GetComponent<Outline>().OutlineWidth = 6;
                    interactableGameObject.GetComponent<MatChange>().switchm();
                    text.enabled = true;
                    hooverdSpecial = true;
                }
                else
                {
                    interactableGameObject = null;
                    text.enabled = false;
                    hooverdSpecial = false;
                }

                //Debug.Log("did hit");
            }


            Debug.Log($"Found: {hit.transform.name}");
            // Do something with the object that was hit by the raycast.
        }
    }

    //Handle the cameras rotation baseed on mouse postition
    void CameraMove()
    {
        if(!clickedSpecial && !invCanvas.enabled)
        { 
            // mouse look at
            curRot.x += Input.GetAxis("Mouse X") * sensitivity;
            curRot.y -= Input.GetAxis("Mouse Y") * sensitivity;
            curRot.x = Mathf.Repeat(curRot.x, 360);
            curRot.y = Mathf.Clamp(curRot.y, -maxYAngle, maxYAngle);
            mycam.transform.rotation = Quaternion.Euler(curRot.y, curRot.x, 0);

            //mimic cameras rotation to gameobject rotation
            transform.rotation = Quaternion.Euler(0, curRot.x, 0);

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Handle Player object movement in scene
    void PlayerMovement()
    {
        Camera mycam = Camera.main;

        //Recalculate forward
        Vector3 fwd = transform.rotation * Vector3.forward;
        Vector3 bkw = transform.rotation * Vector3.back;
        Vector3 lft = transform.rotation * Vector3.left;
        Vector3 rht = transform.rotation * Vector3.right;

        if (!clickedSpecial)
        {
            //Sprint
            if (Input.GetKey(KeyCode.LeftShift))
                movementSpeed = 8;

            else
                movementSpeed = 2;
            
            //WASD movment
            if (Input.GetKey(KeyCode.W))
                Move(fwd);
            
            else if (Input.GetKey(KeyCode.S))            
                Move(bkw);
            
            else if (Input.GetKey(KeyCode.A))            
                Move(lft);
            
            else if (Input.GetKey(KeyCode.D))
                Move(rht);
        }        
    }

    private void UIIput()
    {
        if (Input.GetKey(KeyCode.E) && hooverdSpecial)
        {
            clickedSpecial = true;
            hooverdSpecial = false;
        }

        //RenderText.active;

        if (Input.GetKey(KeyCode.Tab) && RenderText.enabled)
        {
            if(curInvItem != 0)
            {
                inventory[curInvItem - 1].gameObject.GetComponent<MatChange>().toggleRotate(false);
                invCanvas.transform.GetChild(1).GetComponent<InventoryUIUpdate>().Assign();                
            }
            else
            {
                interactableGameObject.gameObject.GetComponent<MatChange>().toggleRotate(false);
                clickedSpecial = false;             
            }

            RenderText.enabled = false;
        }

        if (Input.GetKey(KeyCode.G) && clickedSpecial)
        {
            addToInventory();

            interactableGameObject.gameObject.GetComponent<MatChange>().toggleRotate(false);

            Vector3 newLocation = interactableGameObject.transform.position;
            newLocation.y = -50f;
            interactableGameObject.transform.position = newLocation;

            clickedSpecial = false;
            RenderText.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            //invCanvas.enabled = true;
            if (!invCanvas.enabled)
            {
                invCanvas.transform.GetChild(1).GetComponent<InventoryUIUpdate>().Assign();

                invCanvas.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (inventory[i])
                        inventory[i].gameObject.GetComponent<MatChange>().toggleRotate(false);
                }

                invCanvas.enabled = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void Move(Vector3 direction)
    {
       rigidBody.velocity = direction * (movementSpeed);
    }

    void MoveItem()
    {
        interactableGameObject.gameObject.GetComponent<Outline>().OutlineWidth = 0f;
        interactableGameObject.gameObject.GetComponent<MatChange>().toggleRotate(true);       
        
        text.enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //RenderText.enabled = true;
        RenderText.enabled = true;
    }

    public void useInventoryItem(int index, bool active)
    {
        if(active)
        {
            curInvItem = (index + 1);
            inventory[index].GetComponent<MatChange>().toggleRotate(true);

            text.enabled = false;
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;

            //RenderText.enabled = true;
            RenderText.enabled = true;


        }
        else
        {
            inventory[index].GetComponent<MatChange>().toggleRotate(false);

            text.enabled = true;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            RenderText.enabled = false;
            curInvItem = 0;
        }
    }
}
