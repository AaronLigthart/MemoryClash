using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string currentState;
    public string type;
    public string allience;
    public int health;
    public int attackDamage;
    public float attackSpeed;
    public float walkingSpeed;
    public Vector3 position;
    public Vector3 velocity;
    public bool isStopping;
    public bool isDead = false;
    public UnitSpawner unitSpawner;
    public Sprite[] image;

    public void Initialize(UnitSpawner controller, Vector3 spawnPosition, string unitType, string unitAllience, float walkSpeed)
    {
        type = unitType;
        allience = unitAllience;
        transform.position = spawnPosition;
        walkingSpeed = walkSpeed;
        velocity = new Vector3(walkingSpeed, 0, 0);
        isStopping = false;
        unitSpawner = controller;
        
        print(type);
        SetUnit(type);
        


    }
    public void SetUnit(string unitType)
    {
        switch (unitType)
        {
            case "soldier":
                health = 5;
                attackDamage = 1;
                gameObject.GetComponent<SpriteRenderer>().sprite = image[0];
                break;
            case "tank":
                health = 8;
                attackDamage = 2;
                gameObject.GetComponent<SpriteRenderer>().sprite = image[1];

                break;
            case "mage":
                health = 2;
                attackDamage = 5;
                gameObject.GetComponent<SpriteRenderer>().sprite = image[2];

                break;
            case "archer":
                health = 3;
                attackDamage = 3;
                gameObject.GetComponent<SpriteRenderer>().sprite = image[3];

                break;
            default:
                Debug.LogError("No unit named " + unitType);
                break;
        }

    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case "walk":
                Walk();
                break;
            case "stop":
                Stop();
                break;
            case "idle":
                Idle();
                break;
            case "defend":
                Defend();
                break;
            case "die":
                Die();
                break;
            case "special":
                Special();
                break;
            default:
                Debug.LogWarning("NO current state for unit");
                break;
        }
    }
    public void Walk()
    {
        this.transform.position += velocity;
        isStopping = true;

    }
    public void Stop()
    {
        velocity *= 0.85f;
        if (velocity.x <= 0.01 && velocity.x > -0.01)
        {
            velocity.x = 0;
            ChangeState("idle");
            isStopping = false;
            unitSpawner.isAttacking = true;
        }
        this.transform.position += velocity;

    }
    public void Idle()
    {
        velocity = new Vector3(walkingSpeed, 0, 0);
    }
    public void Defend()
    {

    }
    public void Attack(Unit enemy)
    {
        enemy.GetDamage(attackDamage);   
    }
    public void GetDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            ChangeState("die");
        }
    }

    public void Special() { }


    public void ChangeState(string unitState)
    {
        this.currentState = unitState;
    }
    void Die()
    {
        Debug.LogWarning("dead");
        if (allience == "player")
        {
            unitSpawner.myUnitList.Remove(this.gameObject);
            Destroy(this.gameObject);
            unitSpawner.isMovingOut = true;
        }
        if (allience == "enemy")
        {
            unitSpawner.enemyUnitList.Remove(this.gameObject);
            Destroy(this.gameObject);
            unitSpawner.isMovingOut = true;
        }
        unitSpawner.StopAttacking();
    }
}
