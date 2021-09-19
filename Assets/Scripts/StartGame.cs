using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGame : MonoBehaviour
{
  public string keyboardMessage = "Press <SPACE> to Start";
  public string touchMessage = "Touch Anywhere to Start";

  private UnityEngine.UI.Text startText;

  void Start()
  {
    startText = GetComponent<UnityEngine.UI.Text>();
    startText.text = SelectCorrectText();
  }

  void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space) || Input.touchCount > 0) {
      SceneManager.LoadScene("Gameplay");
    }
  }

  string SelectCorrectText()
  {
    if (SystemInfo.deviceType == DeviceType.Handheld) {
      return touchMessage;
    }
    return keyboardMessage;
  }
}
