using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {
  void OnTriggerEnter2D(Collider2D other) {
    if (other.gameObject.CompareTag("Player")) {
      Manager.levelSize = -1;
      SceneManager.LoadScene("MainMenu");
    }
  }
}
