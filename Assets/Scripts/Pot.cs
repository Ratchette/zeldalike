using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pot : MonoBehaviour {

    private Animator animator;

    private void Start() {
        this.animator = GetComponent<Animator>();
    }

    public void Smash() {
        animator.SetBool("smash", true);
        StartCoroutine(breakCoroutine());
    }

    IEnumerator breakCoroutine() {
        yield return new WaitForSeconds(0.32f);
        this.gameObject.SetActive(false);
    }
}
