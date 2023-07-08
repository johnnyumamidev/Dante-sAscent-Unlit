using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEventListener
{
    [SerializeField] Transform hitbox;
    public float hitboxRadius;
    public LayerMask playerLayer;
    [SerializeField] bool hitboxEnabled = false;
    [SerializeField] GameEvent enableHitbox;
    [SerializeField] GameEvent attackEnded;

    public float attackKnockback = 500f;
    Vector2 knockbackDirection= Vector2.zero;
    public void DetectTargetWithinHitbox()
    {
        Collider2D[] _hitbox = Physics2D.OverlapCircleAll(hitbox.position, hitboxRadius, playerLayer);

        if (_hitbox.Length > 0 && hitboxEnabled)
        {
            foreach(Collider2D collider in _hitbox)
            {
                PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null) playerHealth.TakeDamage();

                Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
                Vector2 position = transform.position;
                Vector2 targetPos = collider.transform.position;
                knockbackDirection = targetPos - position;
                if (playerRb != null) playerRb.AddForce(knockbackDirection * attackKnockback * 1000);
            }
            hitboxEnabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitbox.position, hitboxRadius);
    }
    private void OnEnable()
    {
        enableHitbox?.RegisterListener(this);
        attackEnded?.RegisterListener(this);
    }
    private void OnDisable()
    {
        enableHitbox?.UnregisterListener(this); 
        attackEnded?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == attackEnded) hitboxEnabled = false;
        if (gameEvent == enableHitbox) hitboxEnabled = true;
    }
}
