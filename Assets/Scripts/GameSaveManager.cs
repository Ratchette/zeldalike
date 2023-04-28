using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveManager : MonoBehaviour {

    public static GameSaveManager saveData;
    public List<ScriptableObject> persistedData = new List<ScriptableObject>();

    private void Awake() {
        if (saveData != null) {
            Destroy(this.gameObject);
            return;
        }

        saveData = this;
        DontDestroyOnLoad(this);
    }
}
