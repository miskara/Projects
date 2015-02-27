using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	FMOD.Studio.EventInstance menuMusic;
	FMOD.Studio.EventInstance gameMusic;
	
	void Start () {

		menuMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_menu");
		gameMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_game");
	}
	
	public void PlayMenuMusic(){
		menuMusic.start();
	}

	public void StopMenuMusic(){
		menuMusic.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		menuMusic.release();
	}

	public void PlayGameMusic(){
		gameMusic.start();
	}
	
	public void StopGameMusic(){
		gameMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		gameMusic.release();
	}

	void OnLevelWasLoaded(int level) {
		if (level == 2){
			PlayGameMusic();
			StopMenuMusic();
		}
		else if (level == 0){
			PlayMenuMusic();
			StopGameMusic();
		}
		else if (level == 1){
			StopMenuMusic();
			StopGameMusic();
		}
	}

	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
