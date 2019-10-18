using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject spawnpointLeft;
    public GameObject spawnpointRight;
    public GameObject unitPrefab;
    public List<GameObject> myUnitList = new List<GameObject>();
    public List<GameObject> enemyUnitList = new List<GameObject>();
    public bool isMovingOut = true;
    public bool isMyTurn = true;
    public bool isAttacking = false;
    public Coroutine lastEnumerator;

    public void Spawn(int[] myUnits, int[] enemyUnits)
    {
        for(int i = 0; i<= myUnits.Length; i++)
        {
            var unit = Instantiate(unitPrefab);
            myUnitList.Add(unit);
            setUnit(myUnits)
            unit.GetComponent<Unit>().Initialize(this, spawnpointLeft.transform.position - new Vector3(i * 1.2f,0,0),"sword","player", 5, 1, 1, 0.06f);
            
        }
        for (int i = 0; i <= enemyUnits.Length; i++)
        {
            var unit = Instantiate(unitPrefab);
            enemyUnitList.Add(unit);
            unit.GetComponent<Unit>().Initialize(this,spawnpointRight.transform.position + new Vector3(i * 1.2f, 0, 0), "sword", "enemy", 5, 1, 1, -0.06f);
        }
        MoveOut();
    }

    public void StopAttacking()
    {
        StopAllCoroutines();
        MoveOut();
        isMovingOut = true;
        isAttacking = false;
    }



    public IEnumerator AttackPhase()
    {
        yield return new WaitForSeconds(1);


        while (myUnitList.Count != 0 || enemyUnitList.Count != 0)
        {
            if (isMyTurn)
            {
                print("attacking enemie");
                myUnitList[0].GetComponent<Unit>().Attack(enemyUnitList[0].GetComponent<Unit>());
                isMyTurn = false;
                yield return new WaitForSeconds(1);
            }
            else if (!isMyTurn)
            {
                print("attacking player");
                enemyUnitList[0].GetComponent<Unit>().Attack(myUnitList[0].GetComponent<Unit>());

                isMyTurn = true;
                
                yield return new WaitForSeconds(1);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (isMovingOut)
        {
            CheckDistance(myUnitList[0], enemyUnitList[0], 2);
        }
        else if (isAttacking && !myUnitList[0].GetComponent<Unit>().isStopping)
        {
            isAttacking = false;
            StartCoroutine(AttackPhase());
        }

    }

    public void MoveOut()
    {
        foreach (GameObject unit in myUnitList)
        {
            unit.GetComponent<Unit>().ChangeState("walk");
        }
        foreach (GameObject unit in enemyUnitList)
        {
            unit.GetComponent<Unit>().ChangeState("walk");
        }
    }
    public void StopMoving()
    {
        foreach (GameObject unit in myUnitList)
        {
            unit.GetComponent<Unit>().ChangeState("stop");
        }
        foreach (GameObject unit in enemyUnitList)
        {
            unit.GetComponent<Unit>().ChangeState("stop");
        }
    }

    public void CheckDistance(GameObject unit1, GameObject unit2, float minDistance) {
        float distance = Vector3.Distance(unit1.transform.position, unit2.transform.position);
        if (distance <= minDistance)
        {
            StopMoving();
            isMovingOut = false;
            isAttacking = false;
        }
    }
}
