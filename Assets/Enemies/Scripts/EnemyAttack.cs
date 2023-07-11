using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] Transform hitbox;
    public float hitboxRadius;
    public LayerMask playerLayer;
    [SerializeField] bool hitboxEnabled = true;
    [SerializeField] float hitboxCooldownTime = 1f;

    public float attackKnockback = 500f;
    Vector2 knockbackDirection= Vector2.zero;

    [SerializeField] UnityEvent onHit;
    [SerializeField] UnityEvent onHitboxActive;

    public void DetectTargetWithinHitbox()
    {
        Collider2D[] _hitbox = Physics2D.OverlapCircleAll(hitbox.position, hitboxRadius, playerLayer);

        if (_hitbox.Length > 0 && hitboxEnabled)
        {
            foreach(Collider2D collider in _hitbox)
            {
                PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth.isInvincible) return;
                if (playerHealth != null) playerHealth.TakeDamage();

                Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
                Vector2 position = transform.position;
                Vector2 targetPos = collider.transform.position;
                knockbackDirection = targetPos - position;
                if (playerRb != null) playerRb.AddForce(knockbackDirection * attackKnockback * 1000);
            }
            hitboxEnabled = false;
            onHit?.Invoke();
            StartCoroutine(ActivateHitbox());
        }
    }

    private IEnumerator ActivateHitbox()
    {
        WaitForSeconds cooldown = new WaitForSeconds(hitboxCooldownTime);
        while (!hitboxEnabled)
        {
            yield return cooldown;
            hitboxEnabled = true;
            onHitboxActive?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitbox.position, hitboxRadius);
    }
}
