using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private string startScene = "Overworld";
    [SerializeField] private GameObject fadeOutPanel;
    [SerializeField] private GameObject fadeInPanel;
    [SerializeField] private float fadeWait = 1.0f;

    [SerializeField] private ScriptableObject[] objectsToReset;

    public void Awake() {
        if (fadeInPanel != null) {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1f);
        }
    }

    public void StartGame() {
        // FIXME - this should only be reset if the player is starting a new game (vs loading an old save)
        foreach(ScriptableObject o in objectsToReset) {
            if(o.GetType() == typeof(FloatValue)) {
                (o as FloatValue).runtimeValue = (o as FloatValue).initialValue;
            } else if (o.GetType() == typeof(BooleanValue)) {
                (o as BooleanValue).runtimeValue = (o as BooleanValue).initialValue;
            }
        }
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
