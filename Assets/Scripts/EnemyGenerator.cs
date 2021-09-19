using UnityEngine;
using System.Collections;

public class EnemyGenerator : PrefabPool<Enemy>
{
  public float delay = 2.0f;
  public float delayRandom = 0.1f;
  public float enemySpeed = 1.0f;
  public GameObject enemyPrefab;

  public float deltaToRight = 3.109865f;

  private int prevRandomIndex = -1;

  void OnEnable()
  {
    GameManager.OnGameBegin += OnGameBegin;
  }

  void OnDisable()
  {
    GameManager.OnGameBegin -= OnGameBegin;
  }

  protected override void Start()
  {
    base.Start();
    UpdatePositionRelativeToRightSide();
  }

  void OnGameBegin()
  {
    StartCoroutine(GenerateEnemies());
  }

  IEnumerator GenerateEnemies()
  {
    yield return null;
    for (;;) {
      GenerateOneEnemy();
      yield return new WaitForSeconds(GetWaitDelay());
    }
  }

  void GenerateOneEnemy()
  {
    int rowIndex = GameRows.Instance.GetRandomIndex(prevRandomIndex);

    Vector3 enemyPos = transform.position;
    enemyPos.y = GameRows.Instance.GetYByIndex(rowIndex);
    prevRandomIndex = rowIndex;

    Enemy enemy = GetFromPool(enemyPos);
    enemy.StartMoving(enemySpeed);
  }

  float GetWaitDelay()
  {
    float half = delayRandom / 2.0f;
    return delay + Random.Range(-half, half);
  }

  void UpdatePositionRelativeToRightSide()
  {
    Vector3 newPosition = transform.position;
    newPosition.x = GameRows.Instance.PlayArea.xMax + deltaToRight;
    transform.position = newPosition;
  }
}
