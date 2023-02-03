using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public float health = 100;
    private float actualHealth;
    public int neededlevel = 0;
    public bool walk = true;
    public float walkSpeed = 5.0f;

    void Start()
    {
        actualHealth = health;
    }

    void Update()
    {
        if (walk)
        {
            transform.position += -transform.right * walkSpeed * Time.deltaTime;
        }
    }
}
