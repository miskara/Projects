using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class Progress : MonoBehaviour {

	GameObject [] buttons;
	int unlocked;

	public bool unlockAllLevels = false;

	void Awake () {
		if (!PlayerPrefs.HasKey ("Levels")) {
			PlayerPrefs.SetInt("Levels", 1);
		}
	}

	void Start () {
		if (Application.loadedLevelName == "ShyBugMenuLevelSelect") {
			unlocked = PlayerPrefs.GetInt ("Levels");

			buttons = GameObject.FindGameObjectsWithTag("LevelButton").OrderBy( go => go.name ).ToArray();

			if (unlockAllLevels){
				unlocked = buttons.Length;
			}

			for (int i=0; i<buttons.Length; i++) {
				lockButtons(i);
			}

			//Debug.Log (unlocked + " levels unlocked");
			if (unlocked > buttons.Length)
				unlocked = buttons.Length;
			for (int i=0; i<unlocked; i++) {
				unlockButtons(i);
			}
		}
	}

	void unlockButtons (int button) {
		buttons [button].GetComponent<Image> ().color = new Vector4 (1f, 1f, 1f, 1f);
		buttons [button].GetComponent<Button> ().enabled = true;
		//buttons [button].GetComponent<Wobbler> ().enabled = true;
	}

	void lockButtons (int button) {
		buttons [button].GetComponent<Image> ().color = new Vector4 (1f, 1f, 1f, 0.4f);
		buttons [button].GetComponent<Button> ().enabled = false;
		//buttons [button].GetComponent<Wobbler> ().enabled = false;
	}
}