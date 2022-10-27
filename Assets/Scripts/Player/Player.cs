using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    walk,
    attack,
    interact,
    stagger
}

public class Player : MonoBehaviour {
    public FloatValue health;
    public SignalSender playerHealthSignal;
    public PlayerState currentState;

    private Animator animator;

    private Vector3 user_input;
    private Rigidbody2D feet_collider;
    public float speed;

    public Vector2Value startingPosition;
    public Vector2Value startingDirection;


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
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger) {
            StartCoroutine(AttackCoroutine());
        }
    }

    private void FixedUpdate() {
        // Use this method for retreiving continuious updates & doing things that involve physics
        // Called every 0.02 seconds - which may be more or less than once a frame
        user_input = Vector3.zero;

        user_input.x = Input.GetAxisRaw("Horizontal");
        user_input.y = Input.GetAxisRaw("Vertical");

        if(currentState == PlayerState.walk) {
            UpdateAnimationAndMove();
        }
    }

    protected PlayerState ChangeState(PlayerState newState) {
        if (currentState != newState) {
            PlayerState previousState = currentState;
            currentState = newState;
            return previousState;
        } else {
            return currentState;
        }
    }


    private IEnumerator AttackCoroutine() {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;

        yield return null;

        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.33f);

        currentState = PlayerState.walk;
    }


    void UpdateAnimationAndMove() {
        if (user_input != Vector3.zero) {
            MoveCharacter();

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

    void MoveCharacter() {
        feet_collider.MovePosition(transform.position + user_input.normalized * speed * Time.deltaTime);
    }

    public void Hit(Vector2 force, float duration, float damage) {
        if(currentState == PlayerState.stagger) {
            return;
        }
        ChangeState(PlayerState.stagger);

        TakeDamage(damage);
        if (health.runtimeValue > 0) {
            StartCoroutine(KnockbackCoroutine(force, duration));
        } else {
            this.gameObject.SetActive(false);
        }
    }

    private void TakeDamage(float damage) {
        Debug.LogFormat("[{0}][Hit] health={1}, damage={2}, newHealth={3}", "Player", health.runtimeValue, damage, (health.runtimeValue - damage));
        health.runtimeValue -= damage;
        playerHealthSignal.Raise();
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        feet_collider.velocity = force;
        yield return new WaitForSeconds(duration);

        feet_collider.velocity = Vector2.zero;
        ChangeState(PlayerState.walk);
    }
}
