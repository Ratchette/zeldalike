using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable {

    [SerializeField] private float speed = 3;

    private float decayTime;

    void Start() {
        decayTime = Time.time + 2.0f;
    }

    public void SetVelocity(Vector2 direction) {
        Vector2 velocity = direction * speed;
        this.gameObject.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    private void FixedUpdate() {
        if (decayTime < Time.time) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(Player.TAG)) {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(Vector2 force, float damage) {
        Destroy(this.gameObject);
    }
}
