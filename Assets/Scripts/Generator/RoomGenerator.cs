using UnityEngine;
using System.Linq;

enum Type {
  None,
  Start,
  Exit
};

public class RoomGenerator : MonoBehaviour {
  public int xSize;
  public int ySize;
  public int maxRooms;
  public GameObject player;

  private GameObject basicTile;
  private GameObject background;
  private GameObject bedrock;
  private GameObject door;
  private GameObject exitSign;

  void Awake() {
    basicTile = Resources.Load("Prefabs/Level/BasicTile") as GameObject;
    background = Resources.Load("Prefabs/Level/Background") as GameObject;
    bedrock = Resources.Load("Prefabs/Level/BasicTile") as GameObject;
    door = Resources.Load("Prefabs/Level/Door") as GameObject;
    exitSign = Resources.Load("Prefabs/Level/ExitSign") as GameObject;
  }

  void GenerateBedrock(int xLevel, int yLevel) {
    for (float i = -1; i <= xLevel * xSize; i++) {
      Instantiate(bedrock, new Vector3(i, -1, 0), Quaternion.identity);
      Instantiate(bedrock, new Vector3(i, yLevel * ySize, 0), Quaternion.identity);
    }

    for (float j = 0; j <= yLevel * ySize - 1; j++) {
      Instantiate(bedrock, new Vector3(-1, j, 0), Quaternion.identity);
      Instantiate(bedrock, new Vector3(xLevel * xSize, j, 0), Quaternion.identity);
    }
  }

  void GenerateBackground(int xLevel, int yLevel) {
    for (float i = 0; i < xLevel * xSize; i++)
      for (float j = 0; j < yLevel * ySize; j++)
        Instantiate(background, new Vector3(i, j, 0), Quaternion.identity);
  }

  void InstantiateTile(char tile, int posX, int posY) {
    if (tile == '0') return;
    if (tile == '1') Instantiate(basicTile, new Vector3(posX, posY), Quaternion.identity);
  }

  void InstantiateRoom(char[,] room, int posX, int posY, Type type) {
    float cenX = posX - 0.5f + 1;
    float cenY = posY - 0.5f + 1;

    for (int y = 0; y < ySize; y++)
      for (int x = 0; x < xSize; x++)
        InstantiateTile(room[x, y], x + posX, y + posY);

    if (type == Type.Start) {
      Coords spot = Utils.GetSpot(room);
      Instantiate(player, new Vector3(spot.x + cenX, spot.y + cenY), Quaternion.identity);
      Instantiate(door, new Vector3(spot.x + cenX, spot.y + cenY), Quaternion.identity);
    } else if (type == Type.Exit) {
      Coords spot = Utils.GetSpot(room);
      Instantiate(door, new Vector3(spot.x + cenX, spot.y + cenY), Quaternion.identity);
      Instantiate(exitSign, new Vector3(spot.x + cenX, spot.y + cenY - 1), Quaternion.identity);
    }
  }

  char[,] GetRoom(string name) {
    TextAsset data = null;
    while(!data) data = Resources.Load("Rooms/" + name + "P" + Random.Range(0, maxRooms)) as TextAsset;

    string[] rows = data.text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
    if (Utils.Rand(0.5f))
      rows = rows.Select(x => {
        char[] tmp = x.ToCharArray();
        System.Array.Reverse(tmp);
        return new string(tmp);
      }).ToArray();

    char[][] room = rows.Select(x => x.ToCharArray()).ToArray();

    return Utils.JagToMultidem(room, xSize, ySize);
  }

  public void GenerateRooms(int[,] level) {
    string area = "A1/";
    for (int y = 0; y < level.GetLength(1); y++)
      for (int x = 0; x < level.GetLength(0); x++) {
        string room = area;
        Type type = Type.None;

        if (level[x, y] == 8) {
          room += 1;
          type = Type.Start;
        } else if (level[x, y] == 9) {
          room += 1;
          type = Type.Exit;
        } else room += level[x, y];

        InstantiateRoom(GetRoom(room), x * xSize, y * ySize, type);
      }

    GenerateBackground(level.GetLength(0), level.GetLength(1));
    GenerateBedrock(level.GetLength(0), level.GetLength(1));
  }
}
