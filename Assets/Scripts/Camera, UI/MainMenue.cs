using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum SceneIndex {
    MainMenue = 0,
    ChessBoard = 1
}

public class MainMenue : MonoBehaviour {
    [SerializeField] private Animation transition;

    [SerializeField] private UnityEngine.UI.Slider slider;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float CameraSpeed;
    private void FixedUpdate() {
        transform.Rotate(new Vector3(CameraSpeed * Time.deltaTime, CameraSpeed * Time.deltaTime, 0));
    }

    public void editSlider() {
        text.text = "Max. harmonics: 2<sup>" + slider.value.ToString() + "</sup>";
    }

    public void playGame() {
        GameManager.limitLayers = (int) slider.value;

        StartCoroutine(LoadLevel((int) SceneIndex.ChessBoard));
    }

    IEnumerator LoadLevel(int sceneIndex) {
        transition.Play();

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneIndex);
    }

    public void onRule() {
        Application.OpenURL("https://vk.com/doc589450615_547320274");
    }

    public void exitGame() {
        Application.Quit();
    }
}
