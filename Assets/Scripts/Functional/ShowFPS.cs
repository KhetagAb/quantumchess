using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityTemplateProjects;

public class ShowFPS : MonoBehaviour {
    public static float fps;
    [SerializeField] private Vector3 targetTo;

    private void Start() {
        StartCoroutine(moveTo());
    }

    IEnumerator moveTo() {
        while (SceneManager.GetActiveScene().buildIndex != (int) SceneIndex.ChessBoard) {
            yield return null;
        }

        do {
            transform.position = Vector3.Lerp(transform.position, targetTo, 0.1f);
            yield return new WaitForFixedUpdate();
        } while (transform.position != targetTo);

        GetComponentInChildren<SimpleCameraController>().enabled = true;
    }

    void OnGUI() {
        fps = 1.0f / Time.deltaTime;
        GUILayout.Label("FPS: " + (int) fps);
    }
}