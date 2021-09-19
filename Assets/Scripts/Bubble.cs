using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour
{
  public SpriteRenderer spriteRenderer;
  public Sprite[] sprites;

  public float xRandomScale = 0.1f;
  public float yRandomScale = 0.1f;

  public float fadeOutBegin = 2.0f;
  public float fadeOutEnd = 0.3f;

  private IEnumerator upCoroutine = null;
  private float origAlpha = 0.0f;

  public void MoveUp(float yMax, float speed, float swing)
  {
    Randomize();
    if (upCoroutine != null) {
      StopCoroutine(upCoroutine);
    }
    upCoroutine = MoveUpLoop(yMax, speed, swing);
    StartCoroutine(upCoroutine);
  }

  void Randomize()
  {
    RandomizeScale();
    RandomizeRotation();
    RandomizeSprite();
  }

  void RandomizeScale()
  {
    Vector3 newScale = Vector3.one;

    newScale.x += Random.Range(-xRandomScale, xRandomScale);
    newScale.y += Random.Range(-yRandomScale, yRandomScale);

    transform.localScale = newScale;
  }

  void RandomizeRotation()
  {
    Vector3 newRotation = Vector3.zero;
    newRotation.z = Random.Range(0.0f, 360.0f);

    transform.localEulerAngles = newRotation;
  }

  void RandomizeSprite()
  {
    spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    if (origAlpha != 0.0f) {
      SetAlpha(origAlpha);
    }
  }

  void SetAlpha(float a)
  {
    Color newColor = spriteRenderer.color;
    newColor.a = a;
    spriteRenderer.color = newColor;
  }

  IEnumerator MoveUpLoop(float yMax, float speed, float swing)
  {
    origAlpha = spriteRenderer.color.a;
    Vector3 origScale = transform.localScale;

    float delta;
    float movingTime = 0.0f;

    while (true) {
      transform.Translate(
        Vector3.up * Time.deltaTime * speed,
        Space.World
      );
      transform.Translate(
        Vector3.right * Time.deltaTime * Mathf.Cos(movingTime) * swing,
        Space.World
      );

      delta = yMax - transform.position.y - fadeOutEnd;
      if (delta < 0.0f) {
        break;
      }
      if (delta < fadeOutBegin) {
        float t = delta / fadeOutBegin;
        SetAlpha(Mathf.Lerp(0.0f, origAlpha, t));
        transform.localScale = new Vector3(
          Mathf.Lerp(0.6f, origScale.x, t),
          Mathf.Lerp(0.6f, origScale.y, t),
          1.0f
        );
      }
      movingTime += Time.deltaTime;
      yield return null;
    }
    gameObject.SetActive(false);
  }
}
