using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	bool isPaused = false;

	void Start() {

	}

	public void togglePauseMenu() {
		if (isPaused) {
			Time.timeScale = 1.0f;
			isPaused = false;
			gameObject.SetActive(false);
		}

		else {
			Time.timeScale = 0.0f;
			isPaused = true;
			gameObject.SetActive(true);
		}
	}
}
