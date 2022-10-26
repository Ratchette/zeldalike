using System.Collections;
using UnityEngine;

public enum EnemyState {
    idle,
    walk,
    attack,
    stagger
}
public class Enemy : MonoBehaviour {
    protected Rigidbody2D myRigidbody;

    public EnemyState currentState;

    public string enemyName;
    public FloatValue maxHealth;
    public float health;
    public int baseAttack;

    public float moveSpeed;

    protected void Start() {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth.runtimeValue;
    }

    protected EnemyState ChangeState(EnemyState newState) {
        if (currentState != newState) {
            EnemyState previousState = currentState;
            currentState = newState;
            return previousState;
        } else {
            return currentState;
        }
    }

    public void Hit(Vector2 force, float duration, float damage) {
        if (currentState == EnemyState.stagger) {
            return;
        }
        ChangeState(EnemyState.stagger);

        TakeDamage(damage);
        if (health > 0) {
            StartCoroutine(KnockbackCoroutine(force, duration));
        } else {
            gameObject.SetActive(false);
        }
    }

    private void TakeDamage(float damage) {
        Debug.LogFormat("[{0}][Hit] health={1}, damage={2}, newHealth={3}", enemyName, health, damage, (health - damage));
        health = health - damage;
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        Debug.Log("Knockback started");
        myRigidbody.velocity = force;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(EnemyState.walk);
    }
}
