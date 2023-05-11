using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounterManager : MonoBehaviour {
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private TextMeshProUGUI coinCounter;
    void Start() {
        coinCounter.text = FormatForDisplay(playerInventory.getNumCoins());
    }

    // Update is called once per frame
    public void UpdateCoins() {
        coinCounter.text = FormatForDisplay(playerInventory.getNumCoins());
    }

    private string FormatForDisplay(int numCoins) {
        string padding = new string('0', 6 - numCoins.ToString().Length);
        return (padding + numCoins.ToString());
    }
}