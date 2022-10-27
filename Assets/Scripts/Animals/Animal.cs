using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AnimalState {
    idle,
    walk,
    stagger,
    flee
}

public class Animal : MonoBehaviour {
    protected Rigidbody2D myRigidbody;

    public AnimalState currentState;
    public float speed;

    public float knockbackForceMultiplier = 1;

    protected void Start() {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
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

    public void Hit(Vector2 force, float duration, float damage) {
        if (currentState == AnimalState.stagger) {
            return;
        }
        ChangeState(AnimalState.stagger);
        StartCoroutine(KnockbackCoroutine(force, duration));
    }

    protected IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        myRigidbody.velocity = force * knockbackForceMultiplier;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(AnimalState.walk);
    }
}



