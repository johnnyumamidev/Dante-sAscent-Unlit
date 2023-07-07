using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    EnemyDetection enemyDetection;
    EnemyMovement enemyMovement;
    DestructibleObject destructibleData;
    private void Awake()
    {
        transform.name = enemyData.enemyName;

        enemyDetection = GetComponent<EnemyDetection>();
        enemyMovement = GetComponent<EnemyMovement>();
        destructibleData = GetComponent<DestructibleObject>();

        destructibleData.maxObjectHealth = enemyData.maxHealth;
    }

    private void Update()
    {
        enemyDetection.SetAttackRange();
        PerformCurrentAction();
    }

    private void PerformCurrentAction()
    {
        if(enemyDetection?.player == null) 
        {
            enemyDetection.SearchForPlayer();
        }
        else if (enemyDetection.playerWithinRange)
        {
            Debug.Log("attack target!!");
        }
        else
        {
            enemyMovement.MoveTowardsTarget();
        }
    }
}
