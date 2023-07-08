using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDetection : MonoBehaviour, IEventListener
{
    Enemy enemy;
    EnemyData data;
    public Transform player;
    public bool playerFound = false;
    public bool attackReady = false;
    [SerializeField] GameEvent attackEnded;

    [SerializeField] GameObject exclamation;
    [SerializeField] float reactionTime;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        data = enemy.enemyData;
    }
    
    public void SearchForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, data.detectionRadius, data.detectionLayer);
        if (colliders.Length > 0 && !playerFound)
        {
            playerFound = true;
            StartCoroutine(ReactToPlayerFound(colliders));
        }
        else if(colliders.Length <= 0 && playerFound)
        {
            playerFound = false;
        }
    }
    IEnumerator ReactToPlayerFound(Collider2D[] colliders)
    {
        exclamation.SetActive(true);
        yield return new WaitForSeconds(reactionTime);
        exclamation.SetActive(false);
        foreach (Collider2D collider in colliders)
        {
            player = collider.transform;
        }
    }
    public void DetectTargetsWithinAttackRange()
    {
        Collider2D[] targetsInAttackRange = Physics2D.OverlapCircleAll(transform.position, data.attackRange, data.detectionLayer);

        if (targetsInAttackRange.Length > 0 && !attackReady)
        {
            attackReady = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }


    private void OnEnable()
    {
        attackEnded?.RegisterListener(this);
    }
    private void OnDisable()
    {
        attackEnded?.UnregisterListener(this);  
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == attackEnded) attackReady = false;
    }
}
