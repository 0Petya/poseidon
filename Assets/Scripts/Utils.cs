using UnityEngine;

public static class Utils {
  public static string Array2DToString<T>(T[,] array) {
    string str = "";
    for (int i = array.GetLength(0) - 1; i >= 0; i--) {
      for (int j = 0; j < array.GetLength(1); j++)
        str += " " + array[j, i];

      str += "\n";
    }

    return str;
  }

  public static string ArrayToString<T>(T[] array) {
    string str = "";
    foreach (T thing in array)
      str += " " + thing;

    return str;
  }

  public static bool Rand(float percentage) {
    return Random.Range(0f, 1f) <= percentage;
  }

  public static int[] GetRow(int[,] array, int rowID) {
    int[] row = new int[array.GetLength(1)];
    for (int x = 0; x < array.GetLength(0); x++)
      row[x] = array[x, rowID];

    return row;
  }
}
