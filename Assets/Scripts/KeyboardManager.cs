using UnityEngine;
using System.Collections;


public class KeyboardManager : MonoBehaviour
{
  public delegate void TypingEvent();
  public static event TypingEvent OnMistake;
  public static event TypingEvent OnCorrect;
  public static event TypingEvent OnWord;

  private const string PREFIX = "a";

  public PrazeHandler[] handlers = null;
  public Player player;

  private PrazeHandler handlerInControl = null;
  private TouchScreenKeyboard softKeyboard = null;

  void OnEnable()
  {
    GameManager.OnGameBegin += OnGameBegin;
    GameManager.OnGameEnd += OnGameEnd;

    OpenKeyboardIfNeeded();
  }

  void OnDisable()
  {
    GameManager.OnGameEnd -= OnGameEnd;
    GameManager.OnGameBegin -= OnGameBegin;

    if (softKeyboard != null) {
      softKeyboard.active = false;
      softKeyboard = null;
    }
  }

  void Start()
  {
    ResetAllPrazes();
  }

  void Update()
  {
    ProccessPCInput();
    if (OpenKeyboardIfNeeded()) {
      ProccessSoftKeyboardInput();
    }
  }

  void ResetAllPrazes()
  {
    string canStartWith = "";
    foreach (PrazeHandler handler in handlers) {
      handler.AttachEvents(OnActionFinished, OnError);

      string newPraze = PrazeGenerator.Generate(
                          canStartWith,
                          handler.nextLength);
      handler.ResetPraze(newPraze);
      canStartWith += newPraze[0];
    }
  }

  bool OpenKeyboardIfNeeded()
  {
#if UNITY_WEBGL
    return false;
#else
    if (Application.isShowingSplashScreen || !TouchScreenKeyboard.isSupported) {
      return false;
    }

    if (!TouchScreenKeyboard.visible) {
      TouchScreenKeyboard.hideInput = true;
      softKeyboard = TouchScreenKeyboard.Open(
                       PREFIX, TouchScreenKeyboardType.Default, false, false
                     );
    }
    return true;
#endif
  }

  void ProccessPCInput()
  {
    foreach (char c in Input.inputString) {
      ProccessChar(c);
    }
  }

  void ProccessSoftKeyboardInput()
  {
    foreach (char c in softKeyboard.text.Substring(PREFIX.Length)) {
      ProccessChar(c);
    }
    softKeyboard.text = PREFIX;
  }

  void ProccessChar(char c)
  {
    c = char.ToLower(c);

    if (handlerInControl == null) {
      FindHandlerToTakeControl(c);
    }

    if (handlerInControl == null) {
      OnError();
    } else {
      ProccessCharWithHandlerInControl(c);
    }
  }

  void ProccessCharWithHandlerInControl(char c)
  {
    EmitCorrect();
    handlerInControl.TypeChar(c);
  }

  void FindHandlerToTakeControl(char c)
  {
    foreach (PrazeHandler handler in handlers) {
      if (!handler.enabled) {
        continue;
      }

      if (handler.StartsWith(c)) {
        handlerInControl = handler;
        break;
      }
    }
  }

  void ResetHandlerInControl()
  {
    string newPraze = PrazeGenerator.Generate(
                        GetOccupiedFirstLetters(),
                        handlerInControl.nextLength);
    handlerInControl.ResetPraze(newPraze);
    handlerInControl = null;
  }

  string GetOccupiedFirstLetters()
  {
    int size = handlers.Length;
    if (handlerInControl != null) {
      size--;
    }

    char[] result = new char[size];

    int i = 0;
    foreach (PrazeHandler handler in handlers) {
      if (handler != handlerInControl) {
        result[i++] = handler.GetFirstLetter();
      }
    }

    return new string(result);
  }

  void UpdateHandlersEnabledStates()
  {
    foreach (PrazeHandler handler in handlers) {
      handler.TurnOn();
    }

    if (GameRows.Instance.IsTopIndex(player.RowIndex)) {
      handlers[0].TurnOff();
    } else if (GameRows.Instance.IsBottomIndex(player.RowIndex)) {
      handlers[handlers.Length - 1].TurnOff();
    }
  }

  void OnError()
  {
    EmitMistake();
  }

  void OnActionFinished(Player.Task task)
  {
    EmitWord();
    player.DoTask(task);
    UpdateHandlersEnabledStates();
    ResetHandlerInControl();
  }

  void OnGameBegin()
  {
    ResetAllPrazes();
  }

  void OnGameEnd()
  {
    enabled = false;
  }

  void EmitMistake()
  {
    if (OnMistake != null) {
      OnMistake();
    }
  }

  void EmitCorrect()
  {
    if (OnCorrect != null) {
      OnCorrect();
    }
  }

  void EmitWord()
  {
    if (OnWord != null) {
      OnWord();
    }
  }
}
