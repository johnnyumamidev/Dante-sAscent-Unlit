using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableDetector : MonoBehaviour
{
    public float force = 25f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage();
        }
    }
}
