using System.Collections;
using UnityEngine;

public class CapControl : MonoBehaviour, IInteractable
{
    public Transform guide, posScrew, rotScrew;
    public float unscrewHieght;//how far the cap goes to leave the pipe
    public bool caplock, isScrewedIn, m_Held = false;//is it being held
    public PlayerController cam_control;//enables camera lock control if you rotate the cap
    public GameObject holder, main_cam;

    private Rigidbody m_ThisRigidbody = null;//rigid body of object
    private FixedJoint m_HoldJoint = null;//joint reference to connect to player
    float changeSign, rotObjY, rotObjX;//rotation values
    bool isScrewing, ready, holding;//boolean locks

    Vector3 posScrewIn, posScrewOut;//position from screwed out state and screwed in
    Quaternion rotScrewIn, rotScrewOut;//rotation tracking

    GameObject[] possible_parents;
    Transform parent;
    //getter methods for acceptance testing
    public bool IsScrewing
    {
        get => isScrewing;
    }
    public bool IsScrewedIn
    {
        get => isScrewedIn;
    }

    public bool IsHolding
    {
        get => holding;
    }
    public bool IsReady
    {
        get => ready;
    }


    private void Start()
    {
        gameObject.tag = "Interactable";

        //get list of all colliers -> current parent is the closest
        possible_parents = GameObject.FindGameObjectsWithTag("cap_parent");   

        //define position and rotation of cap when screwed in vs screwed out
        posScrewIn = transform.localPosition;
        rotScrewIn = transform.localRotation;

        posScrewOut = new Vector3(posScrewIn.x, posScrewIn.y + unscrewHieght, posScrewIn.z);
        rotScrewOut = Quaternion.Euler(rotScrewIn.x, rotScrewIn.y + 180, rotScrewIn.z);

        isScrewedIn = true;
        isScrewing = false;
        ready = false;
        caplock = true;

        rotObjY = 0f;
        rotObjX = 0f;

        //obtain reference to camera's guide
        holding = false;
        holder = GameObject.Find("guide");
        guide = GameObject.Find("guide").transform;

        main_cam = GameObject.Find("Me");
        cam_control = main_cam.GetComponent<PlayerController>();
        m_ThisRigidbody = GetComponent<Rigidbody>();
    }

    public void OnTriggerEnter(Collider other)
    {
        //on trigger, reset parent and boolean flags
        if (!isScrewedIn & other.gameObject.tag.Equals("cap_trigger"))
        {
            transform.parent = findParent();
            ready = false;
            holding = false;
            m_ThisRigidbody.isKinematic = true;

            //snapping code
            transform.localPosition = posScrewOut;
            transform.localRotation = rotScrewOut;
        }
      
    }

    private void Update()
    {
        holding = m_Held;
        if (Input.GetKey(KeyCode.T))
        {
            //LOCK PLAYER CAMERA MOVEMENT
            if (holding)
            {
                cam_control.rotateLock = true;
                rotate_control();
            }
        }
        else
        {
            //reset the camera lock
            cam_control.rotateLock = false;
        }
        // If the holding joint has broken, drop the object
        if (m_HoldJoint == null && m_Held == true)
        {
            m_Held = false;
            m_ThisRigidbody.useGravity = true;
        }
    }

    private Transform findParent()
    {
        float minDistance = 1000f;
        float tempDistance;
        for (int i = 0; i < possible_parents.Length; i++)
        {
            tempDistance = Vector3.Distance(transform.position, possible_parents[i].transform.position);
            if (tempDistance < minDistance)
            {
                minDistance = tempDistance;
                parent = possible_parents[i].transform;
            }
        }
        return parent;
    }

    //this coroutine handles the screwing in/out animation
    IEnumerator Twist()
    {
        if (isScrewing)
            yield break;

        isScrewing = true;

        float interpolationParameter;

        if (isScrewedIn)
        {
            interpolationParameter = 0;
            changeSign = 1;
        }
        else
        {
            interpolationParameter = 1;
            changeSign = -1;
        }

        while (isScrewing)
        {
            interpolationParameter +=  changeSign * Time.deltaTime / 2;

            if (interpolationParameter >= 1 || interpolationParameter <= 0)
            {
                // Clamp the lerp parameter.

                interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);

                isScrewing = false; // Signal the loop to stop after this.
            }

            transform.localPosition = Vector3.Lerp(posScrewIn, posScrewOut, interpolationParameter);
            transform.localRotation = Quaternion.Lerp(rotScrewIn, rotScrewOut, interpolationParameter);

            yield return null;
        }
        isScrewedIn = !isScrewedIn;
        ready = isScrewedIn ? false : true;
    }

    // Pick up the object, or drop it if it is already being held
    public void Interact(PlayerController playerScript)
    {
        if (!caplock)
        {
            if (!ready)
                StartCoroutine(Twist());
            else
            {
                // Is the object currently being held?
                if (m_Held)
                {
                    //drop the object if the object is being held and left click is pressed again
                    Drop();
                }
                else
                {
                    //set location to guide to make rotations around self
                    transform.position = playerScript.guide_ref.transform.position;
                    transform.rotation = playerScript.guide_ref.transform.rotation;
                    m_ThisRigidbody.isKinematic = false;

                    //set hold parameters
                    m_Held = true;
                    m_ThisRigidbody.useGravity = false;
                    //attach the joint and set the break force
                    m_HoldJoint = playerScript.guide_ref.gameObject.AddComponent<FixedJoint>();
                    m_HoldJoint.breakForce = 30000;
                    m_HoldJoint.connectedBody = m_ThisRigidbody;
                }
            }
        }
    }

    //rotate
    void rotate_control()
    {
        //CONTROL ROTATION
        rotObjY += 75 * Time.deltaTime * Input.GetAxis("Mouse X");
        rotObjX += 75 * Time.deltaTime * -Input.GetAxis("Mouse Y");
        m_HoldJoint.transform.localEulerAngles = new Vector3(rotObjX, rotObjY, 0);
    }

    // Drop the object
    private void Drop()
    {
        m_Held = false;
        m_ThisRigidbody.useGravity = true;

        Destroy(m_HoldJoint);
    }
}
