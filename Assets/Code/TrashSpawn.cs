using UnityEngine;

public class TrashSpawn : MonoBehaviour
{
    private GameObject [] allTrash; //possible spawn locations (ordered from left to right)

    public int luckyPipe;
    public bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        allTrash = GameObject.FindGameObjectsWithTag("trash"); //grab everything in scene with "trash" tag

        luckyPipe = Random.Range(0, 4); //randomly choose trash pipe
        allTrash[luckyPipe].SetActive(true); // set trash to active
        for (int i = 0; i < 4; i++) //set all other trash to inactive
            if (i != luckyPipe)
                allTrash[i].SetActive(false);

        done = true;
    }

}
