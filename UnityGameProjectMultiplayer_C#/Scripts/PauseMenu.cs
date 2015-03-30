using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {


	public GameObject home;
	public GameObject mute;
	public GameObject play;
	public GameController gc;
	Ray ray;
	RaycastHit hit;

	public GameObject sounds;
	
	FMOD.Studio.Bus masterBus;
	
	public Sprite spriteSoundOff;
	public Sprite spriteSoundOn;

	// Use this for initialization
	void Start () {
		//gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		FMOD.Studio.System system = FMOD_StudioSystem.instance.System;
		system.getBus("bus:/", out masterBus);

		if (!PlayerPrefs.HasKey("muted")){
			PlayerPrefs.SetInt ("muted", 0);
		}
		
		if (PlayerPrefs.GetInt ("muted") == 1) {
			masterBus.setMute (true);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOff;
		} else if (PlayerPrefs.GetInt ("muted") == 0) {
				masterBus.setMute (false);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOn;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (InputController.HasTouchBegan ()) {
			ray = Camera.main.ScreenPointToRay (InputController.GetTouchPosition ());
			if (Physics.Raycast (ray, out hit)) {
				if (hit.collider.name.Equals (play.name)) {
					gc.Pause ();
				}
				else if(hit.collider.name.Equals (mute.name)){
					Mute();
				}
				else if(hit.collider.name.Equals (home.name)){
					Application.LoadLevel (0);
					Time.timeScale=1.0f;
				}
				                                           
			}
		}

	}


	public void Mute () {
		if (PlayerPrefs.GetInt ("muted") == 1) { // UNMUTE
			masterBus.setMute (false);
			PlayerPrefs.SetInt ("muted", 0);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOn;
		} 
		else { // MUTE
			masterBus.setMute (true);
			PlayerPrefs.SetInt ("muted", 1);
			sounds.GetComponent<SpriteRenderer>().sprite = spriteSoundOff;
		}
	}

}
