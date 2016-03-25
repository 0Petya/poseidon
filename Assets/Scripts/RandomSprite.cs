using UnityEngine;
using System.Collections;

public class RandomSprite : MonoBehaviour {
  public Sprite[] sprites;

	void Start() {
    GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
  }
}
