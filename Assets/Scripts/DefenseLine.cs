using UnityEngine;
using System.Collections;

public class DefenseLine : MonoBehaviour
{
  public delegate void DefenseLineEvent();
  public static event DefenseLineEvent OnDefenseLineBreached;

  public Color lineColor;
  public float probeLength = 5.0f;

  public string layerName = "UI";
  public int layerOrder = 0;

  public float deltaFromLeft = 3.890135f;

  private LineRenderer lineRender;
  private BoxCollider2D coll2D;

  private int enemiesMask;
  private bool lineIsBreached = false;

  void Start()
  {
    lineRender = GetComponent<LineRenderer>();
    lineRender.sortingLayerName = layerName;
    lineRender.sortingOrder = layerOrder;

    coll2D = GetComponent<BoxCollider2D>();

    enemiesMask = LayerMask.GetMask("Enemies");

    Color startColor = lineColor;
    startColor.a = 0.0f;
    lineRender.SetColors(startColor, startColor);

    UpdatePositionRelativeToLeftSide();
  }

  void Update()
  {
    if (!lineIsBreached) {
      UpdateLineAlpha();
    }
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (!lineIsBreached && other.tag == "Enemy") {
      lineIsBreached = true;
      SetLineAlpha(1.0f);
      EmitDefenseLineBreached();
    }
  }

  void EmitDefenseLineBreached()
  {
    if (OnDefenseLineBreached != null) {
      OnDefenseLineBreached();
    }
  }

  void UpdateLineAlpha()
  {
    SetLineAlpha(GetAlphaByDistanceFromEnemies());
  }

  void SetLineAlpha(float a)
  {
    Color newColor = lineColor;
    newColor.a = a;
    lineRender.SetColors(newColor, newColor);
  }

  float GetAlphaByDistanceFromEnemies()
  {
    float result = 0.0f;

    float x = coll2D.bounds.max.x;
    foreach (float y in GameRows.Instance.Rows) {
      RaycastHit2D hit = Physics2D.Raycast(new Vector2(x, y), Vector2.right,
                                           probeLength, enemiesMask);
      if (hit && (1.0f - hit.fraction) > result) {
        result = (1.0f - hit.fraction);
      }
    }

    return result;
  }

  void UpdatePositionRelativeToLeftSide()
  {
    Vector3 newPosition = transform.position;
    newPosition.x = GameRows.Instance.PlayArea.xMin + deltaFromLeft;
    transform.position = newPosition;
  }
}
