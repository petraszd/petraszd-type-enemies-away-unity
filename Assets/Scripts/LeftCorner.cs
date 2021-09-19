using UnityEngine;
using System.Collections;

public class LeftCorner : MonoBehaviour
{
  void Start ()
  {
    Vector3 newPosition = transform.position;
    newPosition.x = GameRows.Instance.PlayArea.xMin;
    transform.position = newPosition;
  }
}
