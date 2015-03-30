using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GiantbugFrtSpawns : FruitSpawns {

	
	//public GameObject burpy;
	//public GameObject gb;
	public List<Transform> spawns = new List<Transform> ();
	//private PoolingSystem poolingSystem;
	//public GameObject spot;
	//public GameObject coconut;
	//public GameObject pinkly;
	//public GameObject nect;
	//public GameObject clone;
	//public List<GameObject> fruits = new List<GameObject>();
	//public Renderer burpyrenderer;
	//public int full;
	//float value;
	//public bool isfull;
	Vector3 gbspotIncr,gbcocoIncr,gbpinklyIncr,gbnectIncr,gbnul;
	//int count;
	// Use this for initialization
	void Start () {
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject))) {
			if (go.name == "S1") spawns.Add (go.transform);
		}
		full = 0;
		burpy = GameObject.Find ("Burpy");
		burpyrenderer = burpy.GetComponentInChildren<Renderer> ();
		poolingSystem = PoolingSystem.Instance;
		count = spawns.Count;
		gbspotIncr = new Vector3 (0.25f, 0.25f, 0.25f);
		gbnectIncr = new Vector3 (0.375f, 0.375f,0.375f);
		gbpinklyIncr = new Vector3 (0.5f, 0.5f,0.5f);
		gbcocoIncr = new Vector3 (0.75f, 0.75f, 0.75f);
		gbnul = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	void Awake(){
		full = 0;
		burpy = GameObject.Find ("Burpy");
		burpyrenderer = burpy.GetComponentInChildren<Renderer> ();
		poolingSystem = PoolingSystem.Instance;
		count = spawns.Count;
	}
	
   new public void removeFruit(GameObject go){
		fruits.Remove (go);
		PoolingSystem.DestroyAPS (go);
	}



	//public void DoScale(GameObject go,Vector3 start, Vector3 end, float totalTime) {
	//	StartCoroutine(CR_DoScale(go,start, end, totalTime));
	//}

	IEnumerator CR_DoScale(GameObject go,Vector3 start, Vector3 end, float totalTime) {
		float t = 0;
		do {
			go.transform.localScale = Vector3.Lerp (start, end, t / totalTime);
			yield return null;
			t += Time.deltaTime;
		} while (t < totalTime);
		go.transform.localScale = end;
		yield break;
	}
	
	public void SelectSpawn(float value){

		int x = Random.Range (0, count);
		if (spawns [x].gameObject.GetComponent<Spawn>().full == true) {
			SelectSpawn (value);
		}
		else{
		Vector3 position = spawns[x].position;
		Quaternion q = GeneratedTransform ();
		if (value < 60){
			clone = poolingSystem.InstantiateAPS (spot.name, position, q, spawns[x].gameObject) as GameObject;
			clone.GetComponent<Fruit>().parent=spawns[x].gameObject;
				DoScale(clone,gbnul,gbspotIncr,1.0f);
			}
		else if (value > 60 && value < 80){
			clone = poolingSystem.InstantiateAPS (nect.name, position, q, spawns[x].gameObject) as GameObject;
			clone.GetComponent<Fruit>().parent=spawns[x].gameObject;
				DoScale(clone,gbnul,gbnectIncr,1.0f);
			}
		else if (value > 80 && value < 90){
			clone = poolingSystem.InstantiateAPS (pinkly.name, position, q, spawns[x].gameObject) as GameObject;
			clone.GetComponent<Fruit>().parent=spawns[x].gameObject;
				DoScale(clone,gbnul,gbpinklyIncr,1.0f);
			}
		else {
			clone = poolingSystem.InstantiateAPS (coconut.name, position, q, spawns[x].gameObject) as GameObject;
			clone.GetComponent<Coconut>().parent=spawns[x].gameObject;
				DoScale(clone,gbnul,gbcocoIncr,1.0f);
		}
			spawns[x].gameObject.GetComponent<Spawn>().full = true ;
			full++;

			if(full==count-1 && !isfull){
				CancelInvoke ();
				StartCoroutine ("delaySpawn");
			}
			else if(full==count-1 && isfull){
				CancelInvoke();
			}
				
		if(clone.GetComponent<Fruit>())
			fruits.Add (clone);
		}
	}

	new public void StartSpawning(){
		Debug.Log (repeatrate);
		InvokeRepeating ("SpawnFruits", 3.0f, repeatrate);
		isfull=false;
	}

	new public IEnumerator delaySpawn(){
		while (full>=count-1) {
			isfull=true;
			yield return null;
		}
		yield return null;
		StartSpawning ();
	}

	new	public void SpawnFruits(){
		value = Random.Range (0f, 101f);
		SelectSpawn (value);
	}
	
	Quaternion GeneratedTransform(){
		float x,y,z;
		x = Random.Range(0,45);
		y = Random.Range(0,361);
		z = Random.Range(0,45);
		Vector3 rot = new Vector3 (x, y, z);
		return Quaternion.Euler (rot);
	}
	
	Vector3 GeneratedPosition() {	
		int x;
		x = Random.Range (0, spawns.Count);
		return spawns[x].position;
	}
}
