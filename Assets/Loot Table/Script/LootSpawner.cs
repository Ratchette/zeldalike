using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyScript;

public class LootSpawner : MonoBehaviour {
    public LootTable Loot;
    public int RandomDropCount = 1;
    public float DropRange = .5f;

    public void Drop() {
        Loot.SpawnDrop(this.transform, RandomDropCount, DropRange);
    }
}
