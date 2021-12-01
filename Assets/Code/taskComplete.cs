using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskComplete : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inTrash;
    void Start()
    {
        inTrash = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "trash") { //trash is now in bin, set to true
            inTrash = true;
        }
    }
}
