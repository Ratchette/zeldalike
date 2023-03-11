using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class KeyManager : MonoBehaviour {
    [SerializeField] private Sprite[] numbers;
    [SerializeField] private Image keysUI;

    [SerializeField] private FloatValue numKeys;

    private void Start() {
        keysUI.sprite = numbers[(int)numKeys.runtimeValue];
    }

    public void UpdateKeys() {
        keysUI.sprite = numbers[(int)numKeys.runtimeValue];
    }
}
