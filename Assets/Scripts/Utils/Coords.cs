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
