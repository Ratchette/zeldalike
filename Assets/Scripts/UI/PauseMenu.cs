using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;

    [Header("Exit transition")]
    [SerializeField] private string nextScene = "StartMenu";
    [SerializeField] private float fadeWait = 1.0f;
    [SerializeField] private GameObject fadeOutPanel;

    private bool isPaused = false;


    // Update is called once per frame
    void Update() {
        if (!isPaused) {
            if (Input.GetButtonDown(InputMap.BUTTON_PAUSE)) {
                PauseTime();
            }

        } else if (Input.GetButtonDown(InputMap.BUTTON_PAUSE) ||
                    Input.GetButtonDown(InputMap.BUTTON_ESCAPE)) {
            ResumeTime();
        }
    }

    public void ResumeGame() {
        ResumeTime();
    }

    public void ExitToMainMenu() {
        ResumeTime();
        StartCoroutine(loadStartMenu());
    }

    private IEnumerator loadStartMenu() {
        GameObject panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);

        AsyncOperation loadScene = SceneManager.LoadSceneAsync(nextScene);
        while (!loadScene.isDone) {
            yield return null;
        }
    }

    private void PauseTime() {
        Time.timeScale = 0;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    private void ResumeTime() {
        Time.timeScale = 1;
        isPaused = false;
        pauseMenu.SetActive(false);
    }
}
