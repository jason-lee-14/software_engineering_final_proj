using UnityEngine;

public class PipeScale : MonoBehaviour
{
    public XMLManager manager;

    // Start is called before the first frame update
    void Start()
    {

        manager = GameObject.Find("XMLManager").GetComponent<XMLManager>();
        //new Vector3(2.5f, 0.05f, 0.0625f) 
        transform.localScale = transform.localScale * manager.parameters.pipeDiameter;
    }
}
