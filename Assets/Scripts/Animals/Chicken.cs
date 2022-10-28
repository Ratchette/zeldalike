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

        } else if(GetState() == AnimalState.idle){ 
            MaybeWalk();
        }
    }

    private void MaybeWalk() {
        if (Random.value < 0.02) {
         StartCoroutine(WalkCoroutine(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)), Random.value * walkMaxDuration));
        }
    }


    private void Flee(Vector3 chicken, Vector3 player) {
        Vector2 velocity = ((player - chicken) * -1).normalized * fleeSpeed;
        SetWalkAnimation(velocity);
        myRigidbody.velocity = velocity;
    }


    protected private IEnumerator FleeCoroutine(Vector3 animal, Vector3 player) {
        Vector2 velocity = ((player - animal) * -1).normalized * fleeSpeed;

        ChangeState(AnimalState.flee);
        animator.SetBool("moving", true);

        SetWalkAnimation(velocity);
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
}