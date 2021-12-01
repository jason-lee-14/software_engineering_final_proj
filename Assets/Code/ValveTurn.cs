using System.Collections;
using UnityEngine;

public class ValveTurn : MonoBehaviour
{
    // Start is called before the first frame update
    public float angleTurned = 179, angleBase = 0;
    public float flapTime = 3; // Number of seconds for to open or close.
    public GameObject[] caps; //cap array to allow caps to be turned after valve is turned
    public GameObject indicatorLight; //light above valve to indicate whether the trash has turned off or not

    public UnityEngine.Material onMaterial;//on color
    public UnityEngine.Material offMaterial;//on color

    Quaternion rotOpened; // Rotation when fully opened.
    Quaternion rotClosed; // Rotation when full closed.
    bool isFlapping = false; // Animate and lockout while true.
    public bool isClosed = true; // Track open/closed state.
    float changeSign;//help variable to do the animation for closing vs opening the valve
    
    //getter methods for acceptance testing
    public float ChangeSign
    {
        get => changeSign;
    }
    public bool IsFlapping
    {
        get => isFlapping;
    }

    public bool IsClosed
    {
        get => isClosed;
    }

    public Quaternion RotOpened
    {
        get => rotOpened;
    }

    public Quaternion RotClosed
    {
        get => rotClosed;
    }

    void Start(){
        rotOpened = Quaternion.Euler(0, angleTurned, 0);
        rotClosed = Quaternion.Euler(0, angleBase, 0);
        indicatorLight.GetComponentInChildren<Renderer>().material = offMaterial;
    }

    private void OnMouseDown(){
        //enable the switches when the valve is turned
        for (int i = 0; i < 4; i++){
            if(!caps[i].GetComponent<CapControl>().isScrewedIn)
                return;
        }
        StartCoroutine(TurnValve());
    }
    public IEnumerator TurnValve(){
        // Lockout any attempt to start another FlapLid while
        // one is already running.
        if (isFlapping)
        {
            isClosed = !isClosed;
            changeSign = changeSign *-1;
            yield break;
        }
        // Start the animation and lockout others.
        isFlapping = true;
        // Vary this from zero to one, or from one to zero,
        // to interpolate between our quaternions.
        float interpolationParameter;
        // Set lerp parameter to match our state, and the sign
        // of the change to either increase or decrease the
        // lerp parameter during animation.
        if (isClosed){
            interpolationParameter = 0;
            changeSign = 1;
        }else{
            interpolationParameter = 1;
            changeSign = -1;
        }
        while (isFlapping){
            // Change our "lerp" parameter according to speed and time,
            // and according to whether we are opening or closing.
            interpolationParameter = interpolationParameter + changeSign * Time.deltaTime / flapTime;
            // At or past either end of the lerp parameter's range means
            // we are on our last step.
            if (interpolationParameter >= 1 || interpolationParameter <= 0){
                // Clamp the lerp parameter.
                interpolationParameter = Mathf.Clamp(interpolationParameter, 0, 1);
                isFlapping = false; // Signal the loop to stop after this.
            }
            // Set the X angle to however much rotation is done so far.
            transform.localRotation = Quaternion.Lerp(rotClosed, rotOpened, interpolationParameter);
            // Tell Unity to start us up again at some future time.
            yield return null;
        }
        // Toggle our open/closed state.
        isClosed = !isClosed;

        if(!isClosed)
            indicatorLight.GetComponentInChildren<Renderer>().material = onMaterial;
        else
            indicatorLight.GetComponentInChildren<Renderer>().material = offMaterial;

        for (int i = 0; i < 4; i++)
            caps[i].GetComponent<CapControl>().caplock = !caps[i].GetComponent<CapControl>().caplock;
    }
}
