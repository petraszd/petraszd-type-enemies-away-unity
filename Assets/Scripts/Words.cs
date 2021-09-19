using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Words
{
  private Words()
  {
  }

  private const int SHORTEST_LEN = 1;
  private const int LONGEST_LEN = 24;
  private static Dictionary<int, string[]> allWords;

  static Words()
  {
    allWords = new Dictionary<int, string[]>();

    TextAsset res;
    for (int i = SHORTEST_LEN; i <= LONGEST_LEN; ++i) {
      res = Resources.Load(string.Format("Words/{0}", i)) as TextAsset;
      allWords.Add(i, res.text.Split('\n'));
    }
  }

  public static int ClampSize(int size)
  {
    return Mathf.Clamp(size, SHORTEST_LEN, LONGEST_LEN);
  }

  public static string GetRandom(int size)
  {
    string [] words = allWords[size];
    return words[Random.Range(0, words.Length)];
  }
}
