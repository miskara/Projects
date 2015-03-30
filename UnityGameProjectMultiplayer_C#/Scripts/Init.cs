using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour {

	public GameObject MCPrefab;
	
	void Start () {
		if (!GameObject.Find ("MusicController(Clone)")) {
			Instantiate (MCPrefab, transform.position, Quaternion.identity);
		}
		
	}

}
