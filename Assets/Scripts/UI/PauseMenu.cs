using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    private bool menuOpen = false;

    [Header("Exit transition")]
    [SerializeField] private string nextScene = "StartMenu";
    [SerializeField] private float fadeWait = 1.0f;
    [SerializeField] private GameObject fadeOutPanel;

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown(InputMap.BUTTON_ESCAPE)) {
            if (menuOpen) {
                CloseMenu();
            } else if(!isTimePaused()){
                OpenMenu();
            }

            // If this menu is NOT open, but time is paused, that means that a different menu is open.
            // This menu should NOT respond while another menu is open.
        }
    }

    public void ResumeGame() {
        CloseMenu();
    }

    public void ExitToMainMenu() {
        CloseMenu();
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

    private void OpenMenu() {
        Time.timeScale = 0;
        menuOpen = true;
        pauseMenu.SetActive(true);
    }

    private void CloseMenu() {
        Time.timeScale = 1;
        menuOpen = false;
        pauseMenu.SetActive(false);
    }

    private bool isTimePaused() {
        return Time.timeScale == 0;
    }
}
