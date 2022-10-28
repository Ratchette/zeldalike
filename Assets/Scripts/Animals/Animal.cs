using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public enum AnimalState {
    idle,
    walk,
    stagger,
    flee
}

public class Animal : MonoBehaviour, IDamageable {
    protected Rigidbody2D myRigidbody;
    protected Animator animator;
    protected float knockbackMultiplier = 1;

    [SerializeField] private float speed;
    [SerializeField] private float knockbackDuration = 0.25f;

    private AnimalState currentState = AnimalState.idle;

    protected void Start() {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
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

    // Animals stagger, but take no damage by default
    public void TakeDamage(Vector2 force, float damage) {
        if (GetState() == AnimalState.stagger) {
            return;
        }
        ChangeState(AnimalState.stagger);
        StartCoroutine(KnockbackCoroutine(force, knockbackDuration));
    }

    protected IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        myRigidbody.velocity = force * knockbackMultiplier;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(AnimalState.walk);
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
        animator.SetFloat("moveX", v.x);
        animator.SetFloat("moveY", v.y);
    }


    protected IEnumerator WalkCoroutine(Vector2 velocity, float duration) {
        ChangeState(AnimalState.walk);
        animator.SetBool("moving", true);

        SetWalkAnimation(velocity);
        myRigidbody.velocity = velocity;

        yield return new WaitForSeconds(duration);

        if (GetState() == AnimalState.stagger) {
            // Let the stagger animation handle movement instead.
            yield break;
        }

        animator.SetBool("moving", false);
        ChangeState(AnimalState.idle);
        myRigidbody.velocity = Vector3.zero;
    }

}



