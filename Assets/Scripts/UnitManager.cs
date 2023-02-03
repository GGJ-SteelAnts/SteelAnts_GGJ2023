using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public float health = 100;
    private float actualHealth;
    public int neededlevel = 0;
    [HideInInspector]public bool walk = true;
    public float walkSpeed = 5.0f;
    [HideInInspector]public bool haveWood = false;
    private SpriteRenderer UnitView;

    void Start()
    {
        actualHealth = health;
        UnitView = this.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (walk)
        {
            transform.position += (haveWood ? transform.right : -transform.right) * walkSpeed * Time.deltaTime;
        }
        gameObject.transform.localScale = new Vector3((haveWood ? -Mathf.Abs(gameObject.transform.localScale.x) : Mathf.Abs(gameObject.transform.localScale.x)), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Tree")
        {
            haveWood = true;
        }

        if (other.gameObject.tag == "Castle")
        {
            if (haveWood)
            {
                CastleManager castle = other.gameObject.GetComponent<CastleManager>();
                castle.GetWood();
                haveWood = false;
            }
        }
    }
}
