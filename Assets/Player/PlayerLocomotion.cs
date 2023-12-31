using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerInteraction playerInteraction;
    PlayerHealth playerHealth;
    public PlayerData playerData;
    Rigidbody2D rigidBody;

    public bool isGrounded;
    public bool isDodging;
    public float dodgeRollCooldownLength = 1f;
    public float dodgeRollStartup = 0.3f;
    [SerializeField] Vector2 dodgeDirection = new Vector2(1, 0.5f);
    [SerializeField] float invincibilityTime;
    float _moveSpeed;
    [SerializeField]private int remainingJumps;
    [SerializeField] float timeSinceFalling;
    float landingRecoveryTimer;
    float aerialDrift;
    public bool isClimbing;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] Collider2D climbChainCollider;
    Collider2D chain;
    public bool isNearChain;
    [Header("Facing Right Check")]
    public bool facingRight = true;
    [SerializeField] GameEvent playerFacingRight;
    [SerializeField] GameEvent playerFacingLeft;
    Vector2 playerPosition;

    private void Awake()
    {
        if(playerCollider == null) playerCollider = GetComponent<Collider2D>();
        rigidBody= GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        playerPosition = transform.position;
        HandleGravity(playerData.playerGravityScale);
    }

    private void HandleGravity(float setGravity)
    {
        float _gravityScale = setGravity;
        //if climbing
        if (!isGrounded && chain != null && climbChainCollider.IsTouching(chain)) _gravityScale = 0;

        if (isOnSlope && playerInput.movementInput == Vector2.zero)
        {
            transform.position = playerPosition;
            _gravityScale = 0;
        }
        rigidBody.gravityScale = _gravityScale;
        rigidBody.mass = playerData.playerMass;

    }

    public void HandleAllMovement()
    {
        if (isDodging) return; 
        else
        {
            HandleWalking();
            HandleJump();
            HandleClimbing();
        }
        HandleHurtLaunch(playerHealth.playerHurtState);
    }

    public void HandleDodge()
    {
        if (playerInput.performDodge != 0 && !isDodging && isGrounded) 
        {
            StartCoroutine(DodgeRollCooldown());
        }

        if (isDodging)
        {
            Collider2D[] wallCheck = Physics2D.OverlapCircleAll(playerInteraction.interactionHitbox.position, 0.25f, playerData.groundLayer);

            if(wallCheck.Length > 0)
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }
    }
    private IEnumerator DodgeRollCooldown()
    {
        isDodging = true;
        playerHealth.isInvincible = true;
        yield return new WaitForSeconds(dodgeRollStartup);

        if ((!facingRight && dodgeDirection.x > 0) || (facingRight && dodgeDirection.x < 0))
            dodgeDirection.x *= -1;
        
        rigidBody.AddForce(dodgeDirection * playerData.dodgeForce, ForceMode2D.Impulse);
        playerCollider.isTrigger = true;
        yield return new WaitForSeconds(invincibilityTime);
        playerCollider.isTrigger = false;
        while (isDodging)
        {
            yield return new WaitForSeconds(dodgeRollCooldownLength);
            isDodging = false;
            playerHealth.isInvincible = false;
        }
    }

    private void HandleHurtLaunch(bool hurtState)
    {
        if (!hurtState) return;
        Vector2 hurtForceDirection;
        if(facingRight) hurtForceDirection = Vector2.right;
        else { hurtForceDirection = Vector2.left; }
        Vector2 horizontalForce = new Vector2(hurtForceDirection.x, 0);
        rigidBody.velocity = (Vector2.up + horizontalForce) * playerData.hurtLaunchForce;
    }

    private void HandleClimbing()
    {
        isNearChain = false;
        isClimbing = false;
        if (chain == null) return;
        if (!climbChainCollider.IsTouching(chain)) return;
        isNearChain = true;
        if (!isGrounded && rigidBody.velocity.y != 0) rigidBody.velocity = Vector2.zero; 
        if (Mathf.Abs(playerInput.movementInput.y) > 0.4f) isClimbing = true;
        if (playerInput.performJump != 0 && !isGrounded) rigidBody.AddRelativeForce(playerData.relaseChainForce * playerInput.movementInput, ForceMode2D.Impulse);
        if (!isClimbing) return;

        rigidBody.velocity = Vector2.up * playerInput.movementInput.y * playerData.climbSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Chain"))
        {
            chain = collision;
        }
    }
    private void FlipDirection()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        if (facingRight) playerFacingRight.Raise();
        if (!facingRight) playerFacingLeft.Raise();
    }

    private void HandleJump()
    {
        remainingJumps = Mathf.Clamp(remainingJumps, 0, playerData.maxJumps);
        if (playerInput.performJump == 0 && rigidBody.velocity.y > 0 && timeSinceFalling >= playerData.jumpReleaseTimeAllowance)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -1);
        }
        if (!isGrounded)
        {
            timeSinceFalling += Time.deltaTime;
            aerialDrift = playerInput.movementInput.x * playerData.aerialDriftModifier;
            if (chain != null && climbChainCollider.IsTouching(chain)) return;
            //aerial drift ->
            if (playerInput.movementInput.x != 0 && Mathf.Abs(rigidBody.velocity.x) <= playerData.maxAerialSpeed) { rigidBody.velocity += new Vector2(aerialDrift, 0); }
            return;
        }
        
        if (remainingJumps <= 0) return;

        if (playerInput.performJump != 0 && remainingJumps > 0 && playerInput.movementInput.y >= -0.7)
        {
            isGrounded = false;
            remainingJumps--;
            rigidBody.AddRelativeForce(Vector2.up * playerData.jumpForce, ForceMode2D.Impulse);
        }
    }

    bool isOnSlope = false;
    public float slopeModifier;
    private void HandleWalking()
    {
        _moveSpeed = playerData.moveSpeed;
        float playerMoveInput = playerInput.movementInput.normalized.x;
        if (isOnSlope && playerInput.movementInput.x != 0)
        {
            if (!facingRight && slopeModifier > 0) slopeModifier *= -1;
            else if (facingRight && slopeModifier < 0) slopeModifier *= -1;
            playerMoveInput += slopeModifier;
        }
        float currentSpeed = playerMoveInput * _moveSpeed * Time.fixedDeltaTime;
        float clampedSpeed = Mathf.Clamp(currentSpeed, -playerData.maxSpeed, playerData.maxSpeed);
        Vector2 velocity = new Vector2(clampedSpeed, rigidBody.velocity.y);
        if (!isGrounded) return;
        if (playerInput.movementInput.x < 0 && facingRight) FlipDirection();
        else if(playerInput.movementInput.x > 0 && !facingRight) FlipDirection();
        rigidBody.velocity = velocity;
    }
    bool onOneWayPlatform = false;
    GameObject oneWayPlatform;

    public void HandleGroundedCheck()
    {
        Collider2D footCollider = Physics2D.OverlapCircle(playerPosition + playerData.groundCheckOffset, playerData.groundCheckRadius);
        isGrounded = footCollider;

        if (isGrounded)
        {
            timeSinceFalling = 0;
            landingRecoveryTimer += Time.fixedDeltaTime;

            if (footCollider.CompareTag("Slope") && playerCollider.IsTouching(footCollider)) isOnSlope = true;
            else { isOnSlope = false; }

            if (footCollider.CompareTag("Enemy")) Debug.Log("*** BOUNCE ***");
        }
        

        if (playerCollider.IsTouchingLayers(playerData.platform))
        {
            onOneWayPlatform = true;
            if(footCollider) oneWayPlatform = footCollider.gameObject;
            Debug.Log("player is on a platform: " + oneWayPlatform.name);
        }
        else { onOneWayPlatform = false; }

        if(landingRecoveryTimer >= playerData.jumpCooldown)
        {
            remainingJumps = playerData.maxJumps;
            landingRecoveryTimer = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerPosition + playerData.groundCheckOffset, playerData.groundCheckRadius);
    }
}
