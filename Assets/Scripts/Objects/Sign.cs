using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Sign : Interactable {

    // Update is called once per frame
    void Update() {
        if (CanInteract()) {
            Interact();
        }
    }
}
