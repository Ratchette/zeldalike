using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState {
    Walk,
    Attack,
    Interact,
    Stagger,
    Invincible,
    Dead
}

[RequireComponent(typeof(PlayerHealth), typeof(GameOver))]
public class Player : MonoBehaviour, IDamageable {
    static public string TAG = "Player";

    static private string ANIMATOR_MOVING = "moving";
    static private string ANIMATOR_MOVE_X = "moveX";
    static private string ANIMATOR_MOVE_Y = "moveY";

    static private string ANIMATOR_ATTACKING = "attacking";
    static private string ANIMATOR_DISPLAY_ITEM = "display_item";

    static private string ANIMATOR_DEATH = "death";

    private PlayerState currentState;
    private Health health;
    private GameOver gameOver;
    private Attack attack;

    [SerializeField] private BooleanValue hasSword;

    [Header("Items")]
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private SpriteRenderer receivedItemSprite;

    [Header("Movement")]
    [SerializeField] private Vector2Value startingPosition;
    [SerializeField] private Vector2Value startingDirection;
    [SerializeField] private float speed;
    private Rigidbody2D feet_collider;
    private Animator animator;
    private Vector3 user_input;

    [Header("Invincibility")]
    [SerializeField] private float flashDuration = 0.4f;
    [SerializeField] private int numFlashes = 3;

    private Color flashColor = new Color(0.6320754f, 0.2593895f, 0.2593895f, 0.8235294f);
    private Color defaultColor = new Color(1, 1, 1, 1);
    private SpriteRenderer playerSprite;
    [SerializeField] private SignalSender playerHitSignal;

    [Header("Knockback")]
    public float knockbackDuration = 0.25f;

    // Start is called before the first frame update
    void Start() {
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        feet_collider = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        health = gameObject.GetComponent<Health>();
        health.Reset();

        gameOver = gameObject.GetComponent<GameOver>();

        ChangeState(PlayerState.Walk);

        // Vector2.zero == a position hasn't been set, so use the default for that scene.
        if (!startingPosition.runtimeValue.Equals(Vector2.zero)) {
            transform.position = startingPosition.runtimeValue;
            SetWalkAnimation(startingDirection.runtimeValue);
        } else {
            SetWalkAnimation(Vector2.down);
        }
    }

    // Update is called once per frame
    void Update() {
        if(!CanControlPlayer()) {
            return;
        }

        if (Input.GetButtonDown(InputMap.BUTTON_ATTACK) && hasSword.runtimeValue) {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void FixedUpdate() {
        if (!CanControlPlayer()) {
            return;
        }

        // Use this method for retreiving continuious updates & doing things that involve physics
        // Called every 0.02 seconds - which may be more or less than once a frame
        user_input.x = Input.GetAxisRaw(InputMap.INPUT_HORIZONTAL);
        user_input.y = Input.GetAxisRaw(InputMap.INPUT_VERTICAL);
        Move();
    }


    /****************************************************
     *  State Management
     ****************************************************/

    private PlayerState GetState() {
        return currentState;
    }

    private PlayerState ChangeState(PlayerState newState) {
        if (currentState != newState) {
            PlayerState previousState = currentState;
            currentState = newState;
            return previousState;
        } else {
            return currentState;
        }
    }

    bool CanControlPlayer() {
        return (currentState == PlayerState.Walk) || (currentState == PlayerState.Invincible);
    }


    /****************************************************
     *  Movement
     ****************************************************/

    void Move() {
        if (user_input != Vector3.zero)  {
            feet_collider.MovePosition(transform.position + user_input.normalized * speed * Time.fixedDeltaTime);

            animator.SetBool(ANIMATOR_MOVING, true);
            SetWalkAnimation(user_input);
        } else {
            animator.SetBool(ANIMATOR_MOVING, false);
            feet_collider.velocity = Vector2.zero;
        }
    }

    private void SetWalkAnimation(Vector2 v) {
        animator.SetFloat(ANIMATOR_MOVE_X, v.x);
        animator.SetFloat(ANIMATOR_MOVE_Y, v.y);
    }


    /****************************************************
     *  Damage
     ****************************************************/

    public void TakeDamage(Vector2 force, float damage) {
        if(GetState() == PlayerState.Stagger || GetState() == PlayerState.Invincible) {
            return;
        }

        bool isAlive = health.Damage(damage);
        ChangeState(PlayerState.Stagger);

        playerHitSignal.Raise();

        if (isAlive) {
            StartCoroutine(KnockbackCoroutine(force, knockbackDuration));
        } else {
            Die();
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {

        StartCoroutine(InvincibilityCoroutine());

        feet_collider.velocity = force;
        yield return new WaitForSeconds(duration);

        feet_collider.velocity = Vector2.zero;
        ChangeState(PlayerState.Invincible);
    }

    private IEnumerator InvincibilityCoroutine() {
        int flashes = 0;

        while (flashes < numFlashes) {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration/2);

            playerSprite.color = defaultColor;
            yield return new WaitForSeconds(flashDuration/2);

            ++flashes;
        }

        ChangeState(PlayerState.Walk);
    }

    public void Die() {
        ChangeState(PlayerState.Dead);
        feet_collider.bodyType = RigidbodyType2D.Static;
        animator.SetBool(ANIMATOR_ATTACKING, false);
        animator.SetBool(ANIMATOR_MOVING, false);
        animator.SetBool(ANIMATOR_DEATH, true);

        gameOver.Play();
    }

    /****************************************************
     *  Attacking
     ****************************************************/

    private IEnumerator AttackCoroutine() {
        animator.SetBool(ANIMATOR_ATTACKING, true);
        ChangeState(PlayerState.Attack);
        yield return null;

        animator.SetBool(ANIMATOR_ATTACKING, false);
        yield return new WaitForSeconds(0.33f);

        ChangeState(PlayerState.Walk);
    }

    /****************************************************
     *  Ineraction
     ****************************************************/

    public void RaiseItem() {
        // Toggle between raising the item and making the player moveable again
        if (GetState() == PlayerState.Interact) {
            receivedItemSprite.sprite = null;
            animator.SetBool(ANIMATOR_DISPLAY_ITEM, false);
            ChangeState(PlayerState.Walk);

        } else {
            receivedItemSprite.sprite = playerInventory.GetItemToDisplay();
            animator.SetBool(ANIMATOR_DISPLAY_ITEM, true);
            currentState = PlayerState.Interact;
        }
    }
}
