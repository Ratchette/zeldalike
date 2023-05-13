using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {
    private CinemachineVirtualCamera cmCamera;
    private CinemachineBasicMultiChannelPerlin channelsPerlin;

    [SerializeField] private float shakeTime = 0.15f;
    [SerializeField] private float shakeAmplitude = 4;

    public void Awake() {
        cmCamera = GetComponent<CinemachineVirtualCamera>();
        channelsPerlin = cmCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeScreen() {
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine() {
        channelsPerlin.m_AmplitudeGain = shakeAmplitude;
        yield return new WaitForSeconds(shakeTime);
        channelsPerlin.m_AmplitudeGain = 0f;
    }
}
