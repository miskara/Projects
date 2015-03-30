using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	RaycastHit hit;
	Ray ray;
	public enum GameDifficulty {Easy, Medium, Hard}
	public static int unlockedLVLS = 1;
	public static int currentLVL=1;
	public static GameDifficulty diff = GameDifficulty.Easy;



	void Awake(){
		DontDestroyOnLoad (this);
	
	}
	void Start () {
		unlockedLVLS = PlayerPrefs.GetInt ("levels");
	}

}