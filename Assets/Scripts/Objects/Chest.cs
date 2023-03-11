using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ChestState {
    closed,
    opening,
    opened
}

public class Chest : Interactable {
    public Animator animator;
    public Item item;
    public Inventory playerInventory;

    public SignalSender raiseItem;
    public SignalSender keySignal;

    private ChestState currentState = ChestState.closed;

    private void Start() {
        this.animator = GetComponent<Animator>();
        this.text.Add(item.description);
    }

    protected override bool CanInteract() {
        return ((currentState != ChestState.opened) && playerInRange);

    }

    protected override void Interact() {
        if (currentState == ChestState.closed) {
            currentState = ChestState.opening;

            interactableOutOfRange.Raise();
            animator.SetBool("opened", true);

            playerInventory.AddItem(item);
            raiseItem.Raise();

            if(item.isKey) {
                keySignal.Raise();
            }

        } else if (currentState == ChestState.opening) {
            // If we've scrolled through all of the text:
            //  - Return the player to normal
            //  - Make the chest non-interactable
            if (activeText == -1) {
                currentState = ChestState.opened;
                raiseItem.Raise();
            }
        }
    }
}
