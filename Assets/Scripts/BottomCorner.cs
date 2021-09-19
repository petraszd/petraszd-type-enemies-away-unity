using UnityEngine;
using System.Collections;

public class BottomCorner : MonoBehaviour {
  void OnEnable()
  {
    GameManager.OnGameBegin += OnGameBegin;
  }

  void OnDisable()
  {
    GameManager.OnGameBegin -= OnGameBegin;
  }

	void Start ()
  {
	}

  void OnGameBegin()
  {
    UpdatePositionAsBottomOfPlayArea();
  }

  void UpdatePositionAsBottomOfPlayArea()
  {
    Vector3 newPosition = transform.position;
    newPosition.y = GameRows.Instance.PlayArea.yMin;
    transform.position = newPosition;
  }
}
