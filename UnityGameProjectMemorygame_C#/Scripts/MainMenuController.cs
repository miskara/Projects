using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public int world;
	public int level;
	public int clvl;
	public GameObject[] worlds;
	public GameObject[] levels;
	public Sprite[] open;
	public Sprite[] close;


	void Start() {

		PlayerPrefs.SetInt ("World",PlayerPrefs.GetInt ("World",1));
		PlayerPrefs.SetInt ("Level",PlayerPrefs.GetInt ("Level",1));
		PlayerPrefs.SetInt ("CurrentLevel",PlayerPrefs.GetInt ("CurrentLevel",1));
		setWorlds ();
		setLevels ();
		world = PlayerPrefs.GetInt ("World");
		level = PlayerPrefs.GetInt ("Level");
		clvl = PlayerPrefs.GetInt ("CurrentLevel");

	}

	public void Update(){
		if (Input.GetKey (KeyCode.Escape)) {
			if(Application.loadedLevel != 0) Application.LoadLevel(0);
		}
		//	PlayerPrefs.SetInt ("Level", 1);
		//	GameController.currentLVL = PlayerPrefs.GetInt ("Level");
		//	PlayerPrefs.SetInt ("World", 1);
		//	PlayerPrefs.Save ();
		//}
	}



	public void setWorlds(){
		for (int j=0; j<worlds.Length; j++) {
			if (j < PlayerPrefs.GetInt ("World")) worlds [j].GetComponent<SpriteRenderer> ().sprite = open[j];
			else worlds [j].GetComponent<SpriteRenderer> ().sprite = close[j];
		}
	}

	public void setLevels(){
		int w = PlayerPrefs.GetInt ("CurrentWorld");
		for (int j=0; j<levels.Length; j++) {
			if(j+((w-1)*3)<PlayerPrefs.GetInt("Level")) levels[j].GetComponent<SpriteRenderer> ().sprite = open[j];
			else levels[j].GetComponent<SpriteRenderer> ().sprite = close[j];
		}
	}
	public void SelectWorld(int world) {
//		Debug.Log ("selected:"+world);
	//	Debug.Log ("Unlocked:" + PlayerPrefs.GetInt ("World"));
		if (world == 6) {
			Application.LoadLevel("FleakDances");
		} 
		else if(world == 7) Application.LoadLevel ("credits_ver");
		else {
			if (PlayerPrefs.GetInt ("World") < world) {
		
			} else {
				PlayerPrefs.SetInt ("CurrentWorld", world);
				PlayerPrefs.Save ();
				Application.LoadLevel ("MemoryMenuW" + world);
			}
		}
	}



	public void SelectLevel(int level) {

		//Debug.Log ("selected:"+level);
		//Debug.Log ("Unlocked:" + PlayerPrefs.GetInt ("Level"));
		if (PlayerPrefs.GetInt ("Level") < level) {
		}
		else {
		PlayerPrefs.SetInt ("CurrentLevel", level);
		PlayerPrefs.Save ();
		Application.LoadLevel ("Level"+level);
		}

	}



}

