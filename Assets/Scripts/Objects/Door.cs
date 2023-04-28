using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour {
    public BooleanValue isOpen;

    public bool playerInRange = false;
    public Inventory playerInventory;

    public SignalSender keySignal;
    public SignalSender doorInRange;
    public SignalSender doorOutOfRange;


    private void Start() {
        if (isOpen.runtimeValue) {
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;

            if (CanInteract()) {
                doorInRange.Raise();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
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
        return Input.GetButtonDown("interact");
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
