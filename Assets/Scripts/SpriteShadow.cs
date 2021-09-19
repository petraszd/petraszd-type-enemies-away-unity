using UnityEngine;
using System.Collections;

public class SpriteShadow : MonoBehaviour
{
  public Vector3 shadowDelta = new Vector3(0.05f, -0.05f, 0.0f);

  SpriteRenderer spriteRenderer;

  void Start ()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    UpdateShadow();
  }

  public void UpdateShadow()
  {
    SpriteRenderer parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = parentRenderer.sprite;
    transform.position = transform.parent.position;
    transform.Translate(shadowDelta);
  }
}
