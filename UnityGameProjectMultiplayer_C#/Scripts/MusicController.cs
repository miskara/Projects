using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	FMOD.Studio.EventInstance menuMusic;
	FMOD.Studio.EventInstance gameMusic1;
	FMOD.Studio.EventInstance gameMusic2;
	FMOD.Studio.EventInstance gameMusic3;

	FMOD.Studio.PLAYBACK_STATE  menuMusicPlaybackState;
	FMOD.Studio.PLAYBACK_STATE  gameMusic1PlaybackState;
	FMOD.Studio.PLAYBACK_STATE  gameMusic2PlaybackState;
	FMOD.Studio.PLAYBACK_STATE  gameMusic3PlaybackState;
	float volume;

	public static MusicController Instance;

	void Awake() {

		if (Instance == null)
		{
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);

		menuMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_menu");
		gameMusic1 = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_level1");
		gameMusic2 = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_level2");
		gameMusic3 = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_level3");

		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		gameMusic1.getPlaybackState (out gameMusic1PlaybackState);
		gameMusic2.getPlaybackState (out gameMusic2PlaybackState);
		gameMusic3.getPlaybackState (out gameMusic3PlaybackState);

		Debug.Log ("Starting menu music");
		menuMusic.start ();
	}

	void Start () {
	}

	public void PlayMenuMusic(){
		Debug.Log ("Play menu music");
		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		if (menuMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			menuMusic.start ();
		}
	}

	public void StopMenuMusic(){
		Debug.Log ("Stop menu music");
		menuMusic.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	public void PlayGameMusic(){
		gameMusic1.getPlaybackState (out gameMusic1PlaybackState);
		if (gameMusic1PlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			Debug.Log ("Play game music");
			gameMusic1.start ();
		}
	}

	public void PlayGameMusic2(){
		gameMusic2.getPlaybackState (out gameMusic2PlaybackState);
		if (gameMusic2PlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			Debug.Log ("Play game music 2");
			gameMusic2.start ();
		}
	}

	public void PlaygameMusic3(){
		gameMusic3.getPlaybackState (out gameMusic3PlaybackState);
		if (gameMusic3PlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			Debug.Log ("Play game music 3");
			gameMusic3.start ();
		}
	}

	public void StopGameMusic1(){
		gameMusic1.getPlaybackState (out gameMusic1PlaybackState);
		if (gameMusic1PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			Debug.Log ("Stop game music1");
			gameMusic1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	public void StopGameMusic2(){
		gameMusic2.getPlaybackState (out gameMusic2PlaybackState);
		if (gameMusic2PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			Debug.Log ("Stop game music2");
			gameMusic2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

	}
	public void StopgameMusic3(){
		gameMusic3.getPlaybackState (out gameMusic3PlaybackState);
		if (gameMusic3PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			Debug.Log ("Stop game music3");
			gameMusic3.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}

	}
	public void StopGameMusic(){
		Debug.Log ("Stop game music");
		gameMusic1.getPlaybackState (out gameMusic1PlaybackState);
		gameMusic2.getPlaybackState (out gameMusic2PlaybackState);
		gameMusic3.getPlaybackState (out gameMusic3PlaybackState);

		if (gameMusic1PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			gameMusic1.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (gameMusic2PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			gameMusic2.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (gameMusic3PlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			gameMusic3.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
	}

	void OnLevelWasLoaded () {
		int currentLevel = Application.loadedLevel;

		if (currentLevel == 0){
			StopMenuMusic();
			StopGameMusic1();
			StopGameMusic2();
			StopgameMusic3();
			PlayMenuMusic();
		}

		else if (currentLevel == 1){
			StopGameMusic2();
			StopgameMusic3();
			StopMenuMusic();
			PlayGameMusic();
		}

		else if (currentLevel == 2){
			StopGameMusic1();
			StopgameMusic3();
			StopMenuMusic();
			PlayGameMusic2();

		}

		else if (currentLevel == 3){
			StopGameMusic1();
			StopGameMusic2();
			StopMenuMusic();
			PlaygameMusic3();

		}
	}
}
