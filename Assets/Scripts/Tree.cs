using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tree : MonoBehaviour
{
    public int HP = 100;
    [HideInInspector]public int actualHP;
    public int Stamina = 100;
    [HideInInspector]public int actualStamina;
    public int damage = 20;
    public enum Buildings {Nothing, Roots, Tree, Spikes};
    public int[] buildingsPrice = { 20, 5, 10 };
    public Texture2D towerBuildingCursor;
    public Texture2D towerHoverCursor;
    private string status = "life"; //life, halfDead, dead
    public List<Sprite> view = new List<Sprite>();
    private SpriteRenderer treeView;
    public GrowRoots roots;
    public int lifeOfRoot = 2;
    private int actualLifeOfRoot;
    public bool building = false;
    private Buildings buildingTower = Buildings.Nothing;
    public AudioSource MainAudioSource;
    public AudioClip MainNormalAudio;
    public AudioClip MainHalfAudio;
    public AudioSource audioSource;
    public AudioClip attackAudio;
    public List<AudioClip> buildAudios = new List<AudioClip>();

    public List<GameObject> buildingsButtons = new List<GameObject>();
    public List<GameObject> towersPrefab = new List<GameObject>();

    void Start()
    {
        actualLifeOfRoot = lifeOfRoot;
        treeView = this.GetComponent<SpriteRenderer>();
        actualHP = HP;
        actualStamina = Stamina;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        if (actualHP <= 0)
        {
            status = "dead";
        }

        if (actualHP <= HP /2)
        {
            if (MainAudioSource != null)
            {
                MainAudioSource.clip = MainHalfAudio;
            }
        } 
        else
        {
            if (MainAudioSource != null)
            {
                MainAudioSource.clip = MainNormalAudio;
            }
        }
        if (!MainAudioSource.isPlaying)
        {
            MainAudioSource.Play();
        }

        TreeAnimation();

        for (int i = 0; i < buildingsButtons.Count; i++)
        {
            if (building)
            {
                buildingsButtons[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                if (actualStamina - buildingsPrice[i] < 0)
                {
                    buildingsButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    buildingsButtons[i].GetComponent<Button>().interactable = true;
                }
            }
        }

        if (building == true)
        {
            if (Input.GetMouseButton(1)) {
                buildingTower = Buildings.Nothing;
                building = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
            if (hit.collider != null && hit.collider.gameObject.tag == "Root")
            {
                bool canBuild = true;
                if (Input.GetMouseButton(0))
                {
                    for (int i = 0; i < roots.roots.Count; i++)
                    {
                        if (roots.roots[i] == hit.collider.gameObject && roots.rootsTower[i] == null)
                        {
                            if (audioSource != null)
                            {
                                audioSource.PlayOneShot(buildAudios[Random.Range(0, buildAudios.Count)]);
                            }
                            GameObject tower = Instantiate(towersPrefab[(int)buildingTower - 1], roots.gameObject.transform);
                            tower.transform.localPosition = roots.roots[i].transform.localPosition;
                            roots.rootsTower[i] = tower;
                            ConsumeEnergy(buildingsPrice[(int)buildingTower - 1]);
                            buildingTower = Buildings.Nothing;
                            building = false;
                            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                            break;
                        } 
                    }
                }
                for (int i = 0; i < roots.roots.Count; i++)
                {
                    if (roots.roots[i] == hit.collider.gameObject && roots.rootsTower[i] != null)
                    {
                        canBuild = false;
                    }
                }
                if (building == true && canBuild) {
                    Cursor.SetCursor(towerHoverCursor, new Vector2(towerBuildingCursor.width / 2, towerBuildingCursor.height / 2), CursorMode.ForceSoftware);
                }
            } 
            else
            {
                if (building == true) {
                    Cursor.SetCursor(towerBuildingCursor, new Vector2(towerBuildingCursor.width / 2, towerBuildingCursor.height / 2), CursorMode.ForceSoftware);
                }
            }
        }
    }

    public void TreeAnimation()
    {
        if (status == "dead")
        {
            if (view.Count > 2)
            {
                treeView.sprite = view[2];
            }
            return;
        }
        if (actualHP < HP / 2)
        {
            if (view.Count > 1) {
                treeView.sprite = view[1];
            }
            return;
        }
        if (view.Count > 0)
        {
            treeView.sprite = view[0];
        }
    }

    public void ConsumeEnergy(int energyTake)
    {
        actualStamina -= energyTake;
    }

    public void AttackSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(attackAudio);
        }
    }

    public void GetDamage(int damage)
    {
        if (roots != null && roots.roots.Count > 0)
        {
            if (actualLifeOfRoot > 0)
            {
                actualLifeOfRoot--;
            } 
            else
            {
                roots.DestroyRoot();
                actualLifeOfRoot = lifeOfRoot;
            }
        }
        actualHP -= damage;
    }

    public void TreeBuild(int tower)
    {
        if (tower == (int)Buildings.Roots)
        {
            if (actualStamina - buildingsPrice[0] >= 0) {
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(buildAudios[Random.Range(0, buildAudios.Count)]);
                }
                ConsumeEnergy(buildingsPrice[0]);
                roots.SpawnRoot();
            }
        } 
        else if(tower == (int)Buildings.Tree)
        {
            buildingTower = Buildings.Tree;
            building = true;
            Cursor.SetCursor(towerBuildingCursor, new Vector2(towerBuildingCursor.width / 2, towerBuildingCursor.height / 2), CursorMode.ForceSoftware);
        }
        else if (tower == (int)Buildings.Spikes)
        {
            buildingTower = Buildings.Spikes;
            building = true;
            Cursor.SetCursor(towerBuildingCursor, new Vector2(towerBuildingCursor.width / 2, towerBuildingCursor.height / 2), CursorMode.ForceSoftware);
        }
    }
}
