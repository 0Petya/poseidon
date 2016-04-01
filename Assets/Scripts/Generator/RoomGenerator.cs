using UnityEngine;
using System.Linq;

public class RoomGenerator : MonoBehaviour {
  public int xSize;
  public int ySize;

  int[,] GetRoom(string name) {
    TextAsset data = Resources.Load("Rooms/" + name + "P") as TextAsset;

    string[] rows = data.text.Split('\n');
    int[][] room = rows.Select(x => x.ToCharArray().Select(y => y - '0').ToArray()).ToArray();

    return Utils.JagToMultidem(room, xSize, ySize);
  }

  public void GenerateRooms(int[,] level) {
    int[,] room = GetRoom("A1R1");
    print(Utils.Array2DToString(room));
  }
}
