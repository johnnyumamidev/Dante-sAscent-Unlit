using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackForces : MonoBehaviour
{
    public float force = 10f;
    [SerializeField] PlayerLocomotion playerLocomotion;
    [SerializeField] Rigidbody2D playerRigidbody;
    Vector2 forceDirection;
    private void Awake()
    {
        if(playerLocomotion == null) playerLocomotion = GetComponentInParent<PlayerLocomotion>();
    }
    void Update()
    {
        if (playerLocomotion.facingRight) forceDirection = Vector2.right;
        else { forceDirection = Vector2.left; }
    }
    public void AddForce()
    {
        playerRigidbody.AddForce(forceDirection * force, ForceMode2D.Impulse);
    }
}
