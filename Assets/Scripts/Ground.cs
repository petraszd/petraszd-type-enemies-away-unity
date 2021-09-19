using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Ground : MonoBehaviour
{
  public float outOfScreenDelta = 5.0f;
  public float animSpeed = 0.5f;

  private Vector3 normalPosition;
  private Vector3 duringGamePosition;

  void Awake()
  {
    SetPositionValues();
  }

  void OnEnable()
  {
    GameManager.OnGameEnd += OnGameEnd;
    GameManager.OnGameBegin += OnGameBegin;
  }

  void OnDisable()
  {
    GameManager.OnGameEnd -= OnGameEnd;
    GameManager.OnGameBegin -= OnGameBegin;
  }

  private void SetPositionValues()
  {
    normalPosition = transform.position;
    duringGamePosition = transform.position;
    transform.Translate(0.0f, -outOfScreenDelta, 0.0f);
  }

  private void OnGameBegin()
  {
    duringGamePosition.y = GameRows.Instance.PlayArea.yMin;
    transform.DOMove(duringGamePosition, animSpeed);
  }

  private void OnGameEnd()
  {
    transform.position = normalPosition;
  }
}
