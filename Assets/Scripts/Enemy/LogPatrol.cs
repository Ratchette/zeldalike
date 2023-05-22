using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class LogPatrol : Log {

    [Header("Walking route")]
    [SerializeField] private Transform[] path;
    [SerializeField] private int currentGoal;


    new void Start() {
        base.Start();

        animator.SetBool(ANIMATOR_WAKEUP, true);
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        currentGoal = 0;
    }

    new private void OnEnable() {
        base.OnEnable();

        if (animator) {
            animator.SetBool(ANIMATOR_WAKEUP, true);
        }
        currentGoal = 0;
    }

    void FixedUpdate() {
        // Stagger behaviour is handled by a coroutine in Enemy.cs
        if (GetState() == EnemyState.Stagger) {
            return;
        }

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius) {
            if (Vector3.Distance(target.position, transform.position) >= attackRadius) {
                Move(CalculateNewPosition(transform.position, target.position));
            }
        } else {
            Patrol();
        }
    }

    private void Patrol() {
        if(transform.position == path[currentGoal].position) {
            currentGoal = (currentGoal + 1) % path.Length;
        }

        Move(CalculateNewPosition(transform.position, path[currentGoal].position));
    }
}
