using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CastleManager : MonoBehaviour
{
    public float health = 100;
    private float actualHealth;
    public List<GameObject> units = new List<GameObject>();
    private List<GameObject> usableUnits = new List<GameObject>();
    public List<GameObject> bosses = new List<GameObject>();
    public int level = 1;
    public int neededWoodToLevelUp = 2;
    private int actualHaveWood = 0;
    public float spawnUnitsInterval = 5.0f;
    private float lastSpawnInterval = 0.0f;
    public Transform spawnPoint;
    private Tree tree;
    public SpriteRenderer castleView;
    public Sprite castleUnderHalfHealth;
    public SpriteRenderer woodView;
    public List<Sprite> woodsView = new List<Sprite>();
    public List<AudioClip> upgradeAudios = new List<AudioClip>();
    public List<AudioClip> stackWoodAudios = new List<AudioClip>();
    public AudioSource audioSource;
    public int unitsBeforBoss = 40;
    public int unitCounter = 0;
    private GameObject bossComming;
    public float delaySpawn = 7.0f;

    void Start()
    {
        lastSpawnInterval = Time.time + delaySpawn;
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
        if (actualHealth <= health/2)
        {
            castleView.sprite = castleUnderHalfHealth;
        }

        if (actualHealth <= 0)
        {
            PlayerPrefs.SetInt("victory", 1);
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

        if (lastSpawnInterval < Time.time)
        {
            lastSpawnInterval = Time.time + spawnUnitsInterval - (tree.roots.roots.Count * (bossComming != null ? 0.01f : 0.3f));
            unitCounter++;
            if (unitCounter >= unitsBeforBoss) {
                unitCounter = 0;
                SpawnBoss();
            } else {
                SpawnUnits();
            }
        }

        if (actualHaveWood >= neededWoodToLevelUp)
        {
            if (audioSource != null && upgradeAudios.Count > 0)
            {
                audioSource.PlayOneShot(upgradeAudios[Random.Range(0, upgradeAudios.Count)]);
            }
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

        if (woodsView.Count > actualHaveWood && actualHaveWood >= 0)
        {
            woodView.sprite = woodsView[actualHaveWood];
        }
        else
        {
            woodView.sprite = woodsView[0];
        }
    }

    public void GetWood()
    {
        if (audioSource != null && stackWoodAudios.Count > 0)
        {
            audioSource.PlayOneShot(stackWoodAudios[Random.Range(0, stackWoodAudios.Count)]);
        }
        actualHaveWood++;
    }

    public void SpawnUnits()
    {
        if (usableUnits.Count != 0) {
            Instantiate(usableUnits[Random.Range(0, usableUnits.Count)], spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void SpawnBoss()
    {
        if (bosses.Count != 0)
        {
            bossComming = Instantiate(bosses[Random.Range(0, bosses.Count)], spawnPoint.position, spawnPoint.rotation);
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
