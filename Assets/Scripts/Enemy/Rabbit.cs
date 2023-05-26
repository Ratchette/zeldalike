using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rabbit : Enemy {
    static protected string ANIMATOR_ATTACK = "attack";

    [SerializeField] private float attackRadius = 5;
    [SerializeField] private float attackDelay = 1.0f;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float nextAttackTime = 0;

    void FixedUpdate() {
        if (Vector3.Distance(target.position, transform.position) <= attackRadius) {
            if (GetState() == EnemyState.Idle) {
                WakeUp();
            } else if (nextAttackTime < Time.time) {
                Attack();
            }
        } else {
            if (GetState() != EnemyState.Idle) {
                GoToBackToSleep();
            }
        }
    }

    private void WakeUp() {
        animator.SetBool(ANIMATOR_ATTACK, true);
        ChangeState(EnemyState.Walk);
    }

    private void GoToBackToSleep() {
        animator.SetBool(ANIMATOR_ATTACK, false);
        ChangeState(EnemyState.Idle);
    }

    private void Attack() {
        Vector2 direction = target.position - this.transform.position;

        // +90 degrees because the prefab starts with the point facing down -Y axis
        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;

        GameObject attack = Instantiate(projectile, this.transform.position, Quaternion.Euler(0, 0, rotation));
        attack.GetComponent<Projectile>().SetVelocity(direction.normalized);

        nextAttackTime = Time.time + attackDelay;
    }
}
