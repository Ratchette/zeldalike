using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    [SerializeField][TextArea(3, 4)] public List<string> text;

    public int activeText = -1;

    public bool playerInRange = false;

    public SignalSender interactableInRange;
    public SignalSender interactableOutOfRange;

    
    void Update() {
        if (InteractButtonDown() && CanInteract()) {
            _Interact();
        }
    }

    private bool InteractButtonDown() {
        return Input.GetButtonDown("interact");
    }

    virtual protected bool CanInteract() {
        return playerInRange;
    }

    abstract protected void Interact();


    protected void _Interact() {
        if (text[0].Length > 0) {
            // Cycles through every text 
            if (activeText == -1) {
                dialogBox.SetActive(true);
                activeText++;
                dialogText.SetText(text[activeText]);

            } else if ((activeText + 1) >= text.Count) {
                // We've cycled through all of the text, so close the dialog.
                dialogBox.SetActive(false);
                activeText = -1;

            } else {
                // Display the next section of text.
                activeText++;
                dialogText.SetText(text[activeText]);
            }
        }

        Interact();
    }


    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag(Player.TAG) && !other.isTrigger) {
            playerInRange = true;

            if (CanInteract()) {
                interactableInRange.Raise();
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag(Player.TAG) && !other.isTrigger) {
            playerInRange = false;

            interactableOutOfRange.Raise();

            if (dialogBox.activeInHierarchy && activeText != -1) {
                dialogBox.SetActive(false);
                activeText = -1;
            }
        }
    }
}
