using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Log : Enemy {

    [SerializeField] protected Transform home;
    [SerializeField] protected Transform target;

    [SerializeField] protected float wakeup_duration = 0.5f;
    [SerializeField] protected float chaseRadius = 4;
    [SerializeField] protected float attackRadius = 0.5f;

    private Coroutine wakeupCoroutine = null;

    new void Start() {
        base.Start();

        myRigidbody.bodyType = RigidbodyType2D.Static;
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius) {
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;

            if (GetState() == EnemyState.idle) {
                if(wakeupCoroutine == null) {
                    wakeupCoroutine = StartCoroutine(WakeUpCoroutine());
                }
            } else if (GetState() == EnemyState.walk) {
                if (Vector3.Distance(target.position, transform.position) >= attackRadius) {
                    Move(CalculateNewPosition(transform.position, target.position));
                }
            }
        } else {
            if (GetState() != EnemyState.idle) {
                GoToBackToSleep();
            }
        }
    }

    private IEnumerator WakeUpCoroutine() {
        animator.SetBool("wakeUp", true);
        yield return new WaitForSeconds(wakeup_duration);

        ChangeState(EnemyState.walk);
        wakeupCoroutine = null;
    }

    private void GoToBackToSleep() {
        if (Vector3.Distance(home.position, transform.position) == 0) {
            animator.SetBool("wakeUp", false);
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", -1);

            ChangeState(EnemyState.idle);
            myRigidbody.bodyType = RigidbodyType2D.Static;
        } else {
            Move(CalculateNewPosition(transform.position, home.position));
        }
    }
}
