using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Pot[] pots;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(Player.TAG)) {

            foreach(Enemy enemy in enemies) {
                enemy.gameObject.SetActive(true);
            }
            foreach(Pot pot in pots) {
                pot.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag(Player.TAG)) {

            foreach (Enemy enemy in enemies) {
                enemy.gameObject.SetActive(false);
            }
            foreach (Pot pot in pots) {
                pot.gameObject.SetActive(false);
            }
        }
    }
}