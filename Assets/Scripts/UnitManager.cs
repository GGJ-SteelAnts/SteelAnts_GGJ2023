using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public int health = 100;
    private int actualHealth;
    public int neededlevel = 0;
    public int damage = 15;
    public int energy = 10;
    public string status = "walk"; //walk,cut,attack,die
    [HideInInspector]public bool walk = true;
    public float walkSpeed = 5.0f;
    [HideInInspector]public bool haveWood = false;
    private SpriteRenderer UnitView;
    public List<Sprite> unitSprites = new List<Sprite>();
    public List<Sprite> cutAnimations = new List<Sprite>();
    public float timeToCutWood = 2.0f;
    private float actualTimeToCutWood;
    public float animationCutTime = 0.1f;
    private float actualCutTime = 0.0f;
    private int cutIndex = 0;
    private Tree tree;

    void Start()
    {
        actualHealth = health;
        UnitView = this.GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        UnitMove();
        UnitAnimation();
        gameObject.transform.localScale = new Vector3((haveWood ? -Mathf.Abs(gameObject.transform.localScale.x) : Mathf.Abs(gameObject.transform.localScale.x)), gameObject.transform.localScale.y, gameObject.transform.localScale.z);

        if (status == "cut" && actualTimeToCutWood < Time.time)
        {
            tree.GetDamage(damage);
            status = "walk";
            haveWood = true;
            cutIndex = 0;
        }

        if (actualHealth <= 0)
        {
            status = "die";
            haveWood = false;
            cutIndex = 0;
        }
    }

    private void UnitMove()
    {
        if (status == "walk")
        {
            transform.position += (haveWood ? transform.right : -transform.right) * walkSpeed * Time.deltaTime;
        }
    }

    private void UnitAnimation()
    {
        if (status == "die")
        {
            if (unitSprites.Count > 2)
            {
                UnitView.sprite = unitSprites[2];
            }
            return;
        }

        if (status == "cut" || status == "attack")
        {
            if (actualCutTime < Time.time)
            {
                if (cutAnimations.Count > cutIndex + 1)
                {
                    cutIndex++;
                } 
                else
                {
                    cutIndex = 0;
                }
                actualCutTime = Time.time + animationCutTime;
            }
            if (cutAnimations.Count > cutIndex)
            {
                UnitView.sprite = cutAnimations[cutIndex];
            }
            return;
        }
        if (haveWood)
        {
            if (unitSprites.Count > 1)
            {
                UnitView.sprite = unitSprites[1];
            }
            return;
        }
        if (unitSprites.Count > 0)
        {
            UnitView.sprite = unitSprites[0];
        }
    }

    public void GetDamage(int damage)
    {
        actualHealth -= damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tree")
        {
            actualTimeToCutWood = Time.time + timeToCutWood;
            status = "cut";
            tree = other.gameObject.GetComponent<Tree>();
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
