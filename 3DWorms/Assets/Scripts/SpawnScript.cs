using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{

    public GameObject[] spawns;
    GameObject currentPoint;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        index = Random.Range(0, spawns.Length);
        currentPoint = spawns[index];

        transform.position = currentPoint.transform.position;
        transform.rotation = currentPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
