using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Statistics : MonoBehaviour
{
  public delegate void StatisticsEvent(int level, LevelCFG cfg);
  public static event StatisticsEvent OnNewLevel;

  public GameObject endGamePanel;
  public UnityEngine.UI.Text pointsText;
  public string pointsFormat = "Points: {0:D6}";
  public UnityEngine.UI.Text bestScoreText;
  public UnityEngine.UI.Text statisticsText;
  [Multiline]
  public string statisticsFormat = @"Points: {0}
Correct: {1}
Mistakes: {2}
Kills: {3}
Words: {4}";
  public Animator levelAnimator;
  public UnityEngine.UI.Text levelText;
  public string levelFormat = "Level {0}";

  public int pointsForCorrect = 1;
  public int pointsForMistake = -5;
  public int pointsForKill = 10;

  public LevelCFG[] definedLevels;

  public UnityEngine.UI.Image[] images;

  private int nPoints = 0;
  private int nCorrect = 0;
  private int nWords = 0;
  private int nMistake = 0;
  private int nKills = 0;
  private int level = 0;
  private LevelCFG currentLevelCFG;

  void OnEnable()
  {
    KeyboardManager.OnCorrect += OnTypingCorrect;
    KeyboardManager.OnMistake += OnTypingMistake;
    KeyboardManager.OnWord += OnTypingWord;

    Bullet.OnKill += OnEnemyKilled;

    GameManager.OnGameEnd += OnGameEnd;
  }

  void OnDisable()
  {
    KeyboardManager.OnCorrect -= OnTypingCorrect;
    KeyboardManager.OnMistake -= OnTypingMistake;
    KeyboardManager.OnWord -= OnTypingWord;

    Bullet.OnKill -= OnEnemyKilled;

    GameManager.OnGameEnd -= OnGameEnd;
  }

  void Start()
  {
    ResetCounters();
    UpdateCurrentLevelCFG();
    UpdatePointsText();
  }

  void UpdatePointsText()
  {
    pointsText.text = string.Format(pointsFormat, nPoints);
  }

  void ResetCounters()
  {
    nCorrect = 0;
    nMistake = 0;
    nKills = 0;
    nPoints = 0;
    nWords = 0;
  }

  void UpdatePoints(int delta)
  {
    nPoints += delta;
    CheckForNewLevel();
    UpdatePointsText();
  }

  void UpdateCurrentLevelCFG()
  {
    currentLevelCFG = GetCurrentLevelCFG();
    DisplayNewLevel();
    EmitNewLevel();
  }

  void DisplayNewLevel()
  {
    levelText.text = string.Format(levelFormat, (level + 1));
    levelAnimator.SetTrigger("LevelChanged");
  }

  void CheckForNewLevel()
  {
    if (nPoints > currentLevelCFG.toNextLevel) {
      level++;
      UpdateCurrentLevelCFG();
    }
  }

  LevelCFG GetCurrentLevelCFG()
  {
    if (level < definedLevels.Length) {
      return definedLevels[level];
    }
    LevelCFG last = definedLevels[definedLevels.Length - 1];

    int levelDiff = level - definedLevels.Length;
    return new LevelCFG(
             last.movingPrazeLength + (levelDiff / 2),
             last.shootingPrazeLength + 1 + (levelDiff / 2),
             last.toNextLevel + 1000 * levelDiff
           );
  }

  void UpdateBestScoreIfNeeded()
  {
    int oldBestScore = PlayerPrefs.GetInt("best-score", 0);
    if (nPoints > oldBestScore) {
      PlayerPrefs.SetInt("best-score", nPoints);
      bestScoreText.enabled = true;
    }
  }

  void EmitNewLevel()
  {
    if (OnNewLevel != null) {
      OnNewLevel(level, currentLevelCFG);
    }
  }

  void OnTypingCorrect()
  {
    nCorrect++;
    UpdatePoints(pointsForCorrect);
  }

  void OnTypingMistake()
  {
    nMistake++;
#if !UNITY_WEBGL
    Handheld.Vibrate();
#endif
    UpdatePoints(pointsForMistake);
  }

  void OnTypingWord()
  {
    nWords++;
  }

  void OnEnemyKilled(int nth)
  {
    nKills++;
    UpdatePoints(pointsForKill);
  }

  void OnGameEnd()
  {
#if !UNITY_WEBGL
    Handheld.Vibrate();
#endif
    UpdateBestScoreIfNeeded();
    pointsText.enabled = false;
    statisticsText.text = string.Format(
                            statisticsFormat,
                            nPoints, nCorrect, nMistake, nKills, nWords);
    endGamePanel.SetActive(true);
  }
}
