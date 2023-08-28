using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryItemsUI;
    [SerializeField] private GameObject inventoryMenuItemPrefab;

    [SerializeField] private Inventory inventory;
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

        createItems();

        inventoryUI.SetActive(true);
    }

    private void createItems() {
        foreach (KeyValuePair<Item, int> i in inventory.items) {
            GameObject menuItem = Instantiate(inventoryMenuItemPrefab);
            menuItem.GetComponent<InventoryMenuItem>().Init(i.Key, i.Value);
            menuItem.transform.SetParent(inventoryItemsUI.transform);
        }
    }

    public void CloseMenu() {
        Time.timeScale = 1;
        menuOpen = false;

        DestroyItems();

        inventoryUI.SetActive(false);
    }

    private void DestroyItems() {
        foreach(Transform child in inventoryItemsUI.transform) { 
            Destroy(child.gameObject);
        }
    }

    private bool isTimePaused() {
        return Time.timeScale == 0;
    }
}
