using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : Item, IEventListener
{
    [SerializeField] GameEvent attackActiveEvent;
    [SerializeField] GameEvent attackInactiveEvent;
    [SerializeField] GameEvent playerFacingRight;
    [SerializeField] GameEvent playerFacingLeft;

    public LayerMask enemylayer;
    public WeaponData weaponData;
    SpriteRenderer spriteRenderer;

    bool attackActive = false;
    public float attackActiveTime = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponData.weaponSprite;
    }
    protected override void Update()
    {
        base.Update();
        if(playerInteraction != null)
        {
            transform.position = playerInteraction.weaponHoldPosition.position;
            transform.rotation = playerInteraction.weaponHoldPosition.rotation;
        }
    }

    private void OnEnable()
    {
        if(attackActiveEvent != null) attackActiveEvent.RegisterListener(this);
        if(attackInactiveEvent != null) attackInactiveEvent.RegisterListener(this);
        if (playerFacingRight != null)
            playerFacingRight.RegisterListener(this);
        if (playerFacingLeft != null)
            playerFacingLeft.RegisterListener(this);
    }
    private void OnDisable()
    {
        attackActiveEvent?.UnregisterListener(this);
        attackInactiveEvent?.UnregisterListener(this);
        playerFacingRight?.UnregisterListener(this);
        playerFacingLeft?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        if (gameEvent == attackActiveEvent) attackActive = true;
        if (gameEvent == attackInactiveEvent) attackActive = false;
        if (gameEvent == playerFacingRight) spriteRenderer.sortingOrder = 2;
        if (gameEvent == playerFacingLeft) spriteRenderer.sortingOrder = 0;
    }
}
