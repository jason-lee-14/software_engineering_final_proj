using System.Collections;
using UnityEngine;

using UnityEngine.Networking;
using System.Runtime.InteropServices;
using System.Xml.Serialization; //Access xml serializer
using System.IO;                //file management

public class XMLManager : MonoBehaviour
{
	public static XMLManager ins;

	// Import JavaScript helpers to get session information
	[DllImport("__Internal")]
	static extern string GetOrigin();

	string origin = null;

	void Awake()
	{
		ins = this;
		try
		{
			origin = GetOrigin();
		}
		catch (System.Exception e)
		{
			Debug.Log("I can't find origin :(");
		}
		LoadItems();
	}

	//List of items
	public Params parameters;
	const string GET_ENDPOINT = "/task-params";

	public IEnumerator Get()
	{
		//Debug.Log(itemDB.filename);
		if (origin != null)
		{
			using (UnityWebRequest req = new UnityWebRequest(origin + GET_ENDPOINT, "GET"))
			{
				//Send a get request
				req.SetRequestHeader("Content-Type", "text/xml");
				req.downloadHandler = new DownloadHandlerBuffer();
				yield return req.SendWebRequest();
				//Read in the data
				var data = req.downloadHandler.text;

				Debug.Log(data.ToString());

				//Convert data into a data stream to be used by the XMLSerializer
				MemoryStream stream = new MemoryStream();
				StreamWriter writer = new StreamWriter(stream);
				XmlSerializer serializer = new XmlSerializer(typeof(Params));
				writer.Write(data);
				writer.Flush();
				stream.Position = 0;

				Debug.Log("before deserializer");
				parameters = serializer.Deserialize(stream) as Params;
				Debug.Log("after deserializer");
			}
		}
	}

	//Load function
	public void LoadItems()
	{
        StartCoroutine(Get());
	}

}


[System.Serializable]           //Allows it to be viewed in inspector
public class Params
{
	public int numBumps;
	public float pipeDiameter;
	public float trashSize;
}
