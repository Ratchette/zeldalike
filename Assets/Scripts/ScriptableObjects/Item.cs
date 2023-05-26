using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject {
    public Sprite sprite;
    public string description;
    public bool isKey;
    public bool isCoin;

    public SignalSender pickupSignal = null;
    
    public void onPickup() {
        if (pickupSignal) {
            pickupSignal.Raise();
        }
    }
}
