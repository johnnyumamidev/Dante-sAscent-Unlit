using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    PlayerHealth playerHealth;
    PlayerAttack playerAttack;
    public PlayerInput playerInput;
    public PlayerInventory playerInventory;
    PlayerInteraction playerInteraction;

    void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        playerInput = GetComponent<PlayerInput>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerHealth = GetComponent<PlayerHealth>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInventory.HandleInventory();
        playerInteraction.HandleInteraction();
        playerHealth.HandleHealth();
        playerAttack.HandleAllAttackActions();
        playerInput.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleDodge();
        if (playerAttack.isMeleeAttacking) return;
        playerLocomotion.HandleAllMovement();   
    }
}
