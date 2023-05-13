using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pot : MonoBehaviour, IDamageable {
    static protected string ANIMATOR_SMASH = "smash";

    private Animator animator;

    private void Start() {
        this.animator = GetComponent<Animator>();
    }

    public void TakeDamage(Vector2 force, float damage) {
        animator.SetBool(ANIMATOR_SMASH, true);
        StartCoroutine(breakCoroutine());
    }

    IEnumerator breakCoroutine() {
        yield return new WaitForSeconds(0.32f);
        this.gameObject.SetActive(false);
    }
}
