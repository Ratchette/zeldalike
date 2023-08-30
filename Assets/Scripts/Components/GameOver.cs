using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    [SerializeField] private GameObject gameOverTransition;

    // Update is called once per frame
    public void Play() {
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine() {
        // let death animation play
        yield return new WaitForSeconds(1f);

        Instantiate(gameOverTransition, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        AsyncOperation loadScene = SceneManager.LoadSceneAsync("GameOver");
        while (!loadScene.isDone) {
            yield return null;
        }
    }
}
