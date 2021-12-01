using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogValve : LogObjectData
{
    void OnMouseDown() {
        // DEPENDING ON THE ORDER SCRIPTS ARE LOADED, THE VALUE OF THE BOOLEAN MAY HAVE UPDATED BEFORE THIS CALL
        LogAction(GetComponent<ValveTurn>().isClosed ? Actions.close : Actions.open);
    }
}
