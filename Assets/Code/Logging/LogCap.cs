using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogCap : LogObjectData
{
    static float placeLock = 0f;

    void OnMouseDown() {
        CapControl cap = GetComponent<CapControl>();
        bool caplock = cap.caplock;
        bool isScrewedIn = cap.isScrewedIn;
        bool isScrewing = cap.IsScrewing;
        bool holding = cap.IsHolding;
        bool ready = cap.IsReady;

        // This took too long to figure out...
        if (!caplock && !isScrewing) {
            // Debug.Log($"{isScrewedIn}, {holding}, {ready}");
            if (isScrewedIn) {
                LogAction(Actions.unscrew);
            } else if (ready && holding) {
                LogAction(Actions.put_down);
            } else if (ready && !holding) {
                LogAction(Actions.pick_up);
            } else if (!ready && holding) {
                LogAction(Actions.screw);
            }
        }
    }

    void OnTriggerEnter(Collider collider) {
        if (!GetComponent<CapControl>().isScrewedIn && collider.gameObject.tag.Equals("cap_trigger")
                && Time.time - placeLock >= 1f) {
            placeLock = Time.time;
            LogAction(Actions.place);
        }
    }
}
