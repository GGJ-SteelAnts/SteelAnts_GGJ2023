using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tree : MonoBehaviour
{
    public int HP = 100;
    public int actualHP;
    public int Stamina = 100;
    private int actualStamina;
    public enum Buildings {Nothing, Roots, Tree, Spikes};
    public int[] buildingsPrice = { 20, 5, 10 };
    public Texture2D towerBuildingCursor;
    private string status = "life"; //life, halfDead, dead
    public List<Sprite> view = new List<Sprite>();
    private SpriteRenderer treeView;
    public GrowRoots roots;
    private bool building = false;
    private Buildings buildingTower = Buildings.Nothing;

    void Start()
    {
        treeView = this.GetComponent<SpriteRenderer>();
        actualHP = HP;
        actualStamina = Stamina;
    }

    void Update()
    {
        if (actualHP <= 0)
        {
            status = "dead";
        }

        TreeAnimation();
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

    public void GetDamage(int damage)
    {
        if (roots != null && roots.roots.Count > 0)
        {
            roots.DestroyRoot();
        }
        actualHP -= damage;
    }

    public void TreeBuild(int tower)
    {
        if (tower == (int)Buildings.Roots)
        {
            if (actualStamina - buildingsPrice[0] >= 0) {
                ConsumeEnergy(buildingsPrice[0]);
                roots.SpawnRoot();
            }
        } 
        else if(tower == (int)Buildings.Tree)
        {
            buildingTower = Buildings.Tree;
            building = true;
            Cursor.SetCursor(towerBuildingCursor, new Vector2(towerBuildingCursor.width / 2, towerBuildingCursor.height / 2), CursorMode.Auto);
        }
    }
}
