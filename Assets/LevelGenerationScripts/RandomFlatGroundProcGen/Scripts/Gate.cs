using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
            Debug.Log(transform.name + " collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Tile"))
        {
            Destroy(collision.gameObject);
        }
    }
}
