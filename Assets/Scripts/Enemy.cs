using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
  public Sprite[] sprites;
  public SpriteRenderer spriteRenderer;
  public SpriteShadow spriteShadow;

  public float verticalSpeed = 4.0f;
  public float verticalAmount = 0.5f;

  public float hueVariation = 0.025f;
  public float saturationVariation = 0.2f;

  public float dyingSpeed = 0.3f;

  public float movingFactor;  // Updated by animator

  private float movingSpeed;
  private float movingTime;
  private Rigidbody2D rb2D;
  private Animator animator;
  private Collider2D coll2D;

  private float initHue;
  private float initSaturation;
  private float initValue;
  private bool isDying;

  void Start()
  {
    movingFactor = 0.0f;
    isDying = false;

    rb2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    coll2D = GetComponent<Collider2D>();

    Color.RGBToHSV(spriteRenderer.color,
                   out initHue, out initSaturation, out initValue);
  }

  void Update()
  {
    if (isDying) {
      UpdateWhenDying();
    } else {
      UpdateWhenMoving();
    }
  }

  public void StartMoving(float speed)
  {
    movingSpeed = speed;
    movingTime = 0.0f;
    isDying = false;
    StartCoroutine(StartMovingDelayed());
  }

  public void StartDying()
  {
    animator.SetBool("IsDying", true);
    isDying = true;
    coll2D.enabled = false;
  }

  public void DeactivateAndReset()
  {
    animator.SetBool("IsDying", false);
    isDying = false;
    gameObject.SetActive(false);
    coll2D.enabled = true;
    ResetAlpha();
  }

  void UpdateWhenMoving()
  {
    rb2D.velocity = new Vector2(
      -movingFactor * movingSpeed,
      Mathf.Cos(movingTime * verticalSpeed) / Mathf.PI * verticalAmount
    );
    movingTime += Time.deltaTime;
  }

  void UpdateWhenDying()
  {
    rb2D.velocity = new Vector2(movingFactor * dyingSpeed, 0.0f);
  }

  void ResetAlpha()
  {
    Color newColor = spriteRenderer.color;
    newColor.a = 1.0f;
    spriteRenderer.color = newColor;
  }

  void SetRandomSprite()
  {
    spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    spriteRenderer.color = Random.ColorHSV(
                             initHue + hueVariation,
                             initHue - hueVariation,
                             initSaturation - saturationVariation,
                             initSaturation + saturationVariation,
                             initValue,
                             initValue);
    spriteShadow.UpdateShadow();
  }

  IEnumerator StartMovingDelayed()
  {
    yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
    animator.SetBool("IsMoving", true);
    SetRandomSprite();
  }
}
