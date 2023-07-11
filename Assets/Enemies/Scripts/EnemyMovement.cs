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

    public void MoveTowardsTarget(Transform _target)
    {
        target = _target;
        Vector2 targetPosition = target.position;
        Vector2 enemyPosition = transform.position;
        Vector2 directionToTarget = new Vector2(targetPosition.x - enemyPosition.x, 0);

        enemyRigidbody.velocity = directionToTarget.normalized * data.speed * Time.deltaTime;
    }

    public void PauseMovement()
    {
        enemyRigidbody.velocity = Vector2.zero;
    }

    public void FlipTransform()
    {
        isFacingRight = !isFacingRight;
        float xScale = transform.localScale.x;
        xScale *= -1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, 0);
    }
}
