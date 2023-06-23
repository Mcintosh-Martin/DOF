using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    //Character Controls
    private CharacterController control;
    public float runSpeed = 8.0f;
    public float walkSpeed = 4.0f;
    public float movementSpeed = 4.0f;
    public bool grounded;
    //private Rigidbody rigidBody;
   
    //Camera controls
    public float sensitivity = 10f;
    private const float maxYAngle = 80f;
    private Camera mycam;
    private Vector2 curRot;

    //UI
    public Text text;
    public Canvas RenderText;
    //public Canvas TestCanvas;
    public Canvas invCanvas;

    //Item that is active in scene thats being interacted with
    public GameObject interactableGameObject;

    //All itens within Player inventory
    public GameObject[] inventory;
    public int curInvItem = 0;

    //Access Keypad

    //Access Grid Puzzle

    //Access Light Puzzle

    //Access light bulb

    //Access light bulb
    //private bool usingLightBase = false;

    //Stores the currently Hovered item. Special refers to items that are simply rotatable DEFINED IN USAGEENUMS
    public CurrentlyHovered curHovered = CurrentlyHovered.None;
    public CurrentlyUsing curUsing = CurrentlyUsing.None;

    // Start is called before the first frame update
    void Start()
    {
        control = gameObject.GetComponent<CharacterController>();
       // rigidBody = gameObject.GetComponent<Rigidbody>();
        mycam = Camera.main;

        text.enabled = false;

        inventory = new GameObject[8];

        //Disable character controller collider
        control.detectCollisions = false;
    }

    // Update is called once per frame
    void Update()
    {
        UIIput();
        CameraMove();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        FireRay();
    }

    //Pass item hit by raycast to inventory
    void addToInventory(GameObject ItemToStore)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (!inventory[i]) 
            {
                inventory[i] = ItemToStore;
                break;
            }
        }
    }

    void FireRay()
    {
        RaycastHit hit;
        Ray ray = new Ray(mycam.transform.position, mycam.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (Physics.Raycast(ray, out hit, 2.5f) && curUsing == CurrentlyUsing.None)
        {
            if (hit.collider != null)
            {
                switch(hit.collider.tag)
                {                    
                    case "Special":
                        if (curUsing == CurrentlyUsing.None)
                        {
                            interactableGameObject = hit.collider.gameObject;
                            interactableGameObject.GetComponent<Outline>().OutlineWidth = 6;
                            interactableGameObject.GetComponent<MatChange>().switchm();
                            text.enabled = true;
                            curHovered = CurrentlyHovered.Special;
                        }
                        break;
                    case "Keypad":                        
                        curHovered = CurrentlyHovered.Keypad;
                        interactableGameObject = hit.collider.gameObject;                        
                        break;
                    case "Grid":                        
                        curHovered = CurrentlyHovered.Grid;
                        interactableGameObject = hit.collider.gameObject;                            
                        break;
                    case "LightPuzzle":
                        curHovered = CurrentlyHovered.LightPuzzle;
                        interactableGameObject = hit.collider.gameObject;                 
                        break;

                    case "Bulb":                        
                        interactableGameObject = hit.collider.gameObject;
                        interactableGameObject.GetComponent<Outline>().OutlineWidth = 6;
                        interactableGameObject.GetComponent<BulbControl>().SetOutlineTimer();
                        text.enabled = true;

                        curHovered = CurrentlyHovered.LightBulb;
                        interactableGameObject = hit.collider.gameObject;
                        break;

                    case "LampBase":
                        curHovered = CurrentlyHovered.LampBase;
                        interactableGameObject = hit.collider.gameObject;
                        break;
                        
                    default:
                        text.enabled = false;
                        curHovered = CurrentlyHovered.None;
                        interactableGameObject = null;
                        break;      
                }
            }
            else
                Debug.Log("not Hitting Anything");            
        }
        else
            if(interactableGameObject != null && curUsing == CurrentlyUsing.None)
            {
                Debug.Log("not hittin");
                text.enabled = false;
                curHovered = CurrentlyHovered.None;
                interactableGameObject = null;
            }            
    }

    //Handle the cameras rotation baseed on mouse postition
    void CameraMove()
    {
        if(curUsing == CurrentlyUsing.None && !invCanvas.enabled)
        { 
            // mouse look at
            curRot.x += Input.GetAxis("Mouse X") * sensitivity;
            curRot.y -= Input.GetAxis("Mouse Y") * sensitivity;
            curRot.x = Mathf.Repeat(curRot.x, 360);
            curRot.y = Mathf.Clamp(curRot.y, -maxYAngle, maxYAngle);
            mycam.transform.rotation = Quaternion.Euler(curRot.y, curRot.x, 0);

            //mimic cameras rotation to gameobject rotation
            transform.rotation = Quaternion.Euler(0, curRot.x, 0);

            LockCursor(true);
        }
    }

    //Handle Player object movement in scene
    void PlayerMovement()
    {
        //Handle Gravity of character controller
        grounded = control.isGrounded;
        float gravity = 0;
        if (grounded) { gravity = 0; }
        else { gravity -= 9.81f * Time.deltaTime; }
        control.Move(new Vector3(0, gravity, 0));

        //Recalculate forward
        Vector3 fwd = transform.rotation * Vector3.forward;
        Vector3 bkw = transform.rotation * Vector3.back;
        Vector3 lft = transform.rotation * Vector3.left;
        Vector3 rht = transform.rotation * Vector3.right;

        //Calculate diagonal vectors 
        Vector3 FR = Quaternion.Euler(0, 45, 0) * fwd;
        Vector3 FL = Quaternion.Euler(0, -45, 0) * fwd;
        Vector3 BR = Quaternion.Euler(0, 135, 0) * fwd;
        Vector3 BL = Quaternion.Euler(0, -135, 0) * fwd;     

        if (curUsing == CurrentlyUsing.None)
        {
            //Sprint
            if (Input.GetKey(KeyCode.LeftShift)) { movementSpeed = runSpeed; }
            else { movementSpeed = walkSpeed; }
 
            //WASD movment
            if (Input.GetKey(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.D)) { Move(FR); }                              
                else if (Input.GetKey(KeyCode.A)) { Move(FL); }
                else { Move(fwd); }     
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.D)) { Move(BR); }                                    
                else if (Input.GetKey(KeyCode.A)) { Move(BL); }                    
                else { Move(bkw); }                                
            }
            
            else if (Input.GetKey(KeyCode.A)) { Move(lft); }                                   
            else if (Input.GetKey(KeyCode.D)) { Move(rht); }                
        }        
    }

    private void UIIput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            switch(curHovered)
            {
                //Handle opening the rotating object
                case CurrentlyHovered.Special:
                    curUsing = CurrentlyUsing.Special;
                    curHovered = CurrentlyHovered.None;

                    MoveItem();
                    break;

                //Handle the open of keypad
                case CurrentlyHovered.Keypad:
                    curUsing = CurrentlyUsing.Keypad;
                    curHovered = CurrentlyHovered.None;

                    Camera.main.enabled = false;

                    LockCursor(false);

                    interactableGameObject.GetComponent<KeyPadController>().activate(true);
                    break;

                case CurrentlyHovered.Grid:
                    curUsing = CurrentlyUsing.Grid;
                    curHovered = CurrentlyHovered.None;

                    Camera.main.enabled = false;
                    LockCursor(false);

                    interactableGameObject.GetComponent<GridPuzzleController>().activate(true);
                    break;

                case CurrentlyHovered.LightPuzzle:
                    curUsing = CurrentlyUsing.LightPuzzle;
                    curHovered = CurrentlyHovered.None;

                    Camera.main.enabled = false;
                    LockCursor(false);

                    interactableGameObject.GetComponent<FlashLightPuzzleController>().activate(true);
                    break;

                case CurrentlyHovered.LightBulb:
                    curUsing = CurrentlyUsing.LightBulb;
                    curHovered = CurrentlyHovered.None;

                    interactableGameObject.gameObject.GetComponent<Outline>().OutlineWidth = 0f;
                    interactableGameObject.GetComponent<BulbControl>().toggleRotate(true);
                    break;

                case CurrentlyHovered.LampBase:
                    curUsing = CurrentlyUsing.usingLampBase;
                    curHovered = CurrentlyHovered.None;
                    LockCursor(false);
                    interactableGameObject.GetComponent<LampBaseController>().AddRemoveBulb();
                    break;

            }
        }

        //RenderText.active;
        if (Input.GetKey(KeyCode.Tab))
        {
            if (RenderText.enabled)
            {
                if(curUsing != CurrentlyUsing.usingLampBase)
                {
                    if (curInvItem != 0)
                    {
                        inventory[curInvItem - 1].gameObject.GetComponent<MatChange>().toggleRotate(false);
                        invCanvas.transform.GetChild(1).GetComponent<InventoryUIUpdate>().Assign();
                    }
                    else
                    {
                        interactableGameObject.gameObject.GetComponent<MatChange>().toggleRotate(false);
                        curUsing = CurrentlyUsing.None;
                    }
                }
                else
                {
                    GameObject lampBase = GameObject.FindGameObjectWithTag("LampBase");

                    lampBase.GetComponent<LampBaseController>().lightBulb.GetComponent<BulbControl>().setMovement(2);
                    lampBase.GetComponent<LampBaseController>().lightBulb.GetComponent<BulbControl>().GetComponent<BoxCollider>().enabled = false;
                    lampBase.GetComponent<LampBaseController>().hasBulb = true;
                    curUsing = CurrentlyUsing.None;
                }
                

                RenderText.enabled = false;
            }

            switch(curUsing)
            {
                //Handle the Exit of keypad
                case CurrentlyUsing.Keypad:
                    curUsing = CurrentlyUsing.None;

                    transform.GetChild(0).GetComponent<Camera>().enabled = true;

                    LockCursor(true);

                    interactableGameObject.GetComponent<KeyPadController>().activate(false);
                    break;

                //Handle the Exit of keypad
                case CurrentlyUsing.Grid:
                    curUsing = CurrentlyUsing.None;

                    transform.GetChild(0).GetComponent<Camera>().enabled = true;

                    LockCursor(true);

                    interactableGameObject.GetComponent<GridPuzzleController>().activate(false);
                    break;

                //Handle the Exit of light Puzzle
                case CurrentlyUsing.LightPuzzle:
                    curUsing = CurrentlyUsing.None;

                    transform.GetChild(0).GetComponent<Camera>().enabled = true;

                    LockCursor(true);

                    interactableGameObject.GetComponent<FlashLightPuzzleController>().activate(false);
                    break;
            }
        }

        //Add the current rotating object to inventory
        if (Input.GetKeyDown(KeyCode.R) /*&& (curUsing == CurrentlyUsing.Special || curUsing == CurrentlyUsing.LightBulb || curUsing == CurrentlyUsing.usingLampBase)*/)
        {
            if (curUsing == CurrentlyUsing.Special)
            {
                addToInventory(interactableGameObject);
                Vector3 newLocation = interactableGameObject.transform.position;
                newLocation.y = -50f;
                interactableGameObject.transform.position = newLocation;

                interactableGameObject.gameObject.GetComponent<MatChange>().toggleRotate(false);
                RenderText.enabled = false;
                curUsing = CurrentlyUsing.None;
            }
            
            if(curUsing == CurrentlyUsing.LightBulb)
            {
                addToInventory(interactableGameObject);
                Vector3 newLocation = interactableGameObject.transform.position;
                newLocation.y = -50f;
                interactableGameObject.transform.position = newLocation;
                interactableGameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);

                interactableGameObject.gameObject.GetComponent<BulbControl>().toggleRotate(false);
                curUsing = CurrentlyUsing.None;
            }

            if(curUsing == CurrentlyUsing.usingLampBase)
            {
                //store temp object 
                GameObject lampBase = GameObject.FindGameObjectWithTag("LampBase");

                //Store object in inventory
                addToInventory(lampBase.GetComponent<LampBaseController>().lightBulb);

                //Move to unaccessable area
                Vector3 newLoc = lampBase.GetComponent<LampBaseController>().lightBulb.transform.position;
                newLoc.y = -50f;
                lampBase.GetComponent<LampBaseController>().lightBulb.transform.position = newLoc;

                //Clear up Lampbase variables to starting point without a lightbulb

                //Close renderTexture
                RenderText.enabled = false;
                lampBase.GetComponent<LampBaseController>().lightBulb.GetComponent<BulbControl>().toggleRotate(false);

                //Clear up light bulb variables as though has no lampbase
                lampBase.GetComponent<LampBaseController>().lightBulb.GetComponent<BulbControl>().GetComponent<BoxCollider>().enabled = false;

                //Remove gameobject from Lampbase
                lampBase.GetComponent<LampBaseController>().lightBulb = null;

                curUsing = CurrentlyUsing.None;
            }

            //Toggle lamp power on/off
            if (curHovered == CurrentlyHovered.LampBase)
            {
                interactableGameObject.GetComponent<LampBaseController>().TogglePower();
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!invCanvas.enabled)
            {
                invCanvas.transform.GetChild(1).GetComponent<InventoryUIUpdate>().Assign();

                invCanvas.enabled = true;
                LockCursor(false);
            }
            else
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    if (inventory[i])
                        inventory[i].gameObject.GetComponent<MatChange>().toggleRotate(false);
                }

                if (curInvItem != 0)
                {
                    inventory[curInvItem - 1].gameObject.GetComponent<MatChange>().toggleRotate(false);
                    invCanvas.transform.GetChild(1).GetComponent<InventoryUIUpdate>().Assign();
                    RenderText.enabled = false;
                }

                invCanvas.enabled = false;
                LockCursor(true);
            }
        }
    }

    void Move(Vector3 direction)
    {
        control.Move(direction * (movementSpeed * Time.deltaTime));
    }

    void MoveItem()
    {
        interactableGameObject.gameObject.GetComponent<Outline>().OutlineWidth = 0f;
        interactableGameObject.gameObject.GetComponent<MatChange>().toggleRotate(true);       
        
        text.enabled = false;
        LockCursor(false);
        RenderText.enabled = true;
    }

    public void useInventoryItem(int index, bool active)
    {
        if(active)
        {
            curInvItem = (index + 1);
            inventory[index].GetComponent<MatChange>().toggleRotate(true);

            text.enabled = false;

            RenderText.enabled = true;
        }
        else
        {
            inventory[index].GetComponent<MatChange>().toggleRotate(false);

            text.enabled = true;

            RenderText.enabled = false;
            curInvItem = 0;
        }
    }

    /// <summary>
    /// Active = False: Visible and Movable,
    /// Active = True: Invisible and Non-movable
    /// </summary>
    public void LockCursor(bool active)
    {
        Cursor.visible = !active;

        if(!active)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
