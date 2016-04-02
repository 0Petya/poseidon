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

  public static char[,] JagToMultidem(char[][] array, int xSize, int ySize) {
    char[,] multidem = new char[xSize, ySize];
    for (int x = 0; x < xSize; x++)
      for (int y = 0; y < ySize; y++)
        multidem[x, y] = array[ySize - 1 - y][x];

    return multidem;
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

  public static Coords GetSpot(char[,] array) {
    while (true) {
      int x = Random.Range(0, array.GetLength(0));
      int y = Random.Range(0, 2);
      if (array[x, y] == '1' && array[x, y + 1] == '0') {
        return new Coords(x, y + 1);
      }
    }
  }
}
