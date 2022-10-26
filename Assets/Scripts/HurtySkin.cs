using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtySkin : MonoBehaviour {
    public float knockbackThrust;
    public float knockbackTime;
    public FloatValue damage;

    private void OnTriggerEnter2D(Collider2D other) {
        // NOTE: This allows enemies to hurt other enemies
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("enemy")) {
            Rigidbody2D otherRigidbody = other.GetComponent<Rigidbody2D>();

            if (otherRigidbody != null) {
                Vector2 forceDirection = otherRigidbody.transform.position - transform.position;
                Vector2 force = forceDirection.normalized * knockbackThrust;

                if (other.gameObject.CompareTag("Player")) {
                    other.gameObject.GetComponent<Player>().Hit(force, knockbackTime, damage.runtimeValue);
                }
                // Enemies bumping into one another doesn't cause damage.
                //} else if (other.gameObject.CompareTag("enemy")) {
                //    other.gameObject.GetComponent<Enemy>().Hit(force, knockbackTime, damage.runtimeValue);
                //}
            }
        }
    }
}
