using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject spawnpointLeft;
    public GameObject spawnpointRight;
    public GameObject unitPrefab;
    public static List<GameObject> myUnitList = new List<GameObject>();
    public static List<GameObject> enemyUnitList = new List<GameObject>();
    public static bool isMovingOut = true;
    public static bool isMyTurn = true;


    public void Spawn(int[] myUnits, int[] enemyUnits)
    {
        for(int i = 0; i<= myUnits.Length; i++)
        {
            var unit = Instantiate(unitPrefab);
            myUnitList.Add(unit);
            unit.GetComponent<Unit>().Initialize(spawnpointLeft.transform.position - new Vector3(i * 1.2f,0,0),"sword","player", 5, 1, 1, 0.06f);
            
        }
        for (int i = 0; i <= enemyUnits.Length; i++)
        {
            var unit = Instantiate(unitPrefab);
            enemyUnitList.Add(unit);
            unit.GetComponent<Unit>().Initialize(spawnpointRight.transform.position + new Vector3(i * 1.2f, 0, 0), "sword", "enemy", 5, 1, 1, -0.06f);
        }
        MoveOut();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingOut)
        {
            CheckDistance(myUnitList[0], enemyUnitList[0], 2);
        }
    }

    public void MoveOut()
    {
        isMovingOut = true;
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
        isMovingOut = false;


    }

    public void CheckDistance(GameObject unit1, GameObject unit2, float minDistance) {
        float distance = Vector3.Distance(unit1.transform.position, unit2.transform.position);
        if (distance <= minDistance)
        {
            StopMoving();
        }
    }
}
