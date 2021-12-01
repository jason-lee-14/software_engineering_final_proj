using System.Collections;
using UnityEngine;

public class BumpVisualizer : MonoBehaviour
{
    int currCounter, oldCounter;// counter to determine if bumped
    TrashHold manager;//grabs trash hold component to see the bump counter
    Material mat;//grabbing the texture of the trash

    Color startingColor; //inital color of the trash

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        startingColor = mat.color;

        manager = GetComponent<TrashHold>();
        currCounter = 0;
        oldCounter = currCounter;
    }
    //flash the color red on the trash to indicate that the trash has bumped
    IEnumerator colorFlash()
    {
        GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        yield return new WaitForSeconds(0.2f);
        GetComponent<Renderer>().material.color = startingColor;
    }

    // if you bump then flash that you bumped
    void Update()
    {
        currCounter = manager.collideCounter;
        if (currCounter != oldCounter) {
            StartCoroutine(colorFlash());
            oldCounter = currCounter;
        }
    }

}
