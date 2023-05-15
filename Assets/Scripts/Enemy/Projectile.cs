using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [SerializeField] private float speed = 3;
    [SerializeField] private Vector2 velocity;

    private Rigidbody2D body;
    private float decayTime;

    void Start() {
        body = this.gameObject.GetComponent<Rigidbody2D>();
        decayTime = Time.time + 2.0f;

        Transform target = GameObject.FindWithTag(Player.TAG).transform;
        Transform startPoint = this.gameObject.GetComponent<Transform>();

        velocity = (target.position - startPoint.position).normalized * speed;
    }

    private void FixedUpdate() {
        body.velocity = velocity;

        if (decayTime < Time.time) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(Player.TAG)) {
            Destroy(this.gameObject);
        }
    }

}
