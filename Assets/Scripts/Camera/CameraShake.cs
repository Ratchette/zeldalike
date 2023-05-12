using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {
    private CinemachineVirtualCamera _camera;
    private CinemachineBasicMultiChannelPerlin _channelsPerlin;

    [SerializeField] private float shakeTime = 0.15f;
    [SerializeField] private float shakeAmplitude = 4;

    public void Awake() {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _channelsPerlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (_camera == null) {
            Debug.LogFormat("Camera is null");
        }

        if ( _channelsPerlin == null) {
            Debug.LogFormat("Perlin is null");
        }
    }

    public void ShakeScreen() {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine() {
        Debug.LogFormat("Perlin {0}", _channelsPerlin);

        _channelsPerlin.m_AmplitudeGain = shakeAmplitude;
        yield return new WaitForSeconds(shakeTime);
        _channelsPerlin.m_AmplitudeGain = 0f;
    }
}
