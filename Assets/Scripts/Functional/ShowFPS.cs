using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour {

    public static float fps;

    private void Update() {
        //GetComponentInParent<Transform>().transform.Rotate(Vector3.up, Time.deltaTime * 2);
    }

    void OnGUI() {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("FPS: " + (int) fps);
    }
}