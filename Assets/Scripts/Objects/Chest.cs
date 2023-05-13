using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ChestState {
    Closed,
    Opening,
    Opened
}

public class Chest : Interactable {
    static private string ANIMATOR_OPENED = "opened";

    private Animator animator;
    private ChestState currentState = ChestState.Closed;

    [SerializeField] private BooleanValue isOpen;
    [SerializeField] private Item item;
    [SerializeField] private SignalSender raiseItem;
    [SerializeField] private SignalSender keySignal;

    [Header("Player Data")]
    [SerializeField] private Inventory playerInventory;

    private void Start() {
        this.animator = GetComponent<Animator>();
        this.text.Add(item.description);

        if(isOpen.runtimeValue == true) {
            currentState = ChestState.Opened;
            animator.SetBool(ANIMATOR_OPENED, true);
        }
    }

    protected override bool CanInteract() {
        return ((currentState != ChestState.Opened) && playerInRange);

    }

    protected override void Interact() {
        if (currentState == ChestState.Closed) {
            currentState = ChestState.Opening;

            interactableOutOfRange.Raise();
            animator.SetBool(ANIMATOR_OPENED, true);

            playerInventory.AddItem(item);
            raiseItem.Raise();

            if(item.isKey) {
                keySignal.Raise();
            }

        } else if (currentState == ChestState.Opening) {
            // If we've scrolled through all of the text:
            //  - Return the player to normal
            //  - Make the chest non-interactable
            if (activeText == -1) {
                currentState = ChestState.Opened;
                isOpen.runtimeValue = true;
                raiseItem.Raise();
            }
        }
    }
}
