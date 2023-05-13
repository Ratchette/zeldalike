using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    walk,
    attack,
    interact,
    stagger
}

public class Player : MonoBehaviour, IDamageable {
    public static string TAG = "Player";

    public FloatValue health;
    public SignalSender playerHealthSignal;
    public SignalSender playerHitSignal;
    public PlayerState currentState;

    public SpriteRenderer receivedItemSprite;
    public Inventory playerInventory;

    private Animator animator;

    private Vector3 user_input;
    private Rigidbody2D feet_collider;
    public float speed;

    public Vector2Value startingPosition;
    public Vector2Value startingDirection;

    public float knockbackDuration = 0.25f;

    // Start is called before the first frame update
    void Start() {
        feet_collider = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();

        currentState = PlayerState.walk;

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
        if (Input.GetButtonDown("attack") && GetState() == PlayerState.walk) {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void FixedUpdate() {
        // Use this method for retreiving continuious updates & doing things that involve physics
        // Called every 0.02 seconds - which may be more or less than once a frame
        user_input = Vector3.zero;

        user_input.x = Input.GetAxisRaw("Horizontal");
        user_input.y = Input.GetAxisRaw("Vertical");

        if (GetState() == PlayerState.walk) {
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

            animator.SetBool("moving", true);
            SetWalkAnimation(user_input);
        } else {
            animator.SetBool("moving", false);
        }
    }

    private void SetWalkAnimation(Vector2 v) {
        animator.SetFloat("moveX", v.x);
        animator.SetFloat("moveY", v.y);
    }


    /****************************************************
     *  Damage
     ****************************************************/

    public void TakeDamage(Vector2 force, float damage) {
        if (GetState() == PlayerState.stagger) {
            return;
        }

        ChangeState(PlayerState.stagger);

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
        ChangeState(PlayerState.walk);
    }


    /****************************************************
     *  Attacking
     ****************************************************/

    private IEnumerator AttackCoroutine() {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;

        yield return null;

        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.33f);

        // FIXME - check if PlayerState != interact?
        currentState = PlayerState.walk;
    }

    /****************************************************
     *  Ineraction
     ****************************************************/

    public void RaiseItem() {
        // Toggle between raising the item and making the player moveable again
        if (GetState() == PlayerState.interact) {
            receivedItemSprite.sprite = null;
            animator.SetBool("display_item", false);
            currentState = PlayerState.walk;

        } else {
            receivedItemSprite.sprite = playerInventory.GetItemToDisplay();
            animator.SetBool("display_item", true);
            currentState = PlayerState.interact;
        }
    }
}
