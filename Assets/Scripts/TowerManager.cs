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

    void Start()
    {
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
                unit.GetDamage(damage);
                GetDamage(damage);
            }
        }
    }
}
