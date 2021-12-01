using UnityEngine;

public class physicsPlierHold : MonoBehaviour, IInteractable
{
    public bool m_Held = false;//is it being held 
    private Rigidbody m_ThisRigidbody = null;//rigid body of object
    private FixedJoint m_HoldJoint = null;//joint reference to connect to player
    float rotObjY, rotObjX;//rotation values
    public GameObject holder, main_cam;

    public Transform guide;//holds object in place in front of player
    public bool holding;//determine if obj is being held

    public PlayerController cam_control;// reference to camera to lock it if you are turning the pliers in hand
    public void Start(){
        gameObject.tag = "Interactable";

        //initialize values
        rotObjY = 0f;
        rotObjX = 0f; 
        holding = false;

        //get references to objects
        main_cam = GameObject.Find("Me");
        cam_control = main_cam.GetComponent<PlayerController>();
        m_ThisRigidbody = GetComponent<Rigidbody>();

        //find the guide
        holder = GameObject.Find ("guide");
        guide= GameObject.Find ("guide").transform;
    }
    private void Update(){
        holding = m_Held;
        if(Input.GetKey(KeyCode.T) ){
            //LOCK PLAYER CAMERA MOVEMENT
            if(holding){
                cam_control.rotateLock = true;
                rotate_control();
            }
        }else{
            //reset the camera lock
            cam_control.rotateLock = false;
        }
        // If the holding joint has broken, drop the object
        if (m_HoldJoint == null && m_Held == true){
            m_Held = false;
            m_ThisRigidbody.useGravity = true;
        }
    }

    // Pick up the object, or drop it if it is already being held
    public void Interact(PlayerController playerScript)
    {
        // Is the object currently being held?
        if (m_Held){
            //drop the object if the object is being held and left click is pressed again
            Drop();
        }
        else{
            //set location to guide to make rotations around self
            transform.position = playerScript.guide_ref.transform.position;
            transform.rotation = playerScript.guide_ref.transform.rotation;
            //set hold parameters
            m_Held = true;
            m_ThisRigidbody.useGravity = false;
            //attach the joint and set the break force
            m_HoldJoint = playerScript.guide_ref.gameObject.AddComponent<FixedJoint>();
            m_HoldJoint.breakForce = 30000; 
            m_HoldJoint.connectedBody = m_ThisRigidbody;
        }
    }
    //rotate
    void rotate_control(){
        //CONTROL ROTATION
        rotObjY += 75 * Time.deltaTime * Input.GetAxis("Mouse X");
        rotObjX += 75 * Time.deltaTime * -Input.GetAxis("Mouse Y");
        m_HoldJoint.transform.localEulerAngles = new Vector3(rotObjX,rotObjY,0);
    }

    // Drop the object
    private void Drop(){
        m_Held = false;
        m_ThisRigidbody.useGravity = true;

        Destroy(m_HoldJoint);
    }
}
