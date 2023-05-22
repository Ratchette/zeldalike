using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pot : MonoBehaviour, IDamageable {
    static protected string ANIMATOR_SMASH = "smash";

    private Animator animator;
    private LootSpawner itemSpawner;

    private void Start() {
        this.animator = GetComponent<Animator>();
        this.itemSpawner = GetComponent<LootSpawner>();
    }

    public void TakeDamage(Vector2 force, float damage) {
        animator.SetBool(ANIMATOR_SMASH, true);
        StartCoroutine(breakCoroutine());
    }

    IEnumerator breakCoroutine() {
        itemSpawner.Drop();
        yield return new WaitForSeconds(0.32f);
        this.gameObject.SetActive(false);
    }
}
