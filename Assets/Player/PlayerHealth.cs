using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IEventListener
{
    [Header("GameEvents")]
    [SerializeField] GameEvent playerDeathEvent;
    [SerializeField] GameEvent playerDamageEvent;
    [SerializeField] GameEvent invincibilityEnabled;
    [SerializeField] GameEvent invincibilityDisabled;
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
    public void TakeDamage()
    {
        if (!isInvincible)
        {
            currentHealth--;
            playerHurtState = true;
        }
    }
    public void ActivateInvincibility(bool active)
    {
        isInvincible = active;
    }
    public void Retry()
    {
        SetHealthToFull();
    }

    private void OnEnable()
    {
        playerDamageEvent.RegisterListener(this);
        invincibilityEnabled?.RegisterListener(this);
        invincibilityDisabled?.RegisterListener(this);  
    }
    private void OnDisable()
    {
        playerDamageEvent.UnregisterListener(this);
        invincibilityEnabled?.UnregisterListener(this);
        invincibilityDisabled?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if(gameEvent == invincibilityEnabled) { ActivateInvincibility(true); }
        if(gameEvent == invincibilityDisabled) { ActivateInvincibility(false); }
        if(gameEvent == playerDamageEvent) playerDamage?.Invoke();
    }
}
