using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    [SerializeField] private SignalSender healthSignal;
    [SerializeField] float healthIncrease;

    [Header("Player Data")]
    [SerializeField] private FloatValue heartContainers;
    [SerializeField] private FloatValue playerHealth;
    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(Player.TAG) && !other.isTrigger) {
            float maxHealth = heartContainers.runtimeValue * 4;
            healthIncrease = Mathf.Min(healthIncrease, maxHealth - playerHealth.runtimeValue);

            playerHealth.runtimeValue += healthIncrease;
            healthSignal.Raise();

            Destroy(this.gameObject);
        }
    }
}
