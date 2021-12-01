using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class LogObjectData : MonoBehaviour
{
    // Enum of actions accepted by the Data Logger
    public enum Actions {
        create,
        turn_on, turn_off,
        open, close,
        pick_up, put_down,
        screw, unscrew,
        place,
        collision
    }

    // Flags for specifying included information in a LogAction call
    [System.Flags]
    public enum LogOptions {
        None            = 0b_0000_0000,
        GapTime         = 0b_0000_0001,
        CumulativeTime  = 0b_0000_0010,
        Position        = 0b_0000_0100,
        Rotation        = 0b_0000_1000,
        All             = 0b_0000_1111
    }

    const string PRECISION = "f3";
    const string FORBIDDEN_CHARS_REGEX = "[(),]";

    static float lastActionTime = 0.0f;



    // Start is called before the first frame update for each object
    void Start() {
        // LogAction(Actions.Create);
    }



    // Any MonoBehavior game object can send data to the Data Logger by calling
    // this static method. Useful when a log trigger is in a difficult position.
    // Expected use is to send "this" as the first argument.
    // Ex. LogObjectData.LogAction(this, Actions.pick_up, LogOptions.All);
    public static void LogAction(MonoBehaviour gameObject, Actions action, LogOptions options) {
        List<string> data = new List<string> { action.ToString(), gameObject.name };
        Transform transform = gameObject.transform;
        float cumulativeTime = Time.time;
        float gapTime = cumulativeTime - lastActionTime;
        lastActionTime = cumulativeTime;

        if (options.HasFlag(LogOptions.GapTime))            data.Add(gapTime.ToString(PRECISION));
        if (options.HasFlag(LogOptions.CumulativeTime))     data.Add(cumulativeTime.ToString(PRECISION));
        if (options.HasFlag(LogOptions.Position))           data.Add(Regex.Replace(transform.position.ToString(PRECISION), FORBIDDEN_CHARS_REGEX, ""));
        if (options.HasFlag(LogOptions.Rotation))           data.Add(Regex.Replace(transform.rotation.ToString(PRECISION), FORBIDDEN_CHARS_REGEX, ""));

        string dataLine = string.Join(", ", data.ToArray());
        Debug.Log(dataLine);

        try {
            IEnumerator PostToLog = DataLogger.Instance.PostToLog(dataLine);
            gameObject.StartCoroutine(PostToLog);
        } catch (System.Exception e) {
            Debug.LogWarning(e.ToString());
        }
    }

    public static void LogAction(MonoBehaviour gameObject, Actions action) {
        LogAction(gameObject, action, LogOptions.All);
    }



    // Instantiated classes can call these methods as a shortcut.
    // Left for compatibility from prior revisions.
    protected void LogAction(Actions action, LogOptions options) {
        LogAction(this, action, options);
    }

    protected void LogAction(Actions action) {
        LogAction(this, action, LogOptions.All);
    }
}
