using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour {
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite threeQuartersHeart;
    public Sprite halfHeart;
    public Sprite quarterHeart;
    public Sprite emptyHeart;

    public FloatValue heartContainers;
    public FloatValue playerHealth;

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
