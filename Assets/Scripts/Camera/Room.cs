using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField] private GameObject virtualCamera;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !other.isTrigger) {
            virtualCamera.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") && !other.isTrigger) {
            virtualCamera.SetActive(false);
        }
    }
}
