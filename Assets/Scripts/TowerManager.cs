using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public Tree.Buildings type = Tree.Buildings.Nothing;
    public int health = 60;
    private int actualHealth;
    public int damage = 0;
    public int slower = 0;
    public bool poison = false;
    public float damageRate = 0.5f;
    private float actualDamageRate;
    private Tree tree;

    void Start()
    {
        tree = GameObject.FindGameObjectWithTag("Tree").GetComponent<Tree>();
        actualHealth = health;
    }

    void Update()
    {
        if (actualHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void GetDamage(int damageTake)
    {
        actualHealth -= damageTake;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (type == Tree.Buildings.Spikes) {
                UnitManager unit = collision.gameObject.GetComponent<UnitManager>();
                if (unit.status != "die") {
                    actualDamageRate = Time.time + damageRate;
                    unit.GetDamage(damage);
                    GetDamage(damage);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (type == Tree.Buildings.Spikes)
            {
                if (actualDamageRate < Time.time) {
                    UnitManager unit = collision.gameObject.GetComponent<UnitManager>();
                    if (unit.status != "die")
                    {
                        actualDamageRate = Time.time + damageRate;
                        unit.GetDamage(damage);
                        GetDamage(damage);
                    }
                }
            }

            if (type == Tree.Buildings.Tree)
            {
                if (actualDamageRate < Time.time)
                {
                    UnitManager unit = collision.gameObject.GetComponent<UnitManager>();
                    if (unit.status != "die")
                    {
                        actualDamageRate = Time.time + damageRate;
                        tree.ConsumeEnergy(-(int)((float)unit.energy * 0.2f));
                    }
                }
            }
        }
    }
}
