using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameObject[] worlds;
	public GameObject[] levels;
	void Awake() {

		for (int i = 1; i<4; i++) {
			Image image = levels[i-1].GetComponent<Image>();
			image.color = new Color (image.color.r, image.color.g, image.color.b, 0.176470f);
		}
		for (int i=0; i<1; i++) {
			Image image = levels[i].GetComponent<Image>();
			levels[i].GetComponent<Image>().color = new Color(image.color.r,image.color.g,image.color.b,1f);
		}
	}

	void Start () {




	}

	void Update () {}

	public void SelectWorld(int world) {
		PlayerPrefs.SetInt ("Difficulty", world);
		PlayerPrefs.Save ();
		Application.LoadLevel (1);
	}



	public void SelectLevel(int level) {
		//if (PlayerPrefs.GetInt ("Level") < level) {
		//}
		//else {
		//PlayerPrefs.SetInt ("CurrentLevel", level);
		//PlayerPrefs.Save ();
		Application.LoadLevel (level);
	}



}

