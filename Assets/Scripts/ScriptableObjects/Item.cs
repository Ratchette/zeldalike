using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject {
    public string description;
    public Sprite sprite;

    public bool isKey = false;
    public bool isCoin = false;
    public bool isUseable = false;

    [SerializeField] private SignalSender pickupSignal = null;
    
    public void onPickup() {
        if (pickupSignal) {
            pickupSignal.Raise();
        }
    }
}
