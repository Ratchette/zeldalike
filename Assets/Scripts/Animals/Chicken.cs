using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Chicken : Animal {
    [SerializeField] private Transform player;
    [SerializeField] private float fleeSpeed;
    [SerializeField] private float fleeRadius;
    [SerializeField] private float fleeDuration;

    [SerializeField] private float walkMaxDuration;

    new void Start() {
        base.Start();

        player = GameObject.FindWithTag("Player").transform;
        knockbackMultiplier = 2;
    }

    void FixedUpdate() {
        if (Vector3.Distance(player.position, transform.position) <= fleeRadius) {
            if (GetState() != AnimalState.stagger) {
                StartCoroutine(FleeCoroutine(transform.position, player.position));
            }
        } else if (GetState() == AnimalState.flee) {
            Flee(transform.position, player.position);

        } else { 
            WalkRandomly();
        }
    }

    private IEnumerator FleeCoroutine(Vector3 chicken, Vector3 player) {
        Vector2 velocity = ((player - chicken) * -1).normalized * fleeSpeed;

        ChangeState(AnimalState.flee);
        animator.SetBool("moving", true);

        SetWalkAnimationDirection(velocity);
        myRigidbody.velocity = velocity;

        yield return new WaitForSeconds(fleeDuration);

        if (GetState() == AnimalState.stagger) {
            // Let the stagger animation handle movement instead.
            yield break;
        }

        animator.SetBool("moving", false);
        ChangeState(AnimalState.idle);
        myRigidbody.velocity = Vector3.zero;
    }

    private void Flee(Vector3 chicken, Vector3 player) {
        Vector2 velocity = ((player - chicken) * -1).normalized * fleeSpeed;
        SetWalkAnimationDirection(velocity);
        myRigidbody.velocity = velocity;
    }

    private void SetWalkAnimationDirection(Vector2 delta) {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
            if (delta.x <= 0) {
                SetWalkAnimation(Vector2.left);
            } else {
                SetWalkAnimation(Vector2.right);
            }
        } else {
            if (delta.y <= 0) {
                SetWalkAnimation(Vector2.down);
            } else {
                SetWalkAnimation(Vector2.up);
            }
        }
    }

    private void SetWalkAnimation(Vector2 v) {
        animator.SetFloat("moveX", v.x);
        animator.SetFloat("moveY", v.y);
    }

    private void WalkRandomly() {
        if (GetState() == AnimalState.idle) {
            if (Random.value < 0.02) {
                StartCoroutine(WalkCoroutine(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.value * walkMaxDuration));
            }
        }
    }

    private IEnumerator WalkCoroutine(Vector2 velocity, float duration) {
        ChangeState(AnimalState.walk);
        animator.SetBool("moving", true);
        SetWalkAnimationDirection(velocity);
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
