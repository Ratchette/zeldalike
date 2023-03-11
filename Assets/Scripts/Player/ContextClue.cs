using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour {
    public GameObject contextClue;
    public SpriteRenderer contextSprite;

    [SerializeField] private Sprite interactionSprite;
    [SerializeField] private Sprite doorSprite;

    public void InteractionEnable() {
        contextSprite.sprite = interactionSprite;
        contextClue.SetActive(true);
    }

    public void InteractionDisable() {
        contextClue.SetActive(false);
    }

    public void doorEnable() {
        contextSprite.sprite = doorSprite;
        contextClue.SetActive(true);
    }

    public void doorDisable() {
        contextClue.SetActive(false);
    }
}
