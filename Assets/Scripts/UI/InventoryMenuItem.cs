using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuItem : MonoBehaviour {
    public Item item;
    private int quantity;

    private Image displayImage;
    private TextMeshProUGUI displayNumber;

    private InventoryManager inventoryManager;

    public void Init(InventoryManager manager, Item i, int number) {
        inventoryManager = manager;
        item = i;
        quantity = number;

        displayImage = GetComponentInChildren<Image>();
        displayImage.sprite = item.sprite;

        if(i.isUseable) {
            displayNumber = GetComponentInChildren<TextMeshProUGUI>();
            displayNumber.text = "x " + quantity;
        }
    }

    public void AddItem(int num) {
        quantity += num;
        displayNumber.text = "x " + quantity;
    }

    public void OnClick() {
        inventoryManager.SetChosenItem(this, item);
    }

    public void UseItem() {
        quantity--;
        displayNumber.text = "x " + quantity;
    }
}
