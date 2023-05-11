using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver {
    private static int MAX_COINS = 999999;

    private Item itemToDisplay = null;
    public List<Item> items = new List<Item>();

    [SerializeField] private int numKeys;
    [SerializeField] private int numCoins;

    public bool AddItem(Item item) {
        if (item.isKey) {
            numKeys++;
        } else {
            items.Add(item);
        }

        itemToDisplay = item;

        return true;
    }

    public bool AddCoins(int coinsToAdd) {
        numCoins = Mathf.Min(numCoins + coinsToAdd, MAX_COINS);
        return true;
    }

    public Sprite GetItemToDisplay() {
        Sprite sprite = itemToDisplay.sprite;
        itemToDisplay = null;
        return sprite;
    }

    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public void OnAfterDeserialize() {
        items.Clear();
        numKeys = 0;
        numCoins = 0;
    }

    public int getNumKeys() {
        return numKeys;
    }

    public void UseKey() {
        numKeys--;
    }

    public int getNumCoins() {
        return numCoins;
    }
}
