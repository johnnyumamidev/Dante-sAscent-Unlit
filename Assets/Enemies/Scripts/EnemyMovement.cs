using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Enemy enemy;
    EnemyData data;

    EnemyDetection enemyDetection;
    Transform target;
    Rigidbody2D enemyRigidbody;

    public bool isFacingRight = false;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        data = enemy.enemyData;
        enemyDetection = GetComponent<EnemyDetection>();
        enemyRigidbody = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        if (enemyRigidbody.velocity.x < 0 && isFacingRight) Flip();
        else if (enemyRigidbody.velocity.x > 0 && !isFacingRight) Flip();
    }
    public void MoveTowardsTarget()
    {
        target = enemyDetection.player;
        Vector2 targetPosition = target.position;
        Vector2 enemyPosition = transform.position;
        Vector2 directionToTarget = new Vector2(targetPosition.x - enemyPosition.x, 0);

        enemyRigidbody.velocity = directionToTarget.normalized * data.speed * Time.deltaTime;
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        float xScale = transform.localScale.x;
        xScale *= -1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
    }
}
