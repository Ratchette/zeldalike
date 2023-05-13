using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    [Header("New Scene")]
    [SerializeField] private string SceneToLoad;
    [SerializeField] private Vector2 newScenePosition;
    [SerializeField] private Vector2 newSceneDirection;

    [Header("Fade in & out")]
    [SerializeField] private GameObject fadeInPanel;
    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private float fadeWait;

    [Header("Player Data")]
    [SerializeField] private Vector2Value playerPosition;
    [SerializeField] private Vector2Value playerDirection;

    public void Awake() {
        if(fadeInPanel != null) {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !other.isTrigger) {
            playerPosition.runtimeValue = newScenePosition;
            playerDirection.runtimeValue = newSceneDirection;
            StartCoroutine(FadeCoroutine());
        }
    }

    public IEnumerator FadeCoroutine() {
        if (fadeOutPanel != null) {
            GameObject panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeWait);

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneToLoad);
        while (!loadScene.isDone) {
            yield return null;
        }
    }
}
