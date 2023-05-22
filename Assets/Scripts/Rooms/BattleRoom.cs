using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : Room {
    [SerializeField] private GameObject[] doors;

    private void Start() {
        OpenDoors();
    }

    new private void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);

        if (other.gameObject.CompareTag(Player.TAG) && !other.isTrigger) {
            CloseDoors();
        }
    }

    public void CheckEnemies() {
        foreach (Enemy e in enemies) {
            if(e.gameObject.activeSelf) {
                return;
            }
        }

        // We'll only get here if there are no enimies active in the hierarchy.
        OpenDoors();
    }


    private void OpenDoors() {
        foreach (GameObject door in doors) {
            door.SetActive(false);
            //door.Open();
        }
        Debug.Log("Opening Doors");
        
    }

    private void CloseDoors() {
        foreach (GameObject door in doors) {
            door.SetActive(true);

            //door.Close();
        }
        Debug.Log("Closing Doors");
    }

}
