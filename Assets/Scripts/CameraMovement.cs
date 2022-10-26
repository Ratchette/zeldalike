using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing;

    void FixedUpdate() {
        if((transform.position.x != target.position.x) || (transform.position.y != target.position.y)) {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }
}
