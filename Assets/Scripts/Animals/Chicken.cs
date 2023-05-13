using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Chicken : Animal {
    // Continues header "Knockback" from Animal
    [SerializeField] private int knockbackMultiplier = 2;
    private bool playerTooClose = false;

    [Header("Walking")]
    [SerializeField] private float walkMaxDuration;

    [Header("Fleeing")]
    [SerializeField] private float fleeSpeed;
    [SerializeField] private float fleeRadius;
    [SerializeField] private float fleeDuration;

    new void Start() {
        base.Start();

        _knockbackMultiplier = knockbackMultiplier;
    }

    void FixedUpdate() {
        playerTooClose = Vector3.Distance(player.position, transform.position) <= fleeRadius;

        if (GetState() == AnimalState.Flee) {
            Flee(transform.position, player.position);
        }
        else if (playerTooClose) {
            if (GetState() != AnimalState.Stagger) {
                fleeCoroutine = StartCoroutine(FleeCoroutine(transform.position, player.position));
            }
        } else if(GetState() == AnimalState.Idle){ 
            MaybeWalk();
        }
    }

    private void MaybeWalk() {
        if (Random.value < 0.02) {
            walkCoroutine = StartCoroutine(WalkCoroutine(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.value * walkMaxDuration));
        }
    }


    private void Flee(Vector3 chicken, Vector3 player) {
        if (walkCoroutine != null) {
            StopCoroutine(walkCoroutine);
            walkCoroutine = null;
        }

        Vector2 velocity = ((player - chicken) * -1).normalized * fleeSpeed;
        SetWalkAnimation(velocity);
        myRigidbody.velocity = velocity;
    }


    protected private IEnumerator FleeCoroutine(Vector3 animal, Vector3 player) {
        Vector2 velocity = ((player - animal) * -1).normalized * fleeSpeed;

        ChangeState(AnimalState.Flee);
        animator.SetBool(ANIMATOR_MOVING, true);

        SetWalkAnimation(velocity);
        myRigidbody.velocity = velocity;

        while (playerTooClose) {
            yield return new WaitForSeconds(fleeDuration);
        }

        animator.SetBool(ANIMATOR_MOVING, false);
        ChangeState(AnimalState.Idle);
        myRigidbody.velocity = Vector3.zero;
        fleeCoroutine = null;
    }
}