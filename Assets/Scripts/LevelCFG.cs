using UnityEngine;


[System.Serializable]
public struct LevelCFG {
  public LevelCFG(int movingPrazeLength, int shootingPrazeLength, int toNextLevel)
  {
    this.movingPrazeLength = movingPrazeLength;
    this.shootingPrazeLength = shootingPrazeLength;
    this.toNextLevel = toNextLevel;
  }

  public int movingPrazeLength;
  public int shootingPrazeLength;
  public int toNextLevel;

  public int GetLength(Player.Task task)
  {
    if (task == Player.Task.Shoot) {
      return shootingPrazeLength;
    }
    return movingPrazeLength;
  }
}
