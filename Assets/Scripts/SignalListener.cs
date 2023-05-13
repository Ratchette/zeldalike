using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour {
    [SerializeField] private SignalSender signal;
    [SerializeField] private UnityEvent signalEvent;

    private void OnEnable() {
        signal.RegisterListener(this);
    }
    private void OnDisable() {
        signal.DeregisterListener(this);
    }

    public void OnSignalRaised() {
        signalEvent.Invoke();
    }
}
