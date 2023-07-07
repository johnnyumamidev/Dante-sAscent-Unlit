using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    Enemy enemy;
    EnemyData data;
    public Transform player;
    public bool playerWithinRange = false;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        data = enemy.enemyData;
    }
    
    public void SearchForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, data.detectionRadius, data.detectionLayer);

        foreach(Collider2D collider in colliders)
        {
            player = collider.transform;
        }
    }

    public void SetAttackRange()
    {
        Collider2D[] targetsInAttackRange = Physics2D.OverlapCircleAll(transform.position, data.attackRange, data.detectionLayer);

        if(targetsInAttackRange.Length > 0 )
        {
            playerWithinRange = true;
        }
        else
        {
            playerWithinRange = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }
}
