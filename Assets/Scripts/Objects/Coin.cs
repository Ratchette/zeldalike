using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    [SerializeField] private SignalSender coinSignal;
    [SerializeField] private int value;

    [Header("Player Data")]
    [SerializeField] private Inventory playerInventory;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(Player.TAG) && !other.isTrigger) {
            playerInventory.AddCoins(value);
            coinSignal.Raise();

            Destroy(this.gameObject);
        }
    }
}
