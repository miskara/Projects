using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("AdvancedPoolingSystem/PoolingSystem")]


public sealed class PoolingSystem : MonoBehaviour {


	static Vector3 nul = new Vector3 (0f, 0f, 0f);
	GameObject system;
	[System.Serializable]
	public class PoolingItems
	{
		public GameObject prefab;
		public int amount;
	}
	
	public static PoolingSystem Instance;
	
	/// <summary>
	/// These fields will hold all the different types of assets you wish to pool.
	/// </summary>
	public PoolingItems[] poolingItems;
	public List<GameObject>[] pooledItems;
	
	/// <summary>
	/// The default pooling amount for each object type, in case the pooling amount is not mentioned or is 0.
	/// </summary>
	public int defaultPoolAmount = 20;
	
	/// <summary>
	/// Do you want the pool to expand in case more instances of pooled objects are required.
	/// </summary>
	public bool poolExpand = true;
	
	void Awake ()
	{
		//DontDestroyOnLoad(this);

		if (Instance) {
			Destroy (this.gameObject);
		}
		else {
			Instance = this;
		//	DontDestroyOnLoad(this);
				}
	}
	
	void Start () {
			
			pooledItems = new List<GameObject>[poolingItems.Length];

			for (int i=0; i<poolingItems.Length; i++) {

				pooledItems [i] = new List<GameObject> ();
				int poolingAmount;

				if (poolingItems [i].amount > 0) poolingAmount = poolingItems [i].amount;
				else poolingAmount = defaultPoolAmount;

				for (int j=0; j<poolingAmount; j++) {

					GameObject newItem = (GameObject)Instantiate (poolingItems [i].prefab);
					newItem.SetActive (false);
					pooledItems [i].Add (newItem);
					newItem.transform.parent = transform;
			}
		}

	}
	
	public static void DestroyAPS(GameObject myObject)
	{
		if (myObject.GetComponent<Rigidbody> () != null) {
			myObject.rigidbody.velocity = nul;
			myObject.rigidbody.angularVelocity = nul;
		}
		myObject.transform.position = nul;
		myObject.SetActive(false);

	}

	
	public GameObject InstantiateAPS (string itemType)
	{
		GameObject newObject = GetPooledItem(itemType);
		if(newObject != null) {
			newObject.SetActive(true);
			return newObject;
		}
		Debug.Log("Warning: Pool is out of objects.\nTry enabling 'Pool Expand' option.");
		return null;
	}
	
	public GameObject InstantiateAPS (string itemType, Vector3 itemPosition, Quaternion itemRotation)
	{
		GameObject newObject = GetPooledItem(itemType);
		if(newObject != null) {
			newObject.transform.position = itemPosition;
			newObject.transform.rotation = itemRotation;
			newObject.SetActive(true);

			if(newObject.GetComponent<Rigidbody>()!=null){
				newObject.rigidbody.velocity = nul;
				newObject.rigidbody.angularVelocity = nul;
				newObject.rigidbody.detectCollisions=false;
				newObject.rigidbody.useGravity=false;
			}

			return newObject;
		}
		Debug.Log("Warning: Pool is out of objects.\nTry enabling 'Pool Expand' option.");
		return null;
	}

	public GameObject InstantiateAPS (string itemType, Vector3 itemPosition)
	{
		GameObject newObject = GetPooledItem(itemType);
		if(newObject != null) {
			newObject.transform.position = itemPosition;
			newObject.SetActive(true);

			if(newObject.GetComponent<Rigidbody>()!=null){
				newObject.rigidbody.velocity = nul;
				newObject.rigidbody.angularVelocity = nul;
				newObject.rigidbody.detectCollisions=false;
				newObject.rigidbody.useGravity=false;
			
			}
			return newObject;
		}
		Debug.Log("Warning: Pool is out of objects.\nTry enabling 'Pool Expand' option.");
		return null;
	}
	
	public GameObject InstantiateAPS (string itemType, Vector3 itemPosition, Quaternion itemRotation, GameObject myParent)
	{
		
		GameObject newObject = GetPooledItem(itemType);
		if(newObject != null) {
			newObject.transform.position = itemPosition;
			newObject.transform.rotation = itemRotation;
			newObject.transform.parent = myParent.transform;
			newObject.SetActive(true);
			return newObject;
		}
		Debug.Log("Warning: Pool is out of objects.\nTry enabling 'Pool Expand' option.");
		return null;
	}
	
	public static void PlayEffect(GameObject particleEffect, int particlesAmount)
	{
		if(particleEffect.particleSystem)
		{
			particleEffect.particleSystem.Emit(particlesAmount);
		}
	}
	
	public static void PlaySound(GameObject soundSource)
	{
		if(soundSource.audio)
		{
			soundSource.audio.PlayOneShot(soundSource.audio.GetComponent<AudioSource>().clip);
		}
	}
	
	public GameObject GetPooledItem (string itemType)
	{
		for(int i=0; i<poolingItems.Length; i++)
		{
			if(poolingItems[i].prefab.name == itemType)
			{
				for(int j=0; j<pooledItems[i].Count; j++)
				{
					if(!pooledItems[i][j].activeInHierarchy)
					{
						return pooledItems[i][j];
					}
				}
				
				if(poolExpand)
				{
					GameObject newItem = (GameObject) Instantiate(poolingItems[i].prefab);
					newItem.SetActive(false);
					pooledItems[i].Add(newItem);
					newItem.transform.parent = transform;
					return newItem;
				}
				
				break;
			}
		}
		
		return null;
	}


	public void DeactiveAll(){
		for (int i=0; i<poolingItems.Length; i++) {
			for (int j=0; j<pooledItems[i].Count; j++) {
				pooledItems [i] [j].SetActive (false);
				pooledItems[i][j].transform.position=nul;
			}
		}
	}


	
}

public static class PoolingSystemExtensions
{
	public static void DestroyAPS(this GameObject myobject)
	{
		PoolingSystem.DestroyAPS(myobject);
	}
	
	public static void PlayEffect(this GameObject particleEffect, int particlesAmount)
	{
		PoolingSystem.PlayEffect(particleEffect, particlesAmount);
	}
	
	public static void PlaySound(this GameObject soundSource)
	{
		PoolingSystem.PlaySound(soundSource);
	}
}