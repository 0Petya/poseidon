using UnityEngine;
using System.Collections;

public class BackgroundGenerator : MonoBehaviour {
  public GameObject background;

	void Start() {
    for (float i = -20f; i < 21f; i++)
      for (float j = -10f; j < 11f; j++)
        Instantiate(background, new Vector3(i, j, 0), Quaternion.identity);
	}
}
