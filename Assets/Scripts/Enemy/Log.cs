using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : Enemy {
    private Animator animator;

    public Transform home;
    public Transform target;

    public float wakeup_duration = 0.5f;
    public float chaseRadius;
    public float attackRadius;

    new void Start() {
        base.Start();

        currentState = EnemyState.idle;
        myRigidbody.bodyType = RigidbodyType2D.Static;

        target = GameObject.FindWithTag("Player").transform;

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    void FixedUpdate() {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius) {
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;

            if (currentState == EnemyState.idle) {
                StartCoroutine(WakeUpCoroutine());
            } else if (currentState == EnemyState.walk) {
                if (Vector3.Distance(target.position, transform.position) >= attackRadius) {
                   Move(transform.position, target.position);
                }
            }
        } else {
            if (currentState != EnemyState.idle) {
                GoToBackToSleep();
            }
        }
    }

    private IEnumerator WakeUpCoroutine() {
        animator.SetBool("wakeUp", true);
        yield return new WaitForSeconds(wakeup_duration);

        ChangeState(EnemyState.walk);
    }

    Vector3 Move(Vector3 myPosition, Vector3 targetPosition) {
        Vector3 move = Vector3.MoveTowards(myPosition, targetPosition, moveSpeed * Time.deltaTime);
        Vector3 delta = move - myPosition;

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

        myRigidbody.MovePosition(move);
        return move;
    }

    private void SetWalkAnimation(Vector2 v) {
        animator.SetFloat("moveX", v.x);
        animator.SetFloat("moveY", v.y);
    }

    private void GoToBackToSleep() {
        if (Vector3.Distance(home.position, transform.position) == 0) {
            animator.SetBool("wakeUp", false);
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", -1);

            ChangeState(EnemyState.idle);
            myRigidbody.bodyType = RigidbodyType2D.Static;
        } else {
            Vector3 move = Move(transform.position, home.position);
            myRigidbody.MovePosition(move);
        }
    }
}
