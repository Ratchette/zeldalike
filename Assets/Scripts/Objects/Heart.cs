using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    [SerializeField] private SignalSender healthSignal;

    [SerializeField] private FloatValue heartContainers;
    [SerializeField] private FloatValue playerHealth;

    [SerializeField] float healthIncrease;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !other.isTrigger) {
            float maxHealth = heartContainers.runtimeValue * 4;
            healthIncrease = Mathf.Min(healthIncrease, maxHealth - playerHealth.runtimeValue);

            playerHealth.runtimeValue += healthIncrease;
            healthSignal.Raise();

            Destroy(this.gameObject);
        }
    }
}
