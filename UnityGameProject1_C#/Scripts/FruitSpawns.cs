using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitSpawns : MonoBehaviour {


	public GameObject burpy;
	private PoolingSystem poolingSystem;
	public GameObject spot;
	public GameObject coconut;
	public GameObject pinkly;
	public GameObject nect;
	public GameObject clone;
	public List<GameObject> fruits = new List<GameObject>();
	
	float maxZ = 25.0f;
	float minZ = -25.0f;
	float maxX = 50.0f;
	float minX = -50.0f;
	float value;
	
	// Use this for initialization
	void Start () {
		burpy = GameObject.Find ("Burpy");
		poolingSystem = PoolingSystem.Instance;
		//InvokeRepeating ("SpawnFruits", 1.0f, 0.5f);
	}

	public void removeFruit(GameObject go){
		fruits.Remove (go);
		PoolingSystem.DestroyAPS (go);
	}

	public void SpawnFruits(){
		value = Random.Range (0f, 101f);
		Vector3 position = GeneratedPosition ();
		Quaternion q = GeneratedTransform ();
		if(value < 60 )
			clone = poolingSystem.InstantiateAPS (spot.name, position, q) as GameObject;
		else if (value > 60 && value < 80 )
			clone = poolingSystem.InstantiateAPS (nect.name, position, q) as GameObject;
		else if( value > 80 && value < 95)
			clone = poolingSystem.InstantiateAPS (pinkly.name, position, q) as GameObject;
		else
			clone = poolingSystem.InstantiateAPS (coconut.name, position, q) as GameObject;
		while (clone.renderer.bounds.Intersects(burpy.renderer.bounds)) {
			position = GeneratedPosition(); // Add your logic to Calculate a new position here
			clone.transform.position = position;
		}
		if(clone.GetComponent<Spotmato>() || clone.GetComponent<Pinkly>() || clone.GetComponent<Nectaberry>())
		fruits.Add (clone);
	}

	Quaternion GeneratedTransform(){
		float x,y,z;
		x = Random.Range(0,361);
		y = Random.Range(0,361);
		z = Random.Range(0,361);
		return new Quaternion (x, y, z,0f);
	}

	Vector3 GeneratedPosition()
	{
		float x,y,z;
		x = Random.Range(minX,maxX);
		y = 2.5f;
		z = Random.Range(minZ,maxZ);
		return new Vector3(x,y,z);
	}
}
