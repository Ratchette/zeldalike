using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {
    [SerializeField] private float healthIncrease;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(Player.TAG) && !other.isTrigger) {

            other.GetComponent<Health>().Heal(healthIncrease);
            Destroy(this.gameObject);
        }
    }
}
