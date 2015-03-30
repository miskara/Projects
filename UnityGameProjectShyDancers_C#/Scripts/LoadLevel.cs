using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	GameObject screenFader;
	float waitTime;
	int levelCount = 10;

	void Start(){
		screenFader = GameObject.Find ("ScreenFader");
		waitTime = screenFader.GetComponent<ScreenFader> ().fadeSpeed;
	}

	public void loadScene(string level)
	{
		Time.timeScale = 1f;
		screenFader.GetComponent<ScreenFader> ().fadeOut();
		StartCoroutine (waitAndLoad (level));
	}

	IEnumerator waitAndLoad(string level) {
		yield return new WaitForSeconds (waitTime);
		Application.LoadLevel (level);
	}

	IEnumerator waitAndLoadInt(int level) {
		yield return new WaitForSeconds (waitTime);
		Application.LoadLevel (level);
	}

	public void loadNextLevel (){
		Time.timeScale = 1f;
		screenFader.GetComponent<ScreenFader> ().fadeOut ();
		int level = PlayerPrefs.GetInt("LastLevel") + 1;
		if (PlayerPrefs.GetInt ("LastLevel") == 3) {
			StartCoroutine (waitAndLoad ("ShyBugTutorial02"));
		}
		else if (levelCount == PlayerPrefs.GetInt("LastLevel"))
			StartCoroutine (waitAndLoad ("ShyBugMenuLevelSelect"));
		else 
			StartCoroutine (waitAndLoadInt (level));
	}

	public void loadLastLevel (){
		Time.timeScale = 1f;
		screenFader.GetComponent<ScreenFader> ().fadeOut ();
		int level = PlayerPrefs.GetInt("LastLevel");
		StartCoroutine (waitAndLoadInt (level));
	}
}
