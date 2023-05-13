using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour {
    private bool playerInRange = false;

    [SerializeField] private BooleanValue isOpen;
    [SerializeField] private SignalSender keySignal;
    [SerializeField] private SignalSender doorInRange;
    [SerializeField] private SignalSender doorOutOfRange;

    [Header("Player Data")]
    [SerializeField] private Inventory playerInventory;


    private void Start() {
        if (isOpen.runtimeValue) {
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Player.TAG)) {
            playerInRange = true;

            if (CanInteract()) {
                doorInRange.Raise();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag(Player.TAG)) {
            playerInRange = false;

            doorOutOfRange.Raise();
        }
    }

    void Update() {
        if (!this.isOpen.runtimeValue && InteractButtonDown() && CanInteract()) {
            Interact();
        }
    }

    private bool InteractButtonDown() {
        return Input.GetButtonDown(InputMap.BUTTON_INTERACT);
    }

    virtual protected bool CanInteract() {
        return playerInRange && playerInventory.getNumKeys() > 0;
    }


    protected void Interact() {
        if (!this.isOpen.runtimeValue) {
            isOpen.runtimeValue = true;
            playerInventory.UseKey();
            keySignal.Raise();

            doorOutOfRange.Raise();

            this.gameObject.SetActive(false);
        }
    }
}
