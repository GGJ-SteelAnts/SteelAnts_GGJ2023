using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRoots : MonoBehaviour
{
    public Tree Tree;

    public GameObject TreeRootPrefab;
    public GameObject TreeEndRoot;
    public List<GameObject> roots = new List<GameObject>();
    public List<GameObject> rootsTower = new List<GameObject>();

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
        rootsTower.Add(null);
    }

    public void DestroyRoot()
    {
        if (roots.Count > 0) {
            int id = roots.Count - 1;
            Destroy(roots[id]);
            roots.RemoveAt(id);
            if (rootsTower.Count > id) {
                Destroy(rootsTower[id]);
                rootsTower.RemoveAt(id);
            }
            if (roots.Count > 0) {
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
