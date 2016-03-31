﻿using UnityEngine;

public struct Coords {
  public int x, y;

  public Coords(int x, int y) {
    this.x = x;
    this.y = y;
  }

  public void Right() { this.x++; }
  public void Left() { this.x--; }
  public void Up() { this.y++; }
  public void Down() { this.y--; }
}

public class Generator : MonoBehaviour {
  public int xSize;
  public int ySize;
  public float upProc;
  public float tunnelProc;
  public float turnProc;
  public GameObject tile;

  private int[,] level;
  private int xEdge;
  private int yEdge;
  private int mitad;
  private Coords pos;

  void GoUp() {
    level[pos.x, pos.y] = 3;
    pos.Up();

    while (Utils.Rand(tunnelProc) && pos.y != yEdge) {
      level[pos.x, pos.y] = 4;
      pos.Up();
    }

    level[pos.x, pos.y] = 2;
  }

  bool IsEdge() { return pos.x == 0 || pos.x == xEdge; }

  void Populate() {
    bool right;
    if (pos.x == 0) right = true;
    else if (pos.x == xEdge) right = false;
    else right = Utils.Rand(pos.x < mitad ? turnProc : 1 - turnProc);

    while (true) {
      if (right) pos.Right();
      else pos.Left();

      if (IsEdge() || Utils.Rand(upProc)) {
        GoUp();
        return;
      } else level[pos.x, pos.y] = 1;
    }
  }

  void GenerateLevel() {
    level = new int[xSize, ySize];

    bool top = Utils.Rand(0.5f);
    level[Random.Range(0, xSize), top ? yEdge : 0] = 8;
    level[Random.Range(0, xSize), top ? 0 : yEdge] = 9;

    int[] floor = Utils.GetRow(level, 0);
    pos = new Coords(System.Array.IndexOf(floor, top ? 9 : 8), 0);

    while (pos.y != yEdge) Populate();

    int[] ceiling = Utils.GetRow(level, yEdge);
    Coords dest = new Coords(System.Array.IndexOf(ceiling, top ? 8 : 9), yEdge);
    if (dest.x == -1) {
      int newSpot = pos.x;
      while (newSpot == pos.x)
        newSpot = Random.Range(0, xSize);

      dest.x = newSpot;
      level[dest.x, dest.y] = top ? 8 : 9;
    }

    while (pos.x != dest.x) {
      if (pos.x > dest.x) pos.Left();
      else pos.Right();
      if (pos.x != dest.x) level[pos.x, pos.y] = 1;
    }

    print(Utils.Array2DToString(level));
  }

	void Start() {
    xEdge = xSize - 1;
    yEdge = ySize - 1;
    mitad = xSize / 2;

    GenerateLevel();
  }
}
