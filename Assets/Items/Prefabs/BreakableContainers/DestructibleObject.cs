using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleObject : MonoBehaviour,IDamageable
{
    [SerializeField] SpriteMask spriteMask;
    [SerializeField] SpriteRenderer spriteRenderer;
    public float maxObjectHealth = 2f;
    float objectHealth;
    Rigidbody2D objectRigidbody;

    [SerializeField] GameObject damageMask;
    SpriteRenderer damageMaskRenderer;
    public float flashTime = 0.1f;
    public float damageEffectLength = 1f;
    [SerializeField] bool isHurt = false;

    [SerializeField] UnityEvent healthDepleted;
    [SerializeField] UnityEvent tookDamage;
    private void Awake()
    {
        damageMask.SetActive(false);
        damageMaskRenderer = damageMask.GetComponent<SpriteRenderer>();
        objectRigidbody = GetComponent<Rigidbody2D>();  
    }
    private void Start()
    {
        objectHealth = maxObjectHealth;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " took damage // remaining health: " + objectHealth);
        isHurt = true;
        tookDamage?.Invoke();
        objectHealth -= damage;
        StartCoroutine(HurtFlash());
    }
    float timer = 0;

    private void Update()
    {
        objectHealth = Mathf.Clamp(objectHealth, 0, maxObjectHealth);
        if (objectHealth <= 0)
        {
            healthDepleted?.Invoke();
        }

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
    private void LateUpdate()
    {
        damageMaskRenderer.sprite = spriteRenderer.sprite;
        spriteMask.sprite = spriteRenderer.sprite;
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
