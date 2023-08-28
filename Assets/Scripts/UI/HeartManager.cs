using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour {
    [Header("Images")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite threeQuartersHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite quarterHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Player Data")]
    [SerializeField] private FloatValue playerHealth;
    [SerializeField] private FloatValue heartContainers;

    void Start() {
        InitHearts();
    }

    public void InitHearts() {
        UpdateHeartContainers();
        UpdateHearts();
    }

    private void UpdateHeartContainers() {
        for (int i = 0; i < heartContainers.runtimeValue; i++) {
            hearts[i].gameObject.SetActive(true);
        }
    }

    public void UpdateHearts() {
        float fullHearts = playerHealth.runtimeValue / 4;

        for (int i = 0; i < heartContainers.runtimeValue; i++) {
            if (fullHearts - i >= 1) {
                hearts[i].sprite = fullHeart;

            } else if (fullHearts - i > 0) {
                if (fullHearts % 1 == 0.75) {
                    hearts[i].sprite = threeQuartersHeart;
                } else if (fullHearts % 1 == 0.5) {
                    hearts[i].sprite = halfHeart;
                } else if (fullHearts % 1 == 0.25) {
                    hearts[i].sprite = quarterHeart;
                } 

            } else {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    public void PickupHeartContainer() {
        heartContainers.runtimeValue++;
        playerHealth.runtimeValue = heartContainers.runtimeValue * 4;

        UpdateHeartContainers();
        UpdateHearts();
    }
}
