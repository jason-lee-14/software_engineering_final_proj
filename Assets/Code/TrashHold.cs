using System.Collections;
using UnityEngine;

public class TrashHold : MonoBehaviour
{
    public Transform guide;// where the trash will attach to 
    public bool holding;// whether or not the trash is currently being held onto
    public int collideCounterLimit, collideCounter; //the counter and limit at which the trash will be let go
    private Vector3 mOffset;//varibale to help keep the trash in place to te pliers
    public Rigidbody rb;//rigibody reference to trash

    public XMLManager manager; 

    void Start(){
        manager = GameObject.Find("XMLManager").GetComponent<XMLManager>();
        collideCounterLimit = manager.parameters.numBumps;
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f) * manager.parameters.trashSize;

        collideCounter = 0;
        holding = false;
        guide= GameObject.Find("Plier_Guide").transform;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }
    //detects objects bumping into the object
    private void OnCollisionEnter(Collision other) {
        //if you come into contact with the plier, get grabbed
        if (other.gameObject.name=="plierHolder"){
            if (!holding)
                hold_control();
        }else{
            if(holding) {
                StartCoroutine(sleepTrash(2f));
                LogObjectData.LogAction(this, LogObjectData.Actions.collision);
            }
        }
    }
    //actually moves the trash and keeps it in front of the pliers
    void FixedUpdate(){
        if(holding){
            Vector3 pos = guide.position +mOffset;
            rb.velocity = (pos - rb.position) * 3250 * Time.deltaTime;
        }
    }
    private void Update() {
        //let go of the object if you press the F key
        if(Input.GetKeyDown(KeyCode.F)){
            if(holding) {
                hold_control();
                LogObjectData.LogAction(this, LogObjectData.Actions.put_down);
            }
        }    
        //let go of the object if you reach the allotted number of times of colliding
        if(collideCounter >= collideCounterLimit)
            hold_control();
        
    }
    //controls whether the trash will latch onto or not
    public void hold_control(){
        //hold if not holding
        if(holding == false){
            gameObject.transform.position = guide.position;
            mOffset = gameObject.transform.position - guide.position;
            holding=true;
            collideCounter=0;
            LogObjectData.LogAction(this, LogObjectData.Actions.pick_up);
        //let go if not holding
        }else{
            rb.velocity = Vector3.zero; 
            holding=false;
            collideCounter=0;
        }
    }
    //prevent collision loops
    IEnumerator sleepTrash(float wait){
        if(holding){
            collideCounter++;
        }
        yield return new WaitForSeconds(wait);
    }
}
