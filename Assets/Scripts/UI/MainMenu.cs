using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private string startScene = "Overworld";
    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private float fadeWait = 1.0f;


    public void StartGame() {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine() {
        GameObject panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(startScene);
        while (!loadScene.isDone) {
            yield return null;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
