using UnityEngine;
using System.Collections;

public class RightCorner : MonoBehaviour
{
  void Start ()
  {
    Vector3 newPosition = transform.position;
    newPosition.x = GameRows.Instance.PlayArea.xMax;
    transform.position = newPosition;
  }
}
