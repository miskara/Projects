using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour {

	public GameObject MusicControllerPrefab;
	
	void Start () {
		if (!GameObject.Find ("MusicController(Clone)")) {
			Instantiate (MusicControllerPrefab, transform.position, Quaternion.identity);
		}
		
	}

}
