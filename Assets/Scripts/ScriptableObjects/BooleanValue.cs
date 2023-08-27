using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Boolean Value", menuName = "ScriptableObjects/Boolean")]
public class BooleanValue : ScriptableObject, ISerializationCallbackReceiver {
    public bool initialValue;
    public bool runtimeValue;
    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public void OnAfterDeserialize() {
        runtimeValue = initialValue;
    }
}