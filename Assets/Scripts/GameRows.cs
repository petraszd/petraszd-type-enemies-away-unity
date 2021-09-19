using UnityEngine;
using System.Collections;

public class GameRows
{
  static private GameRows instance = null;
  static public GameRows Instance
  {
    get {
      if (instance == null) {
        instance = new GameRows();
      }
      return instance;
    }
  }

  private float[] rows;
  public float[] Rows
  {
    get {
      return rows;
    }
  }

  private Rect playArea;
  public Rect PlayArea
  {
    get {
      return playArea;
    }
  }

  private GameRows()
  {
    UpdateRows(new Rect(0, 0, 0, 0));
  }

  public float UpdateZoneIfKeyboardIsOpen()
  {
    //Rect area = TouchScreenKeyboard.area;

    Rect area = new Rect(
      0.0f, //iPhone Area: (x:0.00, y:426.00, width:1334.00, height:324.00)
      426.0f * (Camera.main.pixelWidth / 1334.0f),
      Camera.main.pixelWidth,
      Camera.main.pixelHeight * (324.0f / 750.0f)
    );

    if (area.xMin != area.xMax || area.yMin != area.yMax) {
      UpdateRows(area);
      return area.yMax - area.yMin;
    }
    return 0.0f;
  }

  public int GetCenterIndex()
  {
    Debug.Assert(rows.Length > 0, "Rows must not be empty");
    return rows.Length / 2;
  }

  public float GetYByIndex(int index)
  {
    Debug.Assert(index >= 0 && index < rows.Length, "Index is out of range");
    return rows[index];
  }

  public int ClampIndex(int index)
  {
    return Mathf.Clamp(index, 0, rows.Length - 1);
  }

  public bool IsTopIndex(int index)
  {
    return index == 0;
  }

  public bool IsBottomIndex(int index)
  {
    return index == rows.Length - 1;
  }

  public int GetRandomIndex(int butNotThisIndex)
  {
    int result = Random.Range(0, rows.Length);
    for (int i = 0; i < 100; ++i) {  // TODO: magic number
      if (result != butNotThisIndex) {
        return result;
      }
      result = Random.Range(0, rows.Length);
    }
    return result;
  }

  private void UpdateRows(Rect coveredArea)
  {
    UpdatePlayArea(coveredArea);

    int nRows = 3; // TODO: magic number
    float paddingFraction = 0.1f;
    float padding = playArea.height * paddingFraction;
    float stepSize = (playArea.height - padding * 2.0f) / nRows;

    rows = new float[nRows];
    for (int i = 0; i < nRows; ++i) {
      rows[i] = playArea.yMax - i * stepSize - stepSize * 0.5f - padding;
    }
  }

  private void UpdatePlayArea(Rect coveredArea)
  {
    Camera cam = Camera.main;

    Vector3 screenBL = cam.ViewportToWorldPoint(Vector3.zero);
    Vector3 screenTR = cam.ViewportToWorldPoint(Vector3.one);

    Vector3 coverBL = screenBL;
    Vector3 coverTR = cam.ViewportToWorldPoint(
                        new Vector3(1.0f, coveredArea.height / cam.pixelHeight, 0.0f)
                      );

    float coveredH = 0.0f;
    if (coverBL != coverTR) {
      coveredH = Mathf.Abs(coverBL.y - coverTR.y);
    }

    playArea = new Rect(
      screenBL.x, screenBL.y + coveredH,
      Mathf.Abs(screenTR.x - screenBL.x),
      Mathf.Abs(screenTR.y - screenBL.y) - coveredH
    );
  }
}
