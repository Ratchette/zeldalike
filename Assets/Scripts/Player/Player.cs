using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Walk,
    Attack,
    Interact,
    Stagger
}

public class Player : MonoBehaviour, IDamageable {
    static public string TAG = "Player";

    static private string ANIMATOR_MOVING = "moving";
    static private string ANIMATOR_MOVE_X = "moveX";
    static private string ANIMATOR_MOVE_Y = "moveY";

    static private string ANIMATOR_ATTACKING = "attacking";
    static private string ANIMATOR_DISPLAY_ITEM = "display_item";

    private PlayerState currentState;

    [Header("Health")]
    [SerializeField] private FloatValue health;
    [SerializeField] private SignalSender playerHealthSignal;
    [SerializeField] private SignalSender playerHitSignal;

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

    [Header("Knockback")]
    public float knockbackDuration = 0.25f;

    // Start is called before the first frame update
    void Start() {
        feet_collider = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        currentState = PlayerState.Walk;

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
        if (Input.GetButtonDown(InputMap.BUTTON_ATTACK) && GetState() == PlayerState.Walk) {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void FixedUpdate() {
        // Use this method for retreiving continuious updates & doing things that involve physics
        // Called every 0.02 seconds - which may be more or less than once a frame
        user_input = Vector3.zero;

        user_input.x = Input.GetAxisRaw(InputMap.INPUT_HORIZONTAL);
        user_input.y = Input.GetAxisRaw(InputMap.INPUT_VERTICAL);

        if (GetState() == PlayerState.Walk) {
            Move();
        }
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


    /****************************************************
     *  Movement
     ****************************************************/

    void Move() {
        if (user_input != Vector3.zero) {
            feet_collider.MovePosition(transform.position + user_input.normalized * speed * Time.deltaTime);

            animator.SetBool(ANIMATOR_MOVING, true);
            SetWalkAnimation(user_input);
        } else {
            animator.SetBool(ANIMATOR_MOVING, false);
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
        if (GetState() == PlayerState.Stagger) {
            return;
        }

        ChangeState(PlayerState.Stagger);

        Debug.LogFormat("[{0}][Hit] health={1}, damage={2}, newHealth={3}", "Player", health.runtimeValue, damage, (health.runtimeValue - damage));
        health.runtimeValue -= damage;
        playerHitSignal.Raise();
        playerHealthSignal.Raise();

        if (health.runtimeValue > 0) {
            StartCoroutine(KnockbackCoroutine(force, knockbackDuration));
        } else {
            // FIXME - throw up gameover screen
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        feet_collider.velocity = force;
        yield return new WaitForSeconds(duration);

        feet_collider.velocity = Vector2.zero;
        ChangeState(PlayerState.Walk);
    }


    /****************************************************
     *  Attacking
     ****************************************************/

    private IEnumerator AttackCoroutine() {
        animator.SetBool(ANIMATOR_ATTACKING, true);
        currentState = PlayerState.Attack;

        yield return null;

        animator.SetBool(ANIMATOR_ATTACKING, false);
        yield return new WaitForSeconds(0.33f);

        // FIXME - check if PlayerState != interact?
        currentState = PlayerState.Walk;
    }

    /****************************************************
     *  Ineraction
     ****************************************************/

    public void RaiseItem() {
        // Toggle between raising the item and making the player moveable again
        if (GetState() == PlayerState.Interact) {
            receivedItemSprite.sprite = null;
            animator.SetBool(ANIMATOR_DISPLAY_ITEM, false);
            currentState = PlayerState.Walk;

        } else {
            receivedItemSprite.sprite = playerInventory.GetItemToDisplay();
            animator.SetBool(ANIMATOR_DISPLAY_ITEM, true);
            currentState = PlayerState.Interact;
        }
    }
}
