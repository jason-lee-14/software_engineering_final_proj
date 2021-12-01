using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogTool : LogObjectData
{
    void OnMouseDown() {
        // DEPENDING ON THE ORDER SCRIPTS ARE LOADED, THE VALUE OF THE BOOLEAN MAY HAVE UPDATED BEFORE THIS CALL
        LogAction(GetComponent<physicsPlierHold>().holding ? Actions.put_down : Actions.pick_up);
    }
}
