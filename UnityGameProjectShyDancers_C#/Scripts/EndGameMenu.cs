using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour {

	public GameObject levelTimeText;
	string levelTime;

	void Start () {
		levelTime = PlayerPrefs.GetString ("LevelTime");
		levelTimeText.GetComponent<Text> ().text = levelTime;
	}
}
