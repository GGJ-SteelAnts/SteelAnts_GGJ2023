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
    public List<Sprite> walkAnimations = new List<Sprite>();
    public List<Sprite> deathAnimations = new List<Sprite>();
    public List<Sprite> walkWithWoodAnimations = new List<Sprite>();
    public float timeToCutWood = 2.0f;
    private float actualTimeToCutWood;
    public float animationTime = 0.1f;
    private float actualAnimationTime = 0.0f;
    private int animationIndex = 0;
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
            animationIndex = 0;
        }

        if (actualHealth <= 0)
        {
            status = "die";
            haveWood = false;
            animationIndex = 0;
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
            if (actualAnimationTime < Time.time)
            {
                if (deathAnimations.Count > animationIndex + 1)
                {
                    animationIndex++;
                }
                actualAnimationTime = Time.time + animationTime;
            }
            if (deathAnimations.Count > animationIndex)
            {
                UnitView.sprite = deathAnimations[animationIndex];
            }
            return;
        }

        if (status == "cut" || status == "attack")
        {
            if (actualAnimationTime < Time.time)
            {
                if (cutAnimations.Count > animationIndex + 1)
                {
                    animationIndex++;
                } 
                else
                {
                    animationIndex = 0;
                }
                actualAnimationTime = Time.time + animationTime;
            }
            if (cutAnimations.Count > animationIndex)
            {
                UnitView.sprite = cutAnimations[animationIndex];
            }
            return;
        }
        if (haveWood)
        {
            if (actualAnimationTime < Time.time)
            {
                if (walkWithWoodAnimations.Count > animationIndex + 1)
                {
                    animationIndex++;
                }
                else
                {
                    animationIndex = 0;
                }
                actualAnimationTime = Time.time + animationTime;
            }
            if (walkWithWoodAnimations.Count > animationIndex)
            {
                UnitView.sprite = walkWithWoodAnimations[animationIndex];
            }
            return;
        }
        if (actualAnimationTime < Time.time)
        {
            if (walkAnimations.Count > animationIndex + 1)
            {
                animationIndex++;
            }
            else
            {
                animationIndex = 0;
            }
            actualAnimationTime = Time.time + animationTime;
        }
        if (walkAnimations.Count > animationIndex)
        {
            UnitView.sprite = walkAnimations[animationIndex];
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
            animationIndex = 0;
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
