using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenueCamera : MonoBehaviour {
    [SerializeField] float CameraSpeed;
    private void FixedUpdate() {
        transform.Rotate(new Vector3(CameraSpeed * Time.deltaTime, CameraSpeed * Time.deltaTime, 0));
    }
}
