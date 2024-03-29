using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver {
    static private int MAX_COINS = 999999;

    private Item itemToDisplay = null;

    [SerializeField] public Dictionary<Item, int> items;
    [SerializeField] private int numKeys;
    [SerializeField] private int numCoins;

    public void OnAfterDeserialize() {
        items = new Dictionary<Item, int>();
        numKeys = 0;
        numCoins = 0;
    }

    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public bool AddItem(Item item) {
        if (item.isKey) {
            numKeys++;

        } else if (item.name == "Heart Container") {
            // Heart containers are not handled by the inventory

        } else if (items.ContainsKey(item)) {
            items[item]++;

        } else {
            items.Add(item, 1);
        }

        itemToDisplay = item;
        return true;
    }

    public int RemoveItem(Item item) {
        if (!items.ContainsKey(item)) {
            return -1;

        } else {
            if (items[item] > 1) {
                return --(items[item]);
                               
            } else {
                items.Remove(item);
                return 0;
            }
        }
    }

    public Sprite GetItemToDisplay() {
        Sprite sprite = itemToDisplay.sprite;
        itemToDisplay = null;
        return sprite;
    }

    public int getNumKeys() {
        return numKeys;
    }

    public void UseKey() {
        numKeys--;
    }

    public bool AddCoins(int coinsToAdd) {
        numCoins = Mathf.Min(numCoins + coinsToAdd, MAX_COINS);
        return true;
    }

    public int getNumCoins() {
        return numCoins;
    }
}
