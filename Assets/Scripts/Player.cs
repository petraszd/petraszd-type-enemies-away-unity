using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Player: PrefabPool<Bullet>
{
  private Vector2 BULLET_DELTA = new Vector2(0.06f, -0.35f);

  public enum Task { MoveUp, MoveDown, Shoot };

  public float driftSpeed = 4.0f;
  public float deltaFromLeft = 1.790135f;

  private Rigidbody2D rb2D;
  private Animator animator;

  private Vector2 startPos;
  private Vector2 endPos;
  private int rowIndex = 0;
  public int RowIndex
  {
    get {
      return  rowIndex;
    }
  }

  void OnEnable()
  {
    GameManager.OnGameBegin += OnGameBegin;
    GameManager.OnGameEnd += OnGameEnd;
  }

  void OnDisable()
  {
    GameManager.OnGameBegin -= OnGameBegin;
    GameManager.OnGameEnd -= OnGameEnd;
  }

  protected override void Start()
  {
    base.Start();

    rb2D = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    CenterSelf();
  }

  public void DoTask(Task task)
  {
    switch (task) {
    case Task.MoveUp:
      DoMoveUp();
      break;
    case Task.MoveDown:
      DoMoveDown();
      break;
    case Task.Shoot:
      DoShoot();
      break;
    default:
      break;
    }
  }

  public void ShootBullet()
  {
    Bullet bullet = GetFromPool(rb2D.position + BULLET_DELTA);
    bullet.StartShooting();
    AudioManager.PlayShoot();
  }

  void OnGameBegin()
  {
    StartCoroutine(CenterSelfAndEnterScreen());
  }

  void OnGameEnd()
  {
    animator.SetTrigger("Die");
    StartCoroutine(DriftDown());
  }

  void CenterSelf()
  {
    rowIndex = GameRows.Instance.GetCenterIndex();

    Vector2 newPosition = rb2D.position;
    newPosition.y = GameRows.Instance.GetYByIndex(rowIndex);
    rb2D.MovePosition(newPosition);
  }

  IEnumerator CenterSelfAndEnterScreen()
  {
    yield return null;
    CenterSelf();
    yield return null;
    Vector2 playPos = new Vector2(
      GameRows.Instance.PlayArea.xMin + deltaFromLeft,
      rb2D.position.y
    );
    MoveTo(playPos, 0.7f);  // TODO: magic number
  }

  IEnumerator DriftDown()
  {
    yield return null;
    for (;;) {
      Vector2 newPosition = rb2D.position;
      newPosition.y -= driftSpeed * Time.deltaTime;
      rb2D.MovePosition(newPosition);
      yield return null;
    }
  }

  void DoMoveUp()
  {
    DoMove(-1);
  }

  void DoMoveDown()
  {
    DoMove(1);
  }

  void DoMove(int indexDelta)
  {
    rowIndex = GameRows.Instance.ClampIndex(rowIndex + indexDelta);

    startPos = rb2D.position;
    endPos = new Vector2(startPos.x, GameRows.Instance.GetYByIndex(rowIndex));

    MoveTo(endPos, 0.4f);  // TODO: magic number
  }

  void DoShoot()
  {
    animator.SetTrigger("Shoot");
  }

  void MoveTo(Vector2 newPosition, float time)
  {
    rb2D.DOMove(newPosition, time).SetEase(Ease.OutQuad);
  }
}
