using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSwitch : LogObjectData
{
    void OnMouseDown() {
        // DEPENDING ON THE ORDER SCRIPTS ARE LOADED, THE VALUE OF THE BOOLEAN MAY HAVE UPDATED BEFORE THIS CALL
        LogAction(GetComponent<SwitchFlip>().isFlipped ? Actions.turn_off : Actions.turn_on);
    }
}
