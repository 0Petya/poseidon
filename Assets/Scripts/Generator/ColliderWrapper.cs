using UnityEngine;
using System.Collections.Generic;

public class ColliderWrapper : MonoBehaviour {
  public GameObject edgeCollider;

	private struct Flood {
		public int[,] array;
		public int chunks;
		
		public Flood(int[,] array, int chunks) {
			this.array = array;
			this.chunks = chunks;
		}
	}
	
	// Recursvely traverses the arr starting at pos, setting values to i as it goes.
	// Returns if already visited or at edges of the arr or the chunk as indicated by 0's.
	void Fill(int[,] arr, Coords pos, int i) {
		if (pos.x < 0 || pos.x >= arr.GetLength(0) || pos.y < 0 || pos.y >= arr.GetLength(1)) return;
		if (arr[pos.x, pos.y] != 1) return;
		arr[pos.x, pos.y] = i;
		
		Fill(arr, new Coords(pos.x + 1, pos.y), i);
		Fill(arr, new Coords(pos.x, pos.y + 1), i);
		Fill(arr, new Coords(pos.x, pos.y - 1), i);
		Fill(arr, new Coords(pos.x - 1, pos.y), i);
	}
	
	// Find a spot in the array that has a 1
	// (solid and not yet filled).
  // If can't find one, return the negative Coord.
	Coords FindChunk(int[,] colliders) {
		for (int y = 0; y < colliders.GetLength(1); y++)
			for (int x = 0; x < colliders.GetLength(0); x++)
				if (colliders[x,y] == 1) return new Coords(x, y);
				
		return new Coords(-1, -1);
	}
	
	// Uses Fill to mark the array with 2, 3, 4 and so on for
	// each chunk found with FindChunk.
	// Returns a Flood which is a struct to hold both the
	// filled chunks and the amount of them.
	Flood FloodFill(int[,] colliders) {
		int chunks = 0;
		Coords pos = FindChunk(colliders);
		while (pos.x != -1 && pos.y != -1) {
			chunks++;
			Fill(colliders, pos, chunks + 1);
			pos = FindChunk(colliders);
		}
		
		return new Flood(colliders, chunks);
	}
	
	// Return a 2D of 1's and 0's.
	// 1 represents a solid, 0 represents a lack thereof.
	int[,] GetSolids(List<GameObject> tiles, int xSize, int ySize) {
    int[,] colliders = new int[xSize, ySize];
    tiles.ForEach(x => colliders[(int) x.transform.position.x + 1, (int) x.transform.position.y + 1] = 1);

    for (int i = 0; i != xSize; i++) {
      colliders[i, 0] = 1;
      colliders[i, ySize - 1] = 1;
    }

    for (int i = 0; i != ySize; i++) {
      colliders[0, i] = 1;
      colliders[xSize - 1, i] = 1;
    }

    return colliders;
	}
	
	// Uses GetSolids to get all solid objects in the (xSize by ySize) scene,
	// marks them accordingly with FloodFill,
	// then instantiates the colliders given the chunks marked.
	public void WrapColliders(List<GameObject> tiles, int xSize, int ySize) {
		Flood flood = FloodFill(GetSolids(tiles, xSize + 2, ySize + 2));

    for (int i = 0; i <= flood.chunks; i++) {
      EdgeCollider2D collider = Instantiate(edgeCollider).GetComponent<EdgeCollider2D>();

      Stack<Pair<int, int>> points = new Stack<Pair<int, int>>();
      for (int y = 0; y < flood.array.GetLength(1); y++)
        for (int x = 0; x < flood.array.GetLength(0); x++)
          if (flood.array[x, y] == i + 1)
            points.Push(new Pair<int, int>(x, y));

      Vector2[] vecPoints = new Vector2[points.Count];
      for (int y = 0; y < vecPoints.Length; i++) {
        Pair<int, int> point = points.Pop();
        vecPoints[y] = new Vector2(point.fst, point.sec);
      }

      collider.points = vecPoints;
    }
  }
}
