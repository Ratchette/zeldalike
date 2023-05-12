using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemyState {
    idle,
    walk,
    attack,
    stagger
}
public abstract class Enemy : MonoBehaviour, IDamageable {
    protected Rigidbody2D myRigidbody;
    protected Animator animator;

    [SerializeField] private string enemyName;
    [SerializeField] private FloatValue maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float speed = 1;

    [SerializeField] private int baseAttack;
    [SerializeField] private float knockbackDuration = 0.25f;

    [SerializeField] private GameObject deathEffect;

    private EnemyState currentState = EnemyState.idle;

    protected void Start() {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth.runtimeValue;

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    public EnemyState GetState() {
        return currentState;
    }

    protected EnemyState ChangeState(EnemyState newState) {
        Debug.LogFormat("[{0}][ChangeState] prev={1}, new={2}", enemyName, currentState, newState);

        if (currentState != newState) {
            EnemyState previousState = currentState;
            currentState = newState;
            return previousState;
        } else {
            return currentState;
        }
    }

    public void TakeDamage(Vector2 force, float damage) {
        if (GetState() == EnemyState.stagger) {
            return;
        }
        ChangeState(EnemyState.stagger);

        // Loose health
        Debug.LogFormat("[{0}][Hit] health={1}, damage={2}, newHealth={3}", enemyName, health, damage, (health - damage));
        health = health - damage;

        if (health > 0) {
            StartCoroutine(KnockbackCoroutine(force, knockbackDuration));
        } else {
            Die();
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        myRigidbody.velocity = force;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(EnemyState.walk);
    }

    // Default Moving implementation, can be overriden in subclass

    protected Vector2 CalculateNewPosition(Vector3 myPosition, Vector3 targetPosition) {
        return Vector3.MoveTowards(myPosition, targetPosition, speed * Time.deltaTime);
    }

    protected void Move(Vector3 newPosition) {
        Vector2 delta = newPosition - transform.position;

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

        myRigidbody.MovePosition(newPosition);
    }

    protected void SetWalkAnimation(Vector2 v) {
        animator.SetFloat("moveX", v.x);
        animator.SetFloat("moveY", v.y);
    }

    protected void Die() {
        if (deathEffect) {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        gameObject.SetActive(false);
    }
}
