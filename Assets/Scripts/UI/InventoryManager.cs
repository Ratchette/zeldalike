using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryItemsUI;
    [SerializeField] private GameObject inventoryMenuItemPrefab;

    [SerializeField] private TextMeshProUGUI descriptionBox;
    [SerializeField] private Button useButton;
    private Color buttonDisabledColor = new Color(177 / 255f, 177 / 255f, 177 / 255f);

    [SerializeField] private Inventory inventory;
    private InventoryMenuItem selectedMenuItem = null;
    private Item selectedItem = null;

    private bool menuOpen = false;

    private void Start() {
        useButton.enabled = false;
        useButton.image.color = buttonDisabledColor;
        useButton.GetComponentInChildren<TextMeshProUGUI>().color = buttonDisabledColor;
    }

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

    private bool isTimePaused() {
        return Time.timeScale == 0;
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
            menuItem.GetComponent<InventoryMenuItem>().Init(this, i.Key, i.Value);
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

    public void SetChosenItem(InventoryMenuItem menuItem, Item item) {
        selectedMenuItem = menuItem;
        selectedItem = item;
        descriptionBox.text = item.description;

        if (item.isUseable) {
            useButton.enabled = true;
            useButton.image.color = Color.white;
            useButton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        } else {
            useButton.enabled = false;
            useButton.image.color = buttonDisabledColor;
            useButton.GetComponentInChildren<TextMeshProUGUI>().color = buttonDisabledColor;
        }
    }

    public void UseClicked() {
        selectedItem.onUse();
        selectedMenuItem.UseItem();
        
        int numItems = inventory.RemoveItem(selectedItem);
        if(numItems < 1) {
            Destroy(selectedMenuItem.gameObject);
        }
    }
}
