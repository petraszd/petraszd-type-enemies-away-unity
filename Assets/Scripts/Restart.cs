using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Restart : MonoBehaviour
{
  public string keyboardMessage = "Press 'R' to Restart";
  public string touchMessage = "Touch Anywhere to Restart";

  private UnityEngine.UI.Text restartText;

  void OnEnable()
  {
    GameManager.OnGameEnd += OnGameEnd;
  }

  void OnDisable()
  {
    GameManager.OnGameEnd -= OnGameEnd;
  }

  void Start()
  {
    restartText = GetComponent<UnityEngine.UI.Text>();
  }

  void OnGameEnd()
  {
    StartCoroutine(WaitingForRestart());
  }

  IEnumerator WaitingForRestart()
  {
    yield return new WaitForSeconds(1.0f);
    restartText.text = SelectCorrectText();
    restartText.enabled = true;
    for (;;) {
      yield return null;
      if (Input.GetKeyUp(KeyCode.R) || Input.touchCount > 0) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
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
