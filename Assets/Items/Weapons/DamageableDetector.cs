using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableDetector : MonoBehaviour
{
    [SerializeField] Transform player;
    public float force = 25f;
    public float hitboxRadius = 0.5f;

    public float damage;
    bool damageDealt = false;
    public float damageTimer = 0.25f;

    Vector2 forceDirection = Vector2.zero;
    private void OnEnable()
    {
        damageDealt = false;
    }
    private void Update()
    {
        DetectTargetsWithinHitbox();
    }

    private void DetectTargetsWithinHitbox()
    {
        Collider2D[] hitbox = Physics2D.OverlapCircleAll(transform.position, hitboxRadius);

        if (hitbox.Length > 0 && !damageDealt)
        {
            DealDamage(hitbox);
            damageDealt = true;
        }
    }

    private void DealDamage(Collider2D[] hitbox)
    {
        foreach (Collider2D col in hitbox)
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }

            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 enemyPosition = col.transform.position;
                Vector2 playerPosition = player.position;
                Vector2 directionToEnemy = new Vector2(enemyPosition.x - playerPosition.x, 0);
                Rigidbody2D enemyRigidbody = col.GetComponent<Rigidbody2D>();
                if (directionToEnemy.x > 0) forceDirection = Vector2.right;
                else { forceDirection = Vector2.left; }
                enemyRigidbody.AddForce(forceDirection * force * 100, ForceMode2D.Impulse);
                Debug.Log("enemy hit: " + directionToEnemy);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitboxRadius);
    }
}
