using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRoots : MonoBehaviour
{
    public Tree Tree;

    public GameObject TreeRootPrefab;
    public int numOfSegments = 1;

    public float interval = 5.0f;
    private float intervalLast = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
       if (intervalLast < Time.time && Tree.Stamina > 0){
            intervalLast = Time.time + interval;
            spawnRoots();
       }
    }

    void spawnRoots() {
        GameObject newrootsegment = Instantiate(TreeRootPrefab, new Vector3((numOfSegments * 1.44f) + TreeRootPrefab.transform.position.x, TreeRootPrefab.transform.position.y, TreeRootPrefab.transform.position.z), Quaternion.identity);
        newrootsegment.transform.parent = gameObject.transform;
        Debug.Log( GameObject.Find("rootSegment").transform.position);
        numOfSegments++;
        Tree.Stamina -= 20;
    }
}
