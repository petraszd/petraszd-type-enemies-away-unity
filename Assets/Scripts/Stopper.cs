using UnityEngine;
using System.Collections;

public class Stopper : MonoBehaviour
{
  public string tagToStop;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == tagToStop) {
      other.gameObject.SetActive(false);
    }
  }
}
