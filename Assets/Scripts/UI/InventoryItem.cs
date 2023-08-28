using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject {
    public string itemName;
    public string description;

    public Sprite image;

    public bool isUseable;
    public int quantity;
    // One more var for if it is useable?
}
