using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameObject[] worlds;
	public GameObject[] levels;
	void Awake() {
		if (PlayerPrefs.GetInt ("World") < 2) {
			PlayerPrefs.SetInt ("Wolrd",1);
		}
		if (PlayerPrefs.GetInt ("Level") < 2) {
			PlayerPrefs.SetInt("Level",1);
		}

		for (int j=0; j<worlds.Length; j++) {
			if(j<PlayerPrefs.GetInt("World")) worlds[j].GetComponent<Text>().color = new Color32(130,36,207,255);
			else worlds[j].GetComponent<Text>().color = new Color32(130,36,207,45);
		}


		for (int j=0; j<levels.Length; j++) {
			if(j<PlayerPrefs.GetInt("Level")) levels[j].GetComponent<Text>().color = new Color32(130,36,207,255);
			else levels[j].GetComponent<Text>().color = new Color32(130,36,207,45);

		}
	}

	void Start () {

	}

	void Update () {}

	public void SelectWorld(int world) {
		PlayerPrefs.SetInt ("World", world);
		PlayerPrefs.Save ();
		Application.LoadLevel (1);
	}



	public void SelectLevel(int level) {
		if (PlayerPrefs.GetInt ("Level") < level) {
		}
		else {
		PlayerPrefs.SetInt ("CurrentLevel", level);
		PlayerPrefs.Save ();
		Application.LoadLevel ("Level"+level);
		}

	}



}

