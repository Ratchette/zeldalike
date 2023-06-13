using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameStartCutscene : MonoBehaviour {
    [SerializeField] private BooleanValue gameStartCutscenePlayed;

    private PlayableDirector director;

    // Start is called before the first frame update
    void Start() {
        director = this.GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update() {
        if (!gameStartCutscenePlayed.runtimeValue) {
            director.Play();
            gameStartCutscenePlayed.runtimeValue = true;
        }
    }
}
