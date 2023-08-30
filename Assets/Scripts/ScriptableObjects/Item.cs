using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Item : ScriptableObject {
    public string description;
    public Sprite sprite;

    public bool isKey = false;
    public bool isCoin = false;
    public bool isUseable = false;

    public UnityEvent useEvent;

    [SerializeField] private SignalSender pickupSignal = null;
    
    public void onPickup() {
        if (pickupSignal) {
            pickupSignal.Raise();
        }
    }

    public void onUse() {
        Debug.Log("Using " + this.name);
        //useEvent.Invoke();
    }
}
