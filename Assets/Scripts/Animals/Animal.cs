using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public enum AnimalState {
    Idle,
    Walk,
    Stagger,
    Flee
}

public class Animal : MonoBehaviour, IDamageable {
    static protected string ANIMATOR_MOVING = "moving";
    static protected string ANIMATOR_MOVE_X = "moveX";
    static protected string ANIMATOR_MOVE_Y = "moveY";

    protected Rigidbody2D myRigidbody;
    protected Transform player;
    protected Animator animator;
    protected Coroutine walkCoroutine = null;
    protected Coroutine fleeCoroutine = null;

    private AnimalState currentState = AnimalState.Idle;

    [SerializeField] private float speed;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.25f;
    protected float _knockbackMultiplier = 1;

    

    protected void Start() {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag(Player.TAG).transform;

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat(ANIMATOR_MOVE_X, 0);
        animator.SetFloat(ANIMATOR_MOVE_Y, -1);
    }

    public AnimalState GetState() {
        return currentState;
    }

    protected AnimalState ChangeState(AnimalState newState) {
        if (currentState != newState) {
            AnimalState previousState = currentState;
            currentState = newState;
            return previousState;
        } else {
            return currentState;
        }
    }

    protected void SetWalkAnimation(Vector3 delta) {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
            if (delta.x <= 0) {
                SetWalkAnimationXY(Vector2.left);
            } else {
                SetWalkAnimationXY(Vector2.right);
            }
        } else {
            if (delta.y <= 0) {
                SetWalkAnimationXY(Vector2.down);
            } else {
                SetWalkAnimationXY(Vector2.up);
            }
        }
    }

    protected void SetWalkAnimationXY(Vector2 v) {
        animator.SetFloat(ANIMATOR_MOVE_X, v.x);
        animator.SetFloat(ANIMATOR_MOVE_Y, v.y);
    }


    protected IEnumerator WalkCoroutine(Vector2 velocity, float duration) {
        ChangeState(AnimalState.Walk);
        animator.SetBool(ANIMATOR_MOVING, true);

        SetWalkAnimation(velocity);
        myRigidbody.velocity = velocity;

        yield return new WaitForSeconds(duration);

        animator.SetBool(ANIMATOR_MOVING, false);
        ChangeState(AnimalState.Idle);
        myRigidbody.velocity = Vector3.zero;
        walkCoroutine = null;
    }

    // Animals stagger, but take no damage by default
    public void TakeDamage(Vector2 force, float damage) {
        if (GetState() == AnimalState.Stagger) {
            return;
        }
        ChangeState(AnimalState.Stagger);

        if(walkCoroutine != null) {
            StopCoroutine(walkCoroutine);
            walkCoroutine = null;
        }
        if (fleeCoroutine != null) {
            StopCoroutine(fleeCoroutine);
            fleeCoroutine = null;
        }

        StartCoroutine(KnockbackCoroutine(force, knockbackDuration));
    }

    protected IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        myRigidbody.velocity = force * _knockbackMultiplier;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(AnimalState.Walk);
    }
}



