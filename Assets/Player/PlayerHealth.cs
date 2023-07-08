using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IEventListener
{
    [Header("GameEvents")]
    [SerializeField] GameEvent playerDeathEvent;
    [SerializeField] GameEvent playerDamageEvent;
    [SerializeField] UnityEvent playerDamage;

    [Header("Health")]
    public PlayerData playerData;
    float maxHealth;
    public float currentHealth;
    public bool playerHurtState = false;
    bool isInvincible;
    [SerializeField] float hurtTimer;

    public GameObject bloodParticles;

    void Awake()
    {
        maxHealth = playerData.maxPlayerHealth;
        SetHealthToFull();
    }

    private void SetHealthToFull()
    {
        currentHealth = maxHealth;
    }

    public void HandleHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        HandleHurtState();
        HandlePlayerDeath();
    }

    private void HandlePlayerDeath()
    {
        if (currentHealth <= 0)
        {
            isInvincible = true;
            playerDeathEvent.Raise();
        }
    }

    private void HandleHurtState()
    {
        isInvincible = false;
        if (playerHurtState)
        {
            isInvincible = true;
            bloodParticles.SetActive(true);
            hurtTimer += Time.deltaTime;
        }
        else { hurtTimer = 0; }
        if (hurtTimer >= playerData.hurtStateTime) playerHurtState = false;
    }
    public Vector2 forceDirection = Vector2.up;
    public float knockbackForce = 15f;
    public void TakeDamage()
    {
        if (!isInvincible)
        {
            currentHealth--;
            playerHurtState = true;
        }
    }

    public void Retry()
    {
        SetHealthToFull();
    }

    private void OnEnable()
    {
        playerDamageEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerDamageEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        playerDamage?.Invoke();
    }
}
