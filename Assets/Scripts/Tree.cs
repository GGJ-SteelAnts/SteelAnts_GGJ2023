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
    public enum Buildings {Roots, Tree};
    public int[] buildingsPrice = { 20, 10 };
    private string status = "life"; //life, halfDead, dead
    public List<Sprite> view = new List<Sprite>();
    private SpriteRenderer treeView;
    public GrowRoots roots;

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

    public void TreeBuild(int building)
    {
        if (building == (int)Buildings.Roots)
        {
            if (actualStamina - buildingsPrice[0] >= 0) {
                ConsumeEnergy(buildingsPrice[0]);
                roots.SpawnRoot();
            }
        }
    }
}
