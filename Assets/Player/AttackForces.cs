using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackForces : MonoBehaviour
{
    public float force = 10f;
    [SerializeField] Rigidbody2D playerRigidbody;
    void Start()
    {
        
    }
    public void AddForce()
    {
        Debug.Log("add force on attack");
        playerRigidbody.AddForce(Vector2.right * force, ForceMode2D.Impulse);
    }
}
