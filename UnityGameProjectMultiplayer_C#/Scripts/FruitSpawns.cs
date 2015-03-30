using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawns : MonoBehaviour {


	public GameObject burpy;
	public PoolingSystem poolingSystem;
	public GameObject spot;
	public GameObject coconut;
	public GameObject pinkly;
	public GameObject nect;
	public GameObject clone;
	public List<GameObject> fruits = new List<GameObject>();
	public Renderer burpyrenderer;
	public List<Transform> spawn1 = new List<Transform> ();
	public List<Transform> spawn2 = new List<Transform> ();
	public List<Transform> spawn3 = new List<Transform> ();
	public List<Transform> spawn4 = new List<Transform> ();
	public Vector3 spotIncr,cocoIncr,pinklyIncr,nectIncr,nul;
	int spawn;
	public bool isfull;
	public int count;
	int max;
	public float value;
	public int full;
	public float repeatrate;

	void Start () {
		spotIncr = new Vector3 (1.0f, 1.0f, 1.0f);
		nectIncr = new Vector3 (1.5f, 1.5f,1.5f);
		pinklyIncr = new Vector3 (2.0f, 2.0f,2.0f);
		cocoIncr = new Vector3 (3.0f, 3.0f, 3.0f);
		nul = new Vector3 (0f, 0f, 0f);
		count = 0;
		spawn = 1;
		foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject))) {
			if (go.name == "S1") {
				spawn1.Add (go.transform);
				count++;
			} 

			else if (go.name == "S2"){
				spawn2.Add (go.transform);
				count++;
			}

			else if(go.name == "S3"){
				spawn3.Add (go.transform);
				count++;
			}

			else if(go.name == "S4"){
				spawn4.Add (go.transform);
				count++;
			}
		}

		burpy = GameObject.Find ("Burpy");
		burpyrenderer = burpy.GetComponentInChildren<Renderer> ();
		poolingSystem = PoolingSystem.Instance;
		max = count - 5;
	}

	void Awake(){

		burpy = GameObject.Find ("Burpy");
		burpyrenderer = burpy.GetComponentInChildren<Renderer> ();
		poolingSystem = PoolingSystem.Instance;

	}

	public void DoScale(GameObject go,Vector3 start, Vector3 end, float totalTime) {
		StartCoroutine(CR_DoScale(go,start, end, totalTime));
	}
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

	public void SelectSpawn(float value, List<Transform> spawns){

		if(full>=max && !isfull){
			CancelInvoke ();
			StartCoroutine (delaySpawn ());
		}
		else if(full>=max && isfull){
			CancelInvoke();
		}
		else{

 			int x = Random.Range (0, spawns.Count);


			if (spawns [x].gameObject.GetComponent<Spawn> ().full == true) {
				SpawnFruits();
			}
			
			else{
				Vector3 position = spawns[x].position;
				Quaternion q = GeneratedTransform ();
			
				if (value < 60){
					clone = poolingSystem.InstantiateAPS (spot.name, position, q, spawns[x].gameObject) as GameObject;
					clone.GetComponent<Fruit>().parent=spawns[x].gameObject;
					DoScale(clone,nul,spotIncr,1.0f);
				}
				else if (value > 60 && value < 80){
					clone = poolingSystem.InstantiateAPS (nect.name, position, q, spawns[x].gameObject) as GameObject;
					clone.GetComponent<Fruit>().parent=spawns[x].gameObject;
					DoScale(clone,nul,nectIncr,1.0f);
				}
				else if (value > 80 && value < 90){
					clone = poolingSystem.InstantiateAPS (pinkly.name, position, q, spawns[x].gameObject) as GameObject;
					clone.GetComponent<Fruit>().parent=spawns[x].gameObject;
					DoScale(clone,nul,pinklyIncr,1.0f);
				}
				else {
					clone = poolingSystem.InstantiateAPS (coconut.name, position, q, spawns[x].gameObject) as GameObject;
					clone.GetComponent<Coconut>().parent=spawns[x].gameObject;
					DoScale(clone,nul,cocoIncr,1.0f);
				}
				spawns[x].gameObject.GetComponent<Spawn>().full = true ;
				full++;
			
				if(clone.GetComponent<Fruit>())
					fruits.Add(clone);
			}
		}
	}

	public 	void removeFruit(GameObject go){
		fruits.Remove (go);
		PoolingSystem.DestroyAPS (go);
	}
	/// <summary>
	/// TODO: Fruits spawning by increasing scale??
	/// </summary>
	//public void SpawnFruits(){
	//	value = Random.Range (0f, 101f);
	//	Vector3 position = GeneratedPosition ();
	//	Quaternion q = GeneratedTransform ();
	//	if(value < 60 )
	//		clone = poolingSystem.InstantiateAPS (spot.name, position, q) as GameObject;
	//	else if (value > 60 && value < 80 )
	//		clone = poolingSystem.InstantiateAPS (nect.name, position, q) as GameObject;
	//	else if( value > 80 && value < 90)
	//		clone = poolingSystem.InstantiateAPS (pinkly.name, position, q) as GameObject;
	//	else
	//		clone = poolingSystem.InstantiateAPS (coconut.name, position, q) as GameObject;
	//	for (int i =0; i<fruits.Count; i++) {
	//		while (clone.renderer.bounds.Intersects(burpyrenderer.bounds)) {
	//			position = GeneratedPosition();	
	//			clone.transform.position = position;
	//		}
	//	}
	//	if(clone.GetComponent<Fruit>())
	//	fruits.Add (clone);
	//}

	public void StartSpawning(){
		InvokeRepeating ("SpawnFruits", 3.0f, repeatrate);
		isfull=false;

	}

	public IEnumerator delaySpawn(){
		while (full>=max-5) {
			isfull=true;
			yield return null;
		}
		yield return null;
		StartSpawning ();
	}

	public void SpawnFruits(){
		spawn++;
		if (spawn > 4) spawn = 1;
		value = Random.Range (0f, 101f);
		switch (spawn) {
		
		case 1:
			SelectSpawn (value,spawn1);
			break;
		case 2:
			SelectSpawn (value,spawn2);
			break;
		case 3:
			SelectSpawn (value,spawn3);
			break;
		case 4:
			SelectSpawn (value,spawn4);
			break;
		}
		//spawn++;
		
	}

	Quaternion GeneratedTransform(){
		float x,y,z;
		x = Random.Range(0,45);
		y = Random.Range(0,361);
		z = Random.Range(0,45);
		Vector3 rot = new Vector3 (x, y, z);
		return Quaternion.Euler (rot);
	}

	Vector3 GeneratedPosition(List<Transform> spawns)
	{	
		int x;
		x = Random.Range (0, spawns.Count);
		return spawns[x].position;
	}
}
