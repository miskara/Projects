using UnityEngine;
using System.Collections;

public class ParticleSortingLayer : MonoBehaviour {

	public int sortingOrder = 4;

	void Start ()
	{
		this.renderer.sortingLayerName = "Hide";
		this.renderer.sortingOrder = sortingOrder;
	}
}

