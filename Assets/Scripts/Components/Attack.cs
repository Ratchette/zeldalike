using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    [SerializeField] private FloatValue damage;

    [Header("Knockback")]
    [SerializeField] private float knockbackThrust;
    [SerializeField] private float knockbackTime;

    private void OnTriggerEnter2D(Collider2D other) {
        IDamageable objectHit = (IDamageable)other.gameObject.GetComponent(typeof(IDamageable));

        if (objectHit != null) {

            Rigidbody2D otherRigidbody = other.GetComponent<Rigidbody2D>();
            // All objects that can be damaged must contain a rigidbody
            Debug.Assert(otherRigidbody != null);

            // Calculate where the hit came from
            Vector2 forceDirection = otherRigidbody.transform.position - transform.position;
            Vector2 force = forceDirection.normalized * knockbackThrust;

            objectHit.TakeDamage(force, damage.runtimeValue); 
        }
    }
}
