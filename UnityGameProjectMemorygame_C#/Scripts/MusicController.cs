using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	FMOD.Studio.EventInstance menuMusic;
	FMOD.Studio.EventInstance fleakMusic;
	FMOD.Studio.EventInstance morphyMusic;
	FMOD.Studio.EventInstance joejoeMusic;

	FMOD.Studio.PLAYBACK_STATE  menuMusicPlaybackState;
	FMOD.Studio.PLAYBACK_STATE  fleakMusicPlaybackState;
	FMOD.Studio.PLAYBACK_STATE  morphyMusicPlaybackState;
	FMOD.Studio.PLAYBACK_STATE  joejoeMusicPlaybackState;

	public static MusicController Instance;

	void Awake() {

		if (Instance == null)
		{
			Instance = this;
		}
		DontDestroyOnLoad(this.gameObject);

		menuMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_menu");
		fleakMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_fleak");
		morphyMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_morphy");
		joejoeMusic = FMOD_StudioSystem.instance.GetEvent("event:/02_music/music_joejoe");

		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		fleakMusic.getPlaybackState (out fleakMusicPlaybackState);
		morphyMusic.getPlaybackState (out morphyMusicPlaybackState);
		joejoeMusic.getPlaybackState (out joejoeMusicPlaybackState);
		
		menuMusic.start ();
	}

	void Start () {
	}

	public void PlayMenuMusic(){
		//Debug.Log ("Play menu music");
		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		if (menuMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			menuMusic.start ();
		}
	}

	public void StopMenuMusic(){
		//Debug.Log ("Stop menu music");
		//menuMusic.stop (FMOD.Studio.STOP_MODE.IMMEDIATE);
		menuMusic.stop (FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	public void PlayFleakMusic(){
		//Debug.Log ("Play game music");
		fleakMusic.getPlaybackState (out fleakMusicPlaybackState);
		if (fleakMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			fleakMusic.start ();
		}
	}

	public void PlayMorphyMusic(){
				//Debug.Log ("Play game music");
		morphyMusic.getPlaybackState (out morphyMusicPlaybackState);
		if (morphyMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			morphyMusic.start ();
		}
	}

	public void PlayJoejoeMusic(){
				//Debug.Log ("Play game music");
		joejoeMusic.getPlaybackState (out joejoeMusicPlaybackState);
		if (joejoeMusicPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			joejoeMusic.start ();
		}
	}
	
	public void StopGameMusic(){

		menuMusic.getPlaybackState (out menuMusicPlaybackState);
		fleakMusic.getPlaybackState (out fleakMusicPlaybackState);
		morphyMusic.getPlaybackState (out morphyMusicPlaybackState);
		joejoeMusic.getPlaybackState (out joejoeMusicPlaybackState);
		//Debug.Log ("Stop game music");
		if (fleakMusicPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			fleakMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			//fleakMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (morphyMusicPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			morphyMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			//morphyMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
		if (joejoeMusicPlaybackState == FMOD.Studio.PLAYBACK_STATE.PLAYING) {
			joejoeMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			//joejoeMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		}
	}

	void OnLevelWasLoaded () {

		string currentLevel = Application.loadedLevelName;

		if (currentLevel == "MemoryMenuMain"){
			StopGameMusic();
			PlayMenuMusic();
		}
		else if (currentLevel == "Level1" || currentLevel == "Level2" || currentLevel == "Level3"){
			StopGameMusic();
			PlayFleakMusic();
			StopMenuMusic();
		}
		else if (currentLevel == "Level4" || currentLevel == "Level5" || currentLevel == "Level6"){
			StopGameMusic();
			PlayJoejoeMusic();
			StopMenuMusic();
		}
		else if (currentLevel == "Level7" || currentLevel == "Level8" || currentLevel == "Level9"|| currentLevel == "Level10"){
			StopGameMusic();
			PlayMorphyMusic();
			StopMenuMusic();
		}
	}
}
