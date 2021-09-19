using UnityEngine;
using System.Collections;

public class BubbleManager : PrefabPool<Bubble>
{
  public float minDelay = 0.1f;
  public float randomDelay = 2.0f;
  public float minSpeed = 0.8f;
  public float maxSpeed = 1.4f;
  public float minSwing = 0.1f;
  public float maxSwing = 0.2f;

  private float xMin;
  private float xMax;

  private float yMax;

  protected override void Start()
  {
    base.Start();

    xMin = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
    xMax = Camera.main.ViewportToWorldPoint(Vector3.right).x;
    yMax = Camera.main.ViewportToWorldPoint(Vector3.up).y;

    StartCoroutine(BubbleGenerationLoop());
  }

  IEnumerator BubbleGenerationLoop()
  {
    while (true) {
      GenerateRandomBottomBubble();
      yield return GetRandomWait();
    }
  }

  WaitForSeconds GetRandomWait()
  {
    return new WaitForSeconds(Random.Range(minDelay, minDelay + randomDelay));
  }

  void GenerateRandomBottomBubble()
  {
    Vector3 bubblePos = transform.position;
    bubblePos.x = Random.Range(xMin, xMax);
    GenerateBubble(bubblePos);
  }

  void GenerateBubble(Vector3 bubblePos)
  {
    Bubble bubble = GetFromPool(bubblePos);
    float speed = Random.Range(minSpeed, maxSpeed);
    float swing = Random.Range(minSwing, maxSwing);
    bubble.MoveUp(yMax, speed, swing);
  }
}
