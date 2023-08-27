using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour {
    [SerializeField] private GameObject inventoryMenu;
    private bool menuOpen = false;

    void Update() {
        if (Input.GetButtonDown(InputMap.BUTTON_INVENTORY)) {
            if (menuOpen) {
                CloseMenu();
            } else if (!isTimePaused()) {
                OpenMenu();
            }

            // If this menu is NOT open, but time is paused, that means that a different menu is open.
            // This menu should NOT respond while another menu is open.
        }
    }

    private void OpenMenu() {
        Time.timeScale = 0;
        menuOpen = true;
        inventoryMenu.SetActive(true);
    }

    public void CloseMenu() {
        Time.timeScale = 1;
        menuOpen = false;
        inventoryMenu.SetActive(false);
    }

    private bool isTimePaused() {
        return Time.timeScale == 0;
    }
}
