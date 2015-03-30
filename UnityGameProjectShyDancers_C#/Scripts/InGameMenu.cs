using UnityEngine;
using System.Collections;

public class InGameMenu : MonoBehaviour {

	bool isPaused = false;
	InputBlocker inputBlocker;
	bool IBFound = false;

	void Start() {
		inputBlocker = GameObject.Find ("InputBlocker").GetComponent<InputBlocker> ();
		if (inputBlocker != null)
			IBFound = true;
	}

	public void togglePauseMenu() {
		if (isPaused) {
			Time.timeScale = 1.0f;
			isPaused = false;
			gameObject.SetActive(false);
			if (IBFound)
				inputBlocker.blockOff();
		}

		else {
			Time.timeScale = 0.0f;
			isPaused = true;
			gameObject.SetActive(true);
			if (IBFound)
				inputBlocker.blockOn();
		}
	}
}
