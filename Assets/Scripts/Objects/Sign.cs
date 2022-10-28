using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class Sign : MonoBehaviour {
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] [TextArea(3, 4)] private string text;

    private bool playerInRange;

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("interact") && playerInRange) {
            if (dialogBox.activeInHierarchy) {
                dialogBox.SetActive(false);
            } else {
                dialogText.SetText(text);
                dialogBox.SetActive(true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInRange = false;

            if(dialogBox.activeInHierarchy && dialogText.text.Equals(text)) {
                dialogBox.SetActive(false);
            }
        }
    }
}
