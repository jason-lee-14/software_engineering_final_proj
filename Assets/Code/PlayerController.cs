using UnityEngine;
using UnityEngine.SceneManagement;

public interface IInteractable{
    void Interact(PlayerController script);
}
public class PlayerController : MonoBehaviour{
    public Transform guide_ref = null;
    public float vecSpeed;
    public bool rotateLock;//camera lock for hold variable

    private Scene scene;

    private RaycastHit m_RaycastFocus;
    private bool m_CanInteract = false;//interactable object with right click
    private Vector3 stepVector, sideVector, upDownVec;// vectors for movement
    
    int Speed;//speed of translation
    private float x = 0.0F;
    private float y = 0.0F;
    float xSpeed = 20.0F;//speed of looking around
    float distance = 5.0F;
    bool rightclicked = false;

        //getter methods for acceptance testing
    public Vector3 GetStepVector { get => stepVector; }
    public Vector3 GetSideVector { get => sideVector; }
    public Vector3 GetUpDownVector { get => upDownVec; }
    public int GetSpeed { get => Speed; }


    public void Start()
    {
        guide_ref = GameObject.Find ("guide").transform;
        rotateLock = false;

        stepVector = vecSpeed * Vector3.forward;
        sideVector = vecSpeed * Vector3.right;
        upDownVec = vecSpeed * Vector3.up;
  
        scene = SceneManager.GetActiveScene();

        Speed = 1;
        var angles = transform.eulerAngles;
        x = angles.y;
    }

    private void FixedUpdate(){
        if (Input.GetKey(KeyCode.Escape)){
#if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(scene.name);

        float yValue = 0.0f;
        if (Input.GetMouseButton(1))
            rightclicked = true;
        else
            rightclicked = false;

        /*move up/down*/
        if (Input.GetKey(KeyCode.LeftShift))
            yValue = -Speed*.1F;
        if (Input.GetKey(KeyCode.Space))
            yValue = Speed*.1F;

        //this bind zx axis prevents the user from moving in 3d space
        //movement has been restricted to only one 2D plane at once so as to make it easier to mechanically maneuver around 3d space 
        float bind_ZX_axis = transform.position.y;
        //move side to side/ forward and back
        transform.Translate(Speed * Time.deltaTime * Input.GetAxis("Horizontal"), 0, Speed * Time.deltaTime * Input.GetAxis("Vertical"));
        transform.position = new Vector3(transform.position.x, bind_ZX_axis, transform.position.z);
        //move up and down
        transform.Translate(0.1f * upDownVec * Time.deltaTime * Input.GetAxis("up_down_control"), Space.World);
        //prevent user from leaving the space
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -20, 20),
            Mathf.Clamp(transform.position.y, 0.5f, 10),
            Mathf.Clamp(transform.position.z, -20, 20));
    }
     void LateUpdate() {
         //speed to look around
        if (rightclicked == true && !rotateLock) {
            x += Input.GetAxis("Mouse X") * xSpeed * distance* .02F;
            y += Input.GetAxis("Mouse Y") * xSpeed * distance* .02F;
            var rotation = Quaternion.Euler(-y, x, 0.0F);
            transform.rotation = rotation;
        }
     }
    private void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Cursor.visible = true;

        // Is interactable object detected in front of player?
        if (Physics.Raycast(ray, out m_RaycastFocus, 20) && m_RaycastFocus.collider.transform.tag == "Interactable"){
            m_CanInteract = true;
        }
        else{
            m_CanInteract = false;
        }

        // Has interact button been pressed whilst interactable object is in front of player?
        if (Input.GetButtonDown("Fire1") && m_CanInteract == true)
        {
            IInteractable interactComponent = m_RaycastFocus.collider.transform.GetComponent<IInteractable>();
            if (interactComponent != null)
                interactComponent.Interact(this);  // Perform object's interaction
        }
    }
}