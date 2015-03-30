using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameObject[] worlds;
	public GameObject[] levels;
	public GameObject sounds;
	FMOD.Studio.Bus masterBus;
	
	public Sprite spriteSoundOff;
	public Sprite spriteSoundOn;


	void Start () {

		FMOD.Studio.System system = FMOD_StudioSystem.instance.System;
		system.getBus("bus:/", out masterBus);
		
		if (!PlayerPrefs.HasKey("muted")){
			PlayerPrefs.SetInt ("muted", 0);
		}
		
		if (PlayerPrefs.GetInt ("muted") == 1) {
			masterBus.setMute (true);
			sounds.GetComponent<Image>().sprite=spriteSoundOff;
		} else if (PlayerPrefs.GetInt ("muted") == 0) {
			masterBus.setMute (false);
			sounds.GetComponent<Image>().sprite=spriteSoundOn;
		}


	}


	public void Mute () {
		if (PlayerPrefs.GetInt ("muted") == 1) { // UNMUTE
			masterBus.setMute (false);
			PlayerPrefs.SetInt ("muted", 0);
			sounds.GetComponent<Image>().sprite=spriteSoundOn;
		} 
		else { // MUTE
			masterBus.setMute (true);
			PlayerPrefs.SetInt ("muted", 1);
			sounds.GetComponent<Image>().sprite=spriteSoundOff;
		}
	}

	void Update () {}

	public void SelectWorld(int world) {
		Application.LoadLevel (world);
	}

	public void Credits (){
	
		Application.LoadLevel (4);
	}

	public void SelectLevel(int level) {
		Application.LoadLevel (level);
	}



}

