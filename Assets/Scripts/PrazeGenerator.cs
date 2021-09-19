using UnityEngine;

class PrazeGenerator
{
  private const string letters = "abcdefghijklmnopqrstuvwxyz";

  private PrazeGenerator()
  {
  }

  public static string Generate(string cantStartWith, int size)
  {
    size = Words.ClampSize(size);

    string result;
    char first;

    for (int i = 0; i < 1000; ++i) {
      result = Words.GetRandom(size);
      first = result[0];
      if (cantStartWith.IndexOf(first) == -1) {
        return result;
      }
    }

    // Tried 1000 time to return different letter. I do not want infinitive
    // loop some I am retuning random and hoping for the best :)
    return Words.GetRandom(size);
  }
}
