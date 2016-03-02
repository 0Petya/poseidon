using UnityEngine;
using System.Collections;
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
		if (controls.GetButtonDown("Shoot") && !loadLock && !mapper.isOpen)
			LoadScene();
	}

	void LoadScene() {
		loadLock = true;
		SceneManager.LoadScene(scene);
	}
}
