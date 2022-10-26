using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {
    public string SceneToLoad;
    public Vector2Value playerPosition;
    public Vector2Value playerDirection; 

    public Vector2 newScenePosition;
    public Vector2 newSceneDirection;

    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;
    

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
