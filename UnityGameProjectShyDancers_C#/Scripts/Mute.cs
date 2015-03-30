using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mute : MonoBehaviour {

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
			gameObject.GetComponent<Image>().sprite = spriteSoundOff;
		} else if (PlayerPrefs.GetInt ("muted") == 0) {
			masterBus.setMute (false);
			gameObject.GetComponent<Image>().sprite = spriteSoundOn;
		}
	}

	public void mute () {
		if (PlayerPrefs.GetInt ("muted") == 1) { // UNMUTE
			masterBus.setMute (false);
			PlayerPrefs.SetInt ("muted", 0);
			gameObject.GetComponent<Image>().sprite = spriteSoundOn;
		} 
		else { // MUTE
			masterBus.setMute (true);
			PlayerPrefs.SetInt ("muted", 1);
			gameObject.GetComponent<Image>().sprite = spriteSoundOff;
		}
	}		
}
