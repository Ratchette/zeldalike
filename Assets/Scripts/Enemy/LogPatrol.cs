using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPatrol : Log {

    [SerializeField] private Transform[] path;
    [SerializeField] private int currentGoal;


    new void Start() {
        base.Start();

        // Start the log off in the walking state
        animator.SetBool("wakeUp", true);
        ChangeState(EnemyState.walk);
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;

        // The log will start on goal 0
        currentGoal = 1;
    }

    void FixedUpdate() {
        // Stagger behaviour is handled by a coroutine in Enemy.cs
        if (GetState() == EnemyState.stagger) {
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
