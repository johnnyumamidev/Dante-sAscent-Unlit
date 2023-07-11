using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDetection : MonoBehaviour
{
    Enemy enemy;
    EnemyData data;
    EnemyMovement enemyMovement;
    public Transform player;
    public bool playerFound = false;
    public bool paused = false;
    public bool searching = false;

    [SerializeField] GameObject exclamation;
    [SerializeField] float reactionTime;
    [SerializeField] float pauseDuration;
    [SerializeField] float transitionTime;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyMovement = GetComponent<EnemyMovement>();
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
    IEnumerator PauseBeforeTurnAround(Vector2 direction)
    {
        paused = true;
        searching = true;
        exclamation.SetActive(true);
        while (paused)
        {
            yield return new WaitForSeconds(pauseDuration);
            if (direction.x > 0 && !enemyMovement.isFacingRight)
            {
                enemyMovement.FlipTransform();
            }
            else if (direction.x < 0 && enemyMovement.isFacingRight)
            {
                enemyMovement.FlipTransform();
            }
            exclamation.SetActive(false);
            yield return new WaitForSeconds(transitionTime);
            paused = false;
            searching = false;
        }
    }
    public void ShootPlayerDetectionRaycast()
    {
        Vector2 pos = transform.position;
        if (player != null)
        {
            Vector2 playerPos = player.position;
            Vector2 directionToPlayer = playerPos - pos;

            Vector2 castPoint = new Vector2(transform.position.x, transform.position.y + FOVOffset);
            Vector2 rayDirection = Vector2.zero;
            bool playerBehind = (playerPos.x > 0 && !enemyMovement.isFacingRight) || (playerPos.x < 0 && enemyMovement.isFacingRight);
            if(enemyMovement.isFacingRight) rayDirection = Vector2.right;
            else { rayDirection = Vector2.left; }
            RaycastHit2D ray = Physics2D.Raycast(castPoint, rayDirection, data.detectionRadius, data.detectionLayer);

            if (!ray && playerBehind)
            {
                StartCoroutine(PauseBeforeTurnAround(directionToPlayer));
            }
        }
    }


    public void PauseDetection(bool active)
    {
        paused = active;
    }

    [SerializeField] float FOVOffset = 0.3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, data.detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.attackRange);
    }

}
