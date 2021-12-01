using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTrash : LogObjectData
{
    // LOGGING FEATURE FOR TRASH OBJECT EMBEDDED IN TrashHold.cs
    // (BEHAVIOR IS TOO COMPLEX TO HAVING LOGGING IN A SEPARATE CLASS)
    
    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.name == "plierHolder" && GetComponent<TrashHold>().holding) {
    //         LogAction(Actions.pick_up);
    //     }
    // }

    // void Update() {
    //     if (Input.GetKeyDown(KeyCode.F) && GetComponent<TrashHold>().holding) {
    //         LogAction(Actions.put_down);
    //     }
    // }
}
