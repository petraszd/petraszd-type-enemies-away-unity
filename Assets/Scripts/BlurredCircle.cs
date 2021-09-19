using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BlurredCircle : MonoBehaviour
{
  public float width = 2.2f;
  public float distance = 46.0f;
  public float timePerUnit = 2.0f;

  private Vector3 startPosition;
  private Vector3 endPosition;

  void Start ()
  {
    startPosition = transform.position;
    endPosition = startPosition;
    endPosition.x = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + width;
    MoveRight();
  }

  private void MoveRight()
  {
    float timer = GetTimer();

    transform.position = startPosition;
    transform.DOMove(endPosition, timer)
    .OnComplete(ResetMoving)
    .SetEase(Ease.Linear);
  }

  private void ResetMoving()
  {
    startPosition.x = endPosition.x - distance;
    MoveRight();
  }

  private float GetTimer()
  {
    float delta = endPosition.x - startPosition.x;
    return delta * timePerUnit;
  }
}
