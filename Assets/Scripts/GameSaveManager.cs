using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {
    public List<ScriptableObject> persistedData = new List<ScriptableObject>();

    private static GameSaveManager saveData;

    private void Awake() {
        if (saveData != null) {
            Destroy(this.gameObject);
            return;
        }

        saveData = this;
        DontDestroyOnLoad(this);
    }
}
