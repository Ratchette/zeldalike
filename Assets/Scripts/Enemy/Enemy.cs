using System.Collections;
using UnityEngine;

public enum EnemyState {
    idle,
    walk,
    attack,
    stagger
}
public class Enemy : MonoBehaviour, IDamageable {
    protected Rigidbody2D myRigidbody;
    protected Animator animator;

    [SerializeField] private string enemyName;
    [SerializeField] private FloatValue maxHealth;
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed;

    [SerializeField] private int baseAttack;
    [SerializeField] private float knockbackDuration = 0.25f;
    [SerializeField] private float invincibilityTime = 0.5f;

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
            gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        myRigidbody.velocity = force;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(EnemyState.walk);
    }

    // Default Moving implementation, can be overriden in subclass
    protected Vector3 Move(Vector3 myPosition, Vector3 targetPosition) {
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

    protected void SetWalkAnimation(Vector2 v) {
        animator.SetFloat("moveX", v.x);
        animator.SetFloat("moveY", v.y);
    }
}
