using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField][TextArea(3, 4)] private List<string> text;

    private bool playerInRange = false;
    private int activeText = -1;

    public SignalSender contextClueOn;
    public SignalSender contextClueOff;

    public bool CanInteract() {
        return (Input.GetButtonDown("interact") && playerInRange);
    }

    // Cycles through every text field 
    public void Interact() {
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
            contextClueOn.Raise();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;
            contextClueOff.Raise();

            // If the player walks away while the text is shown
            if (dialogBox.activeInHierarchy && activeText != -1) {
                dialogBox.SetActive(false);
                activeText = -1;
            }
        }
    }
}
