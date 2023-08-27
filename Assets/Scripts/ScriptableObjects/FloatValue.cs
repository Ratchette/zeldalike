using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Value", menuName = "ScriptableObjects/Float")]

public class FloatValue : ScriptableObject, ISerializationCallbackReceiver {
    public float initialValue;
    public float runtimeValue;

    public void OnBeforeSerialize() {
        //throw new System.NotImplementedException();
    }

    public void OnAfterDeserialize() {
        runtimeValue = initialValue;
    }
}
