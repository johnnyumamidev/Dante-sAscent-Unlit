using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestructibleObject : MonoBehaviour,IDamageable
{
    [SerializeField] SpriteMask spriteMask;
    [SerializeField] SpriteRenderer spriteRenderer;
    public float maxObjectHealth = 2f;
    float objectHealth;
    Rigidbody2D objectRigidbody;

    [SerializeField] GameObject damageMask;
    public float flashTime = 0.1f;
    public float damageEffectLength = 1f;
    [SerializeField] bool isHurt = false;
    private void Awake()
    {
        objectHealth = maxObjectHealth;
        damageMask.SetActive(false);
        objectRigidbody = GetComponent<Rigidbody2D>();  
    }
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took damage");
        isHurt = true;
        objectHealth -= damage;
        StartCoroutine(HurtFlash());
    }
    float timer = 0;

    private void Update()
    {
        spriteMask.sprite = spriteRenderer.sprite;
        objectHealth = Mathf.Clamp(objectHealth, 0, maxObjectHealth);
        if(objectHealth <= 0) Destroy(gameObject);
        if (isHurt)
        {
            timer += Time.deltaTime;
            if(timer >= damageEffectLength) isHurt = false;
        }
        else
        {
            timer = 0;
        }
    }
    IEnumerator HurtFlash()
    {
        WaitForSeconds flash = new WaitForSeconds(flashTime);

        while (isHurt)
        {
            damageMask.SetActive(true);
            yield return flash;
            damageMask.SetActive(false);
            yield return flash;
        }
    }
}
