using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenue : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Slider slider;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float CameraSpeed;
    private void FixedUpdate() {
        transform.Rotate(new Vector3(CameraSpeed * Time.deltaTime, CameraSpeed * Time.deltaTime, 0));
    }

    public void editSlider() {
        text.text = "Quantum rate: " + slider.value.ToString();
    }

    public void playGame() {
        GameManager.limitLayers = (int) slider.value;
        SceneManager.LoadScene("ChessBoard");
    }

    public void onRule() {
        Application.OpenURL("https://vk.com/doc589450615_547320274");
    }

    public void exitGame() {
        Application.Quit();
    }
}
