using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rabbit : Enemy {
    static protected string ANIMATOR_ATTACK = "attack";

    [SerializeField] private float attackRadius = 5;
    [SerializeField] private float attackSpeed = 1.0f;
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
        GameObject attack = Instantiate(projectile, this.transform);

        Vector2 direction = target.position - this.transform.position;
        attack.GetComponent<Projectile>().SetVelocity(direction.normalized);

        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attack.transform.rotation = Quaternion.Euler(0, 0, rotationZ + 90);
        
        nextAttackTime = Time.time + attackSpeed;
    }
}
