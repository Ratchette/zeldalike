using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Log : Enemy {
    static protected string ANIMATOR_WAKEUP = "wakeUp";

    [SerializeField] protected float attackRadius = 0.5f;

    [Header("Sleeping")]
    [SerializeField] private Transform home;
    [SerializeField] private float wakeup_duration = 0.5f;
    [SerializeField] protected float chaseRadius = 4;

    private Coroutine wakeupCoroutine = null;


    new void Start() {
        base.Start();

        myRigidbody.bodyType = RigidbodyType2D.Static;
    }

    new protected void OnEnable() {
        base.OnEnable();
        this.transform.position = home.position;
    }

    void FixedUpdate() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius) {
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;

            if (GetState() == EnemyState.Idle) {
                if(wakeupCoroutine == null) {
                    wakeupCoroutine = StartCoroutine(WakeUpCoroutine());
                }
            } else if (GetState() == EnemyState.Walk) {
                if (Vector3.Distance(target.position, transform.position) >= attackRadius) {
                    Move(CalculateNewPosition(transform.position, target.position));
                }
            }
        } else {
            if (GetState() != EnemyState.Idle) {
                GoToBackToSleep();
            }
        }
    }

    private IEnumerator WakeUpCoroutine() {
        animator.SetBool(ANIMATOR_WAKEUP, true);
        yield return new WaitForSeconds(wakeup_duration);

        ChangeState(EnemyState.Walk);
        wakeupCoroutine = null;
    }

    private void GoToBackToSleep() {
        if (Vector3.Distance(home.position, transform.position) == 0) {
            animator.SetBool(ANIMATOR_WAKEUP, false);
            animator.SetFloat(ANIMATOR_MOVE_X, 0);
            animator.SetFloat(ANIMATOR_MOVE_Y, -1);

            ChangeState(EnemyState.Idle);
            myRigidbody.bodyType = RigidbodyType2D.Static;
        } else {
            Move(CalculateNewPosition(transform.position, home.position));
        }
    }

    override protected void TakeDamage_Begin() {
        if(wakeupCoroutine != null) {
            StopCoroutine(wakeupCoroutine);
            wakeupCoroutine = null;
        }
    }
}
