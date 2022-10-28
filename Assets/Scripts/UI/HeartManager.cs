using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour {
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite threeQuartersHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite quarterHeart;
    [SerializeField] private Sprite emptyHeart;

    [SerializeField] private FloatValue heartContainers;
    [SerializeField] private FloatValue playerHealth;

    private void Start() {
        InitHearts();
    }

    public void InitHearts() {
        for (int i=0; i<heartContainers.runtimeValue; i++) {
            hearts[i].gameObject.SetActive(true);
        }
        UpdateHearts();
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
}
