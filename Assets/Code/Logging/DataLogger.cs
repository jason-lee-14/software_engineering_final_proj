using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public sealed class DataLogger : MonoBehaviour
{
    // Import JavaScript helpers to get session information
    [DllImport("__Internal")]
    static extern string GetOrigin();

    // Structure needed to use JsonUtility
    [System.Serializable]
    struct DataStruct {
        public string data;
    }

    const string POST_ENDPOINT = "/log-endpoint";

    string origin = null;

    static DataLogger _instance = null;
    public static DataLogger Instance { get { return _instance ? _instance : throw new UnityException("DataLogger instance not initialized"); } }



    // Runs after the Unity script instance is loaded.
    // Only one DataLogger is allowed in total, accessible through static variable.
    void Awake() {
        if (_instance == null) {
            try {
                origin = GetOrigin();
                Debug.Log($"Data Logger set successfully to {origin}{POST_ENDPOINT}");
            } catch (System.Exception e) {
                Debug.LogWarning($"Data Logger initialization unsuccessful, {e.GetType().Name} thrown");
                Debug.LogWarning("Data Logger will continue logging but will not save any data");
            }
            _instance = this;
        } else if (_instance != this) {
            Destroy(gameObject.GetComponent<DataLogger>());
        }
    }

    // Post data to web server
    public IEnumerator PostToLog(string data) {
        if (origin != null) {
            DataStruct postData = new DataStruct() { data = data };
            byte[] rawJSONData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(postData));

            using (UnityWebRequest req = new UnityWebRequest(origin + POST_ENDPOINT, "POST")) {
                req.uploadHandler = new UploadHandlerRaw(rawJSONData);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                yield return req.SendWebRequest();

                if (req.isNetworkError || req.isHttpError) {
                    Debug.LogWarning(req.error);
                }
            }
        }
    }
}
