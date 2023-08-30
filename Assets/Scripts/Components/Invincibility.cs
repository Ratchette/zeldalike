using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Invincibility : MonoBehaviour {
    [SerializeField] private float flashDuration = 0.4f;
    [SerializeField] private int numFlashes = 3;

    private Color defaultColor;
    [SerializeField] private Color flashColor = new Color(0.6320754f, 0.2593895f, 0.2593895f, 0.8235294f);

    private SpriteRenderer sprite;

    void Start() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        defaultColor = sprite.color;
    }

    public IEnumerator InvincibilityCoroutine() {
        int flashes = 0;

        while (flashes < numFlashes) {
            sprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration / 2);

            sprite.color = defaultColor;
            yield return new WaitForSeconds(flashDuration / 2);

            ++flashes;
        }
    }
}
