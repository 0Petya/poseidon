using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;
using Rewired.UI.ControlMapper;

public class MainMenu : MonoBehaviour {
	public string scene;

	private Player controls;
	private ControlMapper mapper;
	private bool loadLock;

	void Start() {
		controls = ReInput.players.GetPlayer(0);
		mapper = GameObject.Find("ControlMapper").GetComponent<ControlMapper>();
	}

	void Update() {
    print(Manager.levelSize);
    if (!loadLock && !mapper.isOpen) {
      if (controls.GetButtonDown("2")) Manager.levelSize = 2;
      if (controls.GetButtonDown("3")) Manager.levelSize = 3;
      if (controls.GetButtonDown("4")) Manager.levelSize = 4;
      if (controls.GetButtonDown("5")) Manager.levelSize = 5;
      if (controls.GetButtonDown("6")) Manager.levelSize = 6;
      if (controls.GetButtonDown("7")) Manager.levelSize = 7;
      if (controls.GetButtonDown("8")) Manager.levelSize = 8;
      if (controls.GetButtonDown("9")) Manager.levelSize = 9;
      if (Manager.levelSize > 0) LoadScene();
    }

    if (controls.GetButtonDown("Pause"))
      mapper.Open();
	}

	void LoadScene() {
		loadLock = true;
		SceneManager.LoadScene(scene);
	}
}
