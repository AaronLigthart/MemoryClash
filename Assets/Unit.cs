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
    public int walkingDirection;
    public float walkingSpeed;
    public Vector3 position;
    public Vector3 velocity;
    public bool isStopping;
    public bool isDead = false;
    public  UnitSpawner unitSpawner;

    public void Initialize(UnitSpawner controller, Vector3 spawnPosition, string unitType, string unitAllience, int unitHealth, int unitAttackDamage, float unitAttackSpeed, float unitWalkingSpeed)
    {
        type = unitType;
        allience = unitAllience;
        health = unitHealth;
        attackDamage = unitAttackDamage;
        attackSpeed = unitAttackSpeed;
        walkingSpeed = unitWalkingSpeed;
        transform.position = spawnPosition;
        velocity = new Vector3(walkingSpeed, 0, 0);
        isStopping = false;
        unitSpawner = controller;
        
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
