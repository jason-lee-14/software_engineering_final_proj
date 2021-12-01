using System.Collections;
using UnityEngine;

public class SwitchFlip : MonoBehaviour{
    public GameObject indicator;//light indicator
    public UnityEngine.Material onMaterial;//on color
    public UnityEngine.Material offMaterial;//on color
    public UnityEngine.Material standbyMaterial; //standby material
    public float flipAngle, startAngle;//angle for when it turns off or on for the switch

    //getter methods for acceptance testing
    public float GetFlipAngle { get => flipAngle; }
    public float GetStartAngle { get => startAngle; }

    public bool isFlipped = false;//keep track of its state
    public taskComplete trashCan; //reference to trash bin to see if done
    public GameObject trash;//reference to trash to see if trash is removed

    public GameObject valve; // reference to main valve for checking if it's closed

    public TrashSpawn spawner;

    public void Start()
    {
        flipAngle = -18.5f;
        startAngle = 0f;
        StartCoroutine(Setter());

        trashCan = GameObject.Find("trash_can").GetComponent<taskComplete>();
    }
    //set colors
    IEnumerator Setter(){
        yield return new WaitUntil(() => spawner.done);

        if (trash.activeSelf)
            indicator.GetComponentInChildren<Renderer>().material = offMaterial;
        else
            indicator.GetComponentInChildren<Renderer>().material = onMaterial;

    }

    // Update is called once per frame
    public void OnMouseDown(){

        //flip switch based on state
        if(!isFlipped){//turn on switch
            transform.localRotation = Quaternion.Euler(0, 0, flipAngle);
            indicator.GetComponentInChildren<Renderer>().material = standbyMaterial;
        } else{//turn off switch
            transform.localRotation = Quaternion.Euler(0, 0, startAngle);
            if(trashCan.inTrash && valve.GetComponent<ValveTurn>().isClosed) // don't let switch flip green until trash removed
                indicator.GetComponentInChildren<Renderer>().material = onMaterial;
        }
        isFlipped = !isFlipped;
    }
}
