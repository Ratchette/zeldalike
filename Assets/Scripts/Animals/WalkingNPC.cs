using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalkingNPC : Interactable {
    static private string ANIMATOR_MOVING = "moving";
    static private string ANIMATOR_MOVE_X = "moveX";
    static private string ANIMATOR_MOVE_Y = "moveY";

    private Animator animator;
    private Rigidbody2D myRigidbody;
    [SerializeField] private BoxCollider2D bounds;

    [Header("Walking")]
    [SerializeField] private float speed = 4;
    [SerializeField] private float walkMaxDuration = 3;
    [SerializeField] private float walkChance = 0.02f;

    private float walkUntil = -1;
    private Vector2 direction = Vector2.zero;

    private void Start() {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if (playerInRange) {
            return;
        }

        if(direction == Vector2.zero) {
            MaybeStartWalk();
        } else if (walkUntil >= Time.time) {
            ContinueWalk();
        } else { // walkUntil < Time.time
            StopWalk();
        }
 
    }

    private void MaybeStartWalk() {
        if (Random.value >= walkChance) {
            return;
        }

        walkUntil = Time.time + Random.value * walkMaxDuration;

        direction = ((Vector2)bounds.bounds.center - myRigidbody.position).normalized;
        direction.x += Random.Range(-0.5f, 0.5f);
        direction.y += Random.Range(-0.5f, 0.5f);
        direction = direction.normalized;

        Vector2 animationDirection;
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
            animationDirection.y = 0;
            if(direction.x >= 0) {
                animationDirection.x = 1;
            } else {
                animationDirection.x = -1;
            }
        } else {
            animationDirection.x = 0;
            if (direction.y > 0) {
                animationDirection.y = 1;
            } else {
                animationDirection.y = -1;
            }
        }

        SetWalkAnimation(animationDirection);
        animator.SetBool(ANIMATOR_MOVING, true);
    }

    private void ContinueWalk() {
        Vector2 nextPosition = myRigidbody.position + direction * speed * Time.deltaTime;

        if (bounds.bounds.Contains(nextPosition)) {
            myRigidbody.MovePosition(nextPosition);
        } else {
            StopWalk();
        }
    }

    private void StopWalk() {
        walkUntil = 0;
        direction = Vector2.zero;

        animator.SetBool(ANIMATOR_MOVING, false);
    }

    private void SetWalkAnimation(Vector2 v) {
        animator.SetFloat(ANIMATOR_MOVE_X, v.x);
        animator.SetFloat(ANIMATOR_MOVE_Y, v.y);
    }

    new protected void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        if(other.CompareTag(Player.TAG) && !other.isTrigger) {
            StopWalk();
        }
    }

    protected override void Interact() {}
}
