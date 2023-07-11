using System;
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
    DestructibleObject destructibleObj;

    bool noHealthRemaining;
    bool stunned = false;
    [SerializeField] float stunDuration;

    private void Awake()
    {
        enemyDetection = GetComponent<EnemyDetection>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAttack= GetComponent<EnemyAttack>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        destructibleObj = GetComponent<DestructibleObject>();

        transform.name = enemyData.enemyName;
        destructibleObj.maxObjectHealth = enemyData.maxHealth;
    }

    private void Update()
    {
        if (noHealthRemaining)
        {
            enemyAnimation.animStateIndex = 5;
            LeaveCorpseBehind();
            return;
        }
        if (stunned)
        {
            StartCoroutine(StunTimer());
            return;
        }
        DeterminePrimaryAction();
        PerformPassiveActions();
    }

    private void FixedUpdate()
    {
        if (noHealthRemaining) return;
        if (stunned) return;
        PerformMovement();
    }
    private void DeterminePrimaryAction()
    {
        if (enemyDetection?.player == null)
        {
            enemyDetection.SearchForPlayer();
            enemyAnimation.animStateIndex = 0;
        }

        if (enemyDetection.playerFound && enemyDetection.player == null)
        {
            enemyAnimation.animStateIndex = 4;
        }
    }
    private void PerformPassiveActions()
    {
        enemyDetection.ShootPlayerDetectionRaycast();
        enemyAttack.DetectTargetWithinHitbox();
    }

    private void PerformMovement()
    {
        if (enemyDetection?.player != null)
        {
            enemyMovement.MoveTowardsTarget(enemyDetection.player);
            enemyAnimation.animStateIndex = 2;
        }
        
        if (enemyDetection.paused)
        {
            enemyMovement.PauseMovement();
            enemyAnimation.animStateIndex = 1;
        }
    }

    private void LeaveCorpseBehind()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Collider2D col = GetComponent<Collider2D>();
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        col.isTrigger = true;
    }

    private IEnumerator StunTimer()
    {
        while (stunned)
        {
            yield return new WaitForSeconds(stunDuration);
            stunned = false;
        }
    }

    //          UNITY EVENTS            //
    //===================================//
    public void NoHealth()
    {
        noHealthRemaining = true;
    }

    public void StunOnDamageTaken()
    {
        stunned = true;
    }
    //===================================//

}
