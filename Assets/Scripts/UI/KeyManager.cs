using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyManager : MonoBehaviour {
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private Image keysUI;

    [Header("Player Data")]
    [SerializeField] private Inventory playerInventory;

    private void Start() {
        keysUI.sprite = numbers[playerInventory.getNumKeys()];
    }

    public void UpdateKeys() {
        keysUI.sprite = numbers[playerInventory.getNumKeys()];
    }
}
