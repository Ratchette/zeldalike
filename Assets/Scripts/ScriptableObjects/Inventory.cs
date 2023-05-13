using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver {
    static private int MAX_COINS = 999999;

    private Item itemToDisplay = null;

    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private int numKeys;
    [SerializeField] private int numCoins;

    public void OnAfterDeserialize() {
        items.Clear();
        numKeys = 0;
        numCoins = 0;
    }

    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public bool AddItem(Item item) {
        if (item.isKey) {
            numKeys++;
        } else {
            items.Add(item);
        }

        itemToDisplay = item;

        return true;
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
