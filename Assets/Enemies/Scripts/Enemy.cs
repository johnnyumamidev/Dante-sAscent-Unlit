using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData enemyData;

    EnemyDetection enemyDetection;
    EnemyMovement enemyMovement;
    EnemyAttack enemyAttack;
    EnemyAnimation enemyAnimation;
    DestructibleObject destructibleData;
    private void Awake()
    {
        enemyDetection = GetComponent<EnemyDetection>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAttack= GetComponent<EnemyAttack>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        destructibleData = GetComponent<DestructibleObject>();

        transform.name = enemyData.enemyName;
        destructibleData.maxObjectHealth = enemyData.maxHealth;
    }

    private void Update()
    {
        PerformCurrentAction();
    }
    private void FixedUpdate()
    {
        PerformMovement();
    }

    private void PerformMovement()
    {
        if (enemyDetection?.player != null && !enemyDetection.attackReady)
        {
            enemyMovement.MoveTowardsTarget();
            enemyAnimation.animStateIndex = 2;
        }
    }

    private void PerformCurrentAction()
    {
        enemyDetection.DetectTargetsWithinAttackRange();

        if (enemyDetection?.player == null) 
        {
            enemyDetection.SearchForPlayer();
            enemyAnimation.animStateIndex = 1;
        }
        else if (enemyDetection.attackReady)
        {
            enemyAttack.DetectTargetWithinHitbox();
            enemyAnimation.animStateIndex = 3;
        }
        
        if (enemyDetection.playerFound && enemyDetection.player == null)
        {
            enemyAnimation.animStateIndex = 4;
        }
    }
}
