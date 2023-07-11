using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationManager : MonoBehaviour, IEventListener
{
    Animator animator;
    PlayerInput playerInput;
    PlayerLocomotion playerLocomotion;
    PlayerHealth playerHealth;
    PlayerAttack playerAttack;
    public int currentHealth;
    public ArmorState[] armorState;
    [SerializeField] GameObject spriteObject;
    [SerializeField] Transform weaponPosition;
    SpriteRenderer spriteRenderer;

    public int animStateIndex = 0;
    public List<string> animStates = new List<string> { "Idle", "Run", "Jump", "Falling" };

    public Transform weaponHolder;

    private void Awake()
    {
        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        animator = spriteObject.GetComponent<Animator>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        playerHealth = GetComponentInParent<PlayerHealth>();
        if (playerInput == null) playerInput = GetComponentInParent<PlayerInput>();
        if (playerLocomotion == null) playerLocomotion = GetComponentInParent<PlayerLocomotion>();
    }

    private void LateUpdate()
    {
        if (spriteRenderer.sprite.name.Contains("fullArmor"))
        {
            string spriteName = spriteRenderer.sprite.name;
            spriteName = spriteName.Replace("fullArmor_", "");
            int spriteNumber = int.Parse(spriteName);

            if (currentHealth == 0) spriteRenderer.sprite = armorState[0].sprites[spriteNumber];
            else { spriteRenderer.sprite = armorState[currentHealth - 1].sprites[spriteNumber]; }
        }
    }

    private void Update()
    {
        currentHealth = (int)playerHealth.currentHealth;
        weaponHolder.transform.position = weaponPosition.position;

        DetermineAnimationStates();
        DetermineAttackState();

        animStateIndex = Mathf.Clamp(animStateIndex, 0, animStates.Count-1);
        animator.CrossFade(animStates[animStateIndex], 0, 0);
    }


    private void DetermineAnimationStates()
    {
        if (playerLocomotion.isGrounded && playerInput.movementInput.x != 0) { animStateIndex = 7; }
        else if (!playerLocomotion.isGrounded && !playerLocomotion.isNearChain) { animStateIndex = 2; }
        else if (playerLocomotion.isNearChain && playerInput.movementInput == Vector2.zero && playerLocomotion.isGrounded) { animStateIndex = 4; }
        else if (playerLocomotion.isClimbing && !playerLocomotion.isGrounded) { animStateIndex = 5; }
        else if (!playerLocomotion.isGrounded && playerLocomotion.isNearChain && !playerLocomotion.isClimbing) { animStateIndex = 6; }
        else { animStateIndex = 0; }

        if (playerLocomotion.isDodging)
        {
            animStateIndex = 8;
            return;
        }
        if (playerLocomotion.isClimbing)
        {
            animStateIndex = 5;
            return;
        }

        if (playerInput.performAttack != 0 && !playerAttack.currentWeapon.GetComponent<Weapon>().weaponData.isRangedWeapon)
        {
            isAttacking = true;
            if (comboReady) IncreaseComboStep();
        }

        if (playerHealth.playerHurtState) animStateIndex = 9;
        if (playerHealth.currentHealth == 0) animStateIndex = 10;
    }

    bool comboReady = false;
    [SerializeField] bool isAttacking = false;

    public float attackCooldownTime = 2f;
    public static int currentComboStep;
    public int maxComboAttacks;

    public void SetComboState(bool value)
    {
        comboReady = value;
        isAttacking = value;
        if (!comboReady) currentComboStep = 0;
        Debug.Log("combo ready: " + value + "/" + currentComboStep);
    }

    public void IncreaseComboStep()
    {
        currentComboStep++;
        comboReady = false;
    }
    private void DetermineAttackState()
    {
        playerAttack.isMeleeAttacking = isAttacking;
        if (!isAttacking) return;
        if (currentComboStep >= 0) animStateIndex = 11;
        if (currentComboStep >= 1) animStateIndex = 12;
    }

    [System.Serializable]
    public struct ArmorState
    {
        public Sprite[] sprites;
    }

    [SerializeField] GameEvent playerSpawnEvent;
    [SerializeField] UnityEvent playerSpawned;
    private void OnEnable()
    {
        playerSpawnEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        playerSpawnEvent.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        playerSpawned?.Invoke();
    }
}
