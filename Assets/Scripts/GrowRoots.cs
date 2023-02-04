using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRoots : MonoBehaviour
{
    public Tree Tree;

    public GameObject TreeRootPrefab;
    public GameObject TreeEndRoot;
    public List<GameObject> roots = new List<GameObject>();

    public void SpawnRoot() {
        GameObject newrootsegment = null;
        newrootsegment = Instantiate(TreeRootPrefab, Vector3.zero, Quaternion.identity);
        newrootsegment.transform.parent = this.gameObject.transform;
        if (roots.Count > 0)
        {
            Vector3 lastRootPosition = roots[roots.Count - 1].transform.localPosition;
            newrootsegment.transform.localPosition = new Vector3(1.44f + lastRootPosition.x, lastRootPosition.y, lastRootPosition.z);
        }
        else
        {
            newrootsegment.transform.localPosition = Vector3.zero;
        }
        TreeEndRoot.transform.localPosition = new Vector3(newrootsegment.transform.localPosition.x + 1.2f, newrootsegment.transform.localPosition.y, newrootsegment.transform.localPosition.z);
        roots.Add(newrootsegment);
    }

    public void DestroyRoot()
    {
        if (roots.Count > 0) {
            Destroy(roots[roots.Count - 1]);
            roots.RemoveAt(roots.Count - 1);
            if (roots.Count > 1) {
                Vector3 lastRootPosition = roots[roots.Count - 1].transform.localPosition;
                TreeEndRoot.transform.localPosition = new Vector3(lastRootPosition.x + 1.2f, lastRootPosition.y, lastRootPosition.z);
            } 
            else
            {
                TreeEndRoot.transform.localPosition = new Vector3(-0.24f, 0,0);
            }
        }
    }
}
