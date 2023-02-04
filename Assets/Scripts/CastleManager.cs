using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManager : MonoBehaviour
{
    public float health = 100;
    private float actualHealth;
    public List<GameObject> units = new List<GameObject>();
    private List<GameObject> usableUnits = new List<GameObject>();
    public int level = 1;
    public int neededWoodToLevelUp = 2;
    private int actualHaveWood = 0;
    public float spawnUnitsInterval = 5.0f;
    private float lastSpawnInterval = 0.0f;
    public Transform spawnPoint;
    private Tree tree;

    void Start()
    {
        tree = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
        actualHealth = health;
        foreach (GameObject unit in units)
        {
            UnitManager unitManager = unit.GetComponent<UnitManager>();
            if (unitManager != null && unitManager.neededlevel <= level)
            {
                usableUnits.Add(unit);
            }
        }
    }

    void Update()
    {
        if (lastSpawnInterval < Time.time)
        {
            lastSpawnInterval = Time.time + spawnUnitsInterval;
            SpawnUnits();
        }

        if (actualHaveWood >= neededWoodToLevelUp)
        {
            level++;
            actualHaveWood -= neededWoodToLevelUp;
            neededWoodToLevelUp = neededWoodToLevelUp * 2;
            usableUnits = new List<GameObject>();
            foreach (GameObject unit in units)
            {
                UnitManager unitManager = unit.GetComponent<UnitManager>();
                if (unitManager != null && unitManager.neededlevel <= level)
                {
                    usableUnits.Add(unit);
                }
            }
        }
    }

    public void GetWood()
    {
        actualHaveWood++;
    }

    public void SpawnUnits()
    {
        if (usableUnits.Count != 0) {
            Instantiate(usableUnits[Random.Range(0, usableUnits.Count)], spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AttackRoot")
        {
            actualHealth -= tree.damage;
            tree.roots.DestroyRoot();
            tree.AttackSound();
        }
    }
}
