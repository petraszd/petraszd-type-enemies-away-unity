using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
  public delegate void KillEvent(int nth);
  public static event KillEvent OnKill;

  public float speed = 4.0f;
  private int nKills = 0;

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Enemy") {
      nKills++;
      EmitKill();
      AudioManager.PlayKill();

      Enemy enemy = other.gameObject.GetComponent<Enemy>();
      enemy.StartDying();
    }
  }

  public void StartShooting()
  {
    nKills = 0;
    GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0.0f);
  }

  void EmitKill()
  {
    if (OnKill != null) {
      OnKill(nKills);
    }
  }
}
