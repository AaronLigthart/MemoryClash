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

    public void Initialize(Vector3 spawnPosition, string unitType, string unitAllience, int unitHealth, int unitAttackDamage, float unitAttackSpeed, float unitWalkingSpeed)
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
            case "attack":
                Attack();
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
    }
    public void Stop()
    {
        velocity *= 0.85f;
        if (velocity.x <= 0.01 && velocity.x > -0.01)
        {
            velocity.x = 0;
            ChangeState("idle");
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
    public void Attack()
    {
        if (allience == "player" && UnitSpawner.isMyTurn)
        {
            print("player attacks enemy");
            UnitSpawner.enemyUnitList[0].GetComponent<Unit>().GetDamage(attackDamage);
        }
        else if (allience == "enemy" && !UnitSpawner.isMyTurn)
        {
            print("Enemy attacks player");
            UnitSpawner.myUnitList[0].GetComponent<Unit>().GetDamage(attackDamage);
        }
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
            UnitSpawner.myUnitList.Remove(this.gameObject);
            Destroy(this.gameObject);
            UnitSpawner.isMovingOut = true;
        }
        if (allience == "enemy")
        {
            UnitSpawner.enemyUnitList.Remove(this.gameObject);
            Destroy(this.gameObject);
            UnitSpawner.isMovingOut = true;
        }
    }
}
