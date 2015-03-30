using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public int score;
	GameObject[] bugs;
	GameObject[] hides;
	bool victory;
	bool once = true;
	bool update = true;
	bool exitable = false;
	GameTimer timer;

	GameObject victoryParticles;

	public GameObject screenFader;

	void Start () {
		bugs = gameObject.GetComponent<LevelSetup>().bugs;
		hides = gameObject.GetComponent<LevelSetup>().hides;
		timer = GetComponent<GameTimer> ();
		timer.StartTimer ();
		if (Application.loadedLevelName == "ShyBugMenuEnd") {
			update = false;
		}
		screenFader = GameObject.Find ("ScreenFader");
		victoryParticles = GameObject.Find ("prtclVictory");
	}

	void Update () {
		if (update) {
			if (score >= bugs.Length && once) {
				for (int i=0; i<bugs.Length; i++) {
					bugs [i].GetComponent<Bug> ().victory ();
				}

				for (int i=0; i<hides.Length; i++) {
					hides [i].GetComponent<Hide> ().fadeOut ();
				}

				if (victoryParticles != null) {
					victoryParticles.GetComponent<ParticleSystem>().Play();
				}
				else Debug.Log ("No prtclVictory found");

				StartCoroutine(slowMotion(1.5f));
				
				timer.StopTimer ();
				StartCoroutine (waitAndEnableExit ());
				StartCoroutine (exitTimer ());
				FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/victory", transform.position);
				once = false;
			}
		}
	}

	public void add () {
		score += 1;
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/ui_found", transform.position);
	}

	public void substract () {
		score -= 1;
		FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/ui_lost", transform.position);
	}

	public void exit () {
		if (exitable) {
			screenFader.GetComponent<ScreenFader> ().fadeOut();
			unlock ();
			PlayerPrefs.SetInt ("LastLevel", Application.loadedLevel);
			gameObject.GetComponent<LoadLevel> ().loadScene ("ShyBugMenuEnd");
		}
	}

	IEnumerator waitAndEnableExit() {
		yield return new WaitForSeconds(2f);
		exitable = true;
	}
	
	void unlock () {
		int unlocked = PlayerPrefs.GetInt ("Levels");
		if (Application.loadedLevel == unlocked) {
			PlayerPrefs.SetInt ("Levels", unlocked + 1);
		}
	}

	IEnumerator exitTimer () {
		yield return new WaitForSeconds(10f);
		screenFader.GetComponent<ScreenFader> ().fadeOut();
		PlayerPrefs.SetInt ("LastLevel", Application.loadedLevel);
		gameObject.GetComponent<LoadLevel> ().loadScene ("ShyBugMenuEnd");
	}

	IEnumerator slowMotion(float duration) {
		Time.timeScale = 0.5f;
		yield return new WaitForSeconds(duration);
		Time.timeScale = 1.0f;
	}

}
