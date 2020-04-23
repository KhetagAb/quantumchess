using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour {

    public static float fps;

    void OnGUI() {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("FPS: " + (int) fps);
    }
}