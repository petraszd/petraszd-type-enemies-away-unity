using UnityEngine;
using System.Collections;


public class PrazeHandler : MonoBehaviour
{
  public delegate void ErrorEvent();
  public delegate void FinishEvent(Player.Task task);

  public Player.Task playerTask;

  public Color32 typedColor;
  public Color32 remainingColor;

  [HideInInspector]
  public int nextLength;

  private string praze;
  private int typed = 0;

  private ErrorEvent onError = null;
  private FinishEvent onFinish = null;

  private UnityEngine.UI.Text prazeText = null;

  void Awake()
  {
    Statistics.OnNewLevel += OnNewLevel;
  }

  void OnDestroy()
  {
    Statistics.OnNewLevel -= OnNewLevel;
  }

  void Start()
  {
    prazeText = GetComponent<UnityEngine.UI.Text>();
    UpdateText();
  }

  public void TurnOn()
  {
    TurnOnOff(true);
  }

  public void TurnOff()
  {
    TurnOnOff(false);
  }

  public void AttachEvents(FinishEvent newOnFinish, ErrorEvent newOnError)
  {
    onFinish = newOnFinish;
    onError = newOnError;
  }

  public void TypeChar(char c)
  {
    if (praze.Length == 0 || praze[typed] != c) {
      onError();
      return;
    }

    typed ++;
    UpdateText();
    if (praze.Length <= typed) {
      onFinish(playerTask);
    }
  }

  public bool StartsWith(char c)
  {
    return praze[0] == c;
  }

  public void ResetPraze(string newPraze)
  {
    praze = newPraze.ToLower();
    typed = 0;
    UpdateText();
  }

  public char GetFirstLetter()
  {
    return praze[0];
  }

  private void OnNewLevel(int level, LevelCFG cfg)
  {
    nextLength = cfg.GetLength(playerTask);
  }

  private void TurnOnOff(bool on)
  {
    enabled = on;
    prazeText.enabled = on;
  }

  private void UpdateText()
  {
    if (prazeText == null || praze == null) {
      return;
    }
    prazeText.text = string.Format(
                       "<color=#{0}>{1}</color><color=#{2}>{3}</color>",
                       ColorToHex(typedColor), praze.Substring(0, typed),
                       ColorToHex(remainingColor), praze.Substring(typed));
  }

  private string ColorToHex(Color32 color)
  {
    return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}",
                         color.r, color.g, color.b, color.a);
  }
}
