using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector Value", menuName = "ScriptableObjects/Vector")]
public class Vector2Value : ScriptableObject, ISerializationCallbackReceiver {
    public Vector2 initialValue;
    public Vector2 runtimeValue;

    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public void OnAfterDeserialize() {
        runtimeValue = initialValue;
    }
}
