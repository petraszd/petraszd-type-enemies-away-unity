using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CornerManager : MonoBehaviour {
  public SpriteRenderer[] corners;

  private Sequence[] sequences;

  void OnEnable()
  {
    KeyboardManager.OnMistake += OnMistake;
  }

  void OnDisable()
  {
    KeyboardManager.OnMistake -= OnMistake;
  }

	void Start()
  {
    sequences = new Sequence[corners.Length];
    for (int i = 0; i < sequences.Length; ++i) {
      sequences[i] = null;
    }
	}

  void FlashCorners()
  {
    for (int i = 0; i < corners.Length; ++i) {
      FlashCorner(corners[i], i);
    }
  }

  void FlashCorner(SpriteRenderer corner, int index)
  {
    if (sequences[index] != null) {
      sequences[index].Complete(true);
    }

    Color oldColor = corner.color;
    Color newColor = corner.color;
    newColor.a = 1;

    Sequence sequence = DOTween.Sequence();
    sequence.Append(corner.DOColor(newColor, 0.2f));
    sequence.Append(corner.DOColor(oldColor, 0.4f));
    sequence.OnComplete(()=>OnFlashCornerComplete(index));

    sequences[index] = sequence;
  }

  void OnMistake()
  {
    FlashCorners();
  }

  void OnFlashCornerComplete(int index)
  {
    sequences[index] = null;
  }
}
