using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public int score;
	GameObject[] bugs;
	bool victory;
	bool once = true;
	GameTimer timer;

	public GameObject screenFader;

	// Use this for initialization
	void Start () {
		bugs = gameObject.GetComponent<LevelSetup>().bugs;
		timer = GetComponent<GameTimer> ();
		timer.StartTimer ();
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log (score);
		if (score >= bugs.Length && once) {
			for (int i=0; i<bugs.Length; i++) {
				bugs[i].GetComponent<Bug>().victory();
			}
			GameObject.Find("ParticleEffect").GetComponent<Particles>().Play();
			timer.StopTimer ();
			StartCoroutine(waitAndEndMenu());
			FMOD_StudioSystem.instance.PlayOneShot ("event:/01_sfx/victory", transform.position);
			once = false;
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

	IEnumerator waitAndEndMenu() {
		yield return new WaitForSeconds(6f);
		Debug.Log ("next scene should load now");
		screenFader.GetComponent<ScreenFader> ().fadeOut();
		gameObject.GetComponent<LoadLevel> ().loadScene (3);
	}
}
