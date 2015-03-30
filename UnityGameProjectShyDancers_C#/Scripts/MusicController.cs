using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	FMOD.Studio.EventInstance menuMusic;
	public FMOD.Studio.EventInstance gameMusic;

	FMOD.Studio.PLAYBACK_STATE  menuMusicPlaybackState;
	FMOD.Studio.PLAYBACK_STATE  gameMusicPlaybackState;

	public static MusicController Instance;

	void Awake() {

		if (Instance == null)
		{
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);

		menuMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_menu");
		gameMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_game");

		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		gameMusic.getPlaybackState (out gameMusicPlaybackState);

		//Debug.Log ("Starting menu music");
		menuMusic.start ();
	}

	void Start () {
	}

	public void PlayMenuMusic(){
		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		if (menuMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			menuMusic.start ();
		}
	}

	public void StopMenuMusic(){
		menuMusic.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	public void PlayGameMusic(){
		gameMusic.getPlaybackState (out gameMusicPlaybackState);
		if (gameMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			gameMusic.start ();
		}
	}
	
	public void StopGameMusic(){
		gameMusic.getPlaybackState (out gameMusicPlaybackState);
		if (gameMusicPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			gameMusic.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}
	void OnLevelWasLoaded () {
		string currentLevel = Application.loadedLevelName;
		if (currentLevel == "ShyBugMenuMain" || currentLevel == "Credits" || currentLevel == "ShyBugMenuLevelSelect"){
			StopGameMusic();
			PlayMenuMusic();
		}

		else {
			StopMenuMusic();
			PlayGameMusic();
		}
	}
}
