using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	GameObject screenFader;
	float waitTime;

	void Start(){
		screenFader = GameObject.Find ("ScreenFader");
		waitTime = screenFader.GetComponent<ScreenFader> ().fadeSpeed;
	}

	public void loadScene(int level)
	{
		Time.timeScale = 1f;
		screenFader.GetComponent<ScreenFader> ().fadeOut();
		StartCoroutine (waitAndLoad (level));
	}

	IEnumerator waitAndLoad(int level) {
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel(level);
	}

//	void Awake() {
//		DontDestroyOnLoad(transform.gameObject);
//	}
}
