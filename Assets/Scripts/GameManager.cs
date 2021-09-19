using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
  private const float KEYBOARD_WAIT_PAUSE = 0.05f;
  private const int KEYBOARD_WAIT_TRIES = 100;
  private const float KEYBOARD_HEIGHT_ANIM_DELTA = 0.1f;

  public delegate void GameEvent();
  public static event GameEvent OnGameBegin;
  public static event GameEvent OnGameEnd;

  void OnEnable()
  {
    DefenseLine.OnDefenseLineBreached += OnDefenseLineBreached;
  }

  void OnDisable()
  {
    DefenseLine.OnDefenseLineBreached -= OnDefenseLineBreached;
  }

  void Start()
  {
    StartCoroutine(WaitForKeyboard());
  }

  IEnumerator WaitForKeyboard()
  {
    if (!TouchScreenKeyboard.isSupported) {
      EmitBeginGame();
    } else {
      WaitForSeconds wait = new WaitForSeconds(KEYBOARD_WAIT_PAUSE);
      float prevZoneHeight = 0.0f;
      float zoneHeight = 0.0f;
      for (int i = 0; i < KEYBOARD_WAIT_TRIES; ++i) {
        yield return wait;
        zoneHeight = GameRows.Instance.UpdateZoneIfKeyboardIsOpen();
        if (zoneHeight > 0.0f && Mathf.Abs(zoneHeight - prevZoneHeight) < KEYBOARD_HEIGHT_ANIM_DELTA) {
          break;
        }
        prevZoneHeight = zoneHeight;
      }
      EmitBeginGame();
    }
  }

  void OnDefenseLineBreached()
  {
    EmitEndGame();
  }

  void EmitBeginGame()
  {
    if (OnGameBegin != null) {
      OnGameBegin();
    }
  }

  void EmitEndGame()
  {
    if (OnGameEnd != null) {
      OnGameEnd();
    }
  }
}
