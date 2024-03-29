using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum EnemyState {
    Idle,
    Walk,
    Attack,
    Stagger
}

[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour, IDamageable {
    static protected string ANIMATOR_MOVING = "moving";
    static protected string ANIMATOR_MOVE_X = "moveX";
    static protected string ANIMATOR_MOVE_Y = "moveY";

    protected Rigidbody2D myRigidbody;
    protected Transform target;
    protected Animator animator;
    private LootSpawner lootSpawner;

    private Health health; 

    [SerializeField] private EnemyState currentState = EnemyState.Idle;

    [SerializeField] private GameObject deathEffect;
    [SerializeField] private SignalSender deathSignal;

    [Header("Walking", order = 0)]
    [SerializeField] private float speed = 1;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.25f;

    [Header("Attack")]
    [SerializeField] private int baseAttack;


    protected void Start() {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag(Player.TAG).transform;

        animator = gameObject.GetComponent<Animator>();
        animator.SetFloat(ANIMATOR_MOVE_X, 0);
        animator.SetFloat(ANIMATOR_MOVE_Y, -1);

        health = gameObject.GetComponent<Health>();
        lootSpawner = gameObject.GetComponent<LootSpawner>();
    }

    protected virtual void OnEnable() {
        if (health) {
            health.Reset();
        }

        currentState = EnemyState.Idle;
    }

    public EnemyState GetState() {
        return currentState;
    }

    protected EnemyState ChangeState(EnemyState newState) {
        //Debug.LogFormat("[{0}][ChangeState] prev={1}, new={2}", enemyName, currentState, newState);

        if (currentState != newState) {
            EnemyState previousState = currentState;
            currentState = newState;
            return previousState;
        } else {
            return currentState;
        }
    }

    // Default Moving implementation, can be overriden in subclass
    protected Vector2 CalculateNewPosition(Vector3 myPosition, Vector3 targetPosition) {
        return Vector3.MoveTowards(myPosition, targetPosition, speed * Time.fixedDeltaTime);
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
        animator.SetFloat(ANIMATOR_MOVE_X, v.x);
        animator.SetFloat(ANIMATOR_MOVE_Y, v.y);
    }

    virtual protected void TakeDamage_Begin() { }

    public void TakeDamage(Vector2 force, float damage_ammount) {
        TakeDamage_Begin();

        if (GetState() == EnemyState.Stagger) {
            return;
        }
        ChangeState(EnemyState.Stagger);

        if (health.Damage(damage_ammount)) {
            StartCoroutine(KnockbackCoroutine(force, knockbackDuration));
        } else {
            Die();
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 force, float duration) {
        myRigidbody.velocity = force;
        yield return new WaitForSeconds(duration);

        myRigidbody.velocity = Vector2.zero;
        ChangeState(EnemyState.Walk);
    }

    protected void Die() {
        if (deathEffect) {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        if (lootSpawner) {
            lootSpawner.Drop();
        }
        gameObject.SetActive(false);
        deathSignal.Raise();
    }
}
