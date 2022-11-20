using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject, ISerializationCallbackReceiver {
    private Item itemToDisplay = null;
    public List<Item> items = new List<Item>();
    public int numKeys;

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

    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public void OnAfterDeserialize() {
        items.Clear();
        numKeys = 0;
    }
}