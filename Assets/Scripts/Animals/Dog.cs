using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : WalkingNPC {

    [SerializeField] private Item item;
    [SerializeField] private BooleanValue givenAwayItem;
    [SerializeField] private SignalSender raiseItem;
    [SerializeField] private float duration = 2.0f;

    [Header("Player Data")]
    [SerializeField] private Inventory playerInventory;

    
    protected override bool CanInteract() {
        if (givenAwayItem.runtimeValue == false) {
            return base.CanInteract();
        }

        return false;
    }

    protected override void Interact() {
        if(activeText == -1) {
            givenAwayItem.runtimeValue = true;
            StartCoroutine(DisplayItem(duration));
        }
    }

    private IEnumerator DisplayItem(float duration) {
        interactableOutOfRange.Raise();
        playerInventory.AddItem(item);
        raiseItem.Raise();
        yield return new WaitForSeconds(duration);

        item.onPickup();
        raiseItem.Raise();
        yield return null;
    }
}
