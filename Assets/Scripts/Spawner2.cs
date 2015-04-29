using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner2 : MonoBehaviour {
	
	
	public GameObject[] shapesToSpawn;
	public float delay = 5;
	public float sloMoDelay = 5;
	public bool spawning = false;
	private int shapeIndex = 0;
	private Vector3 spawnPos;
	private Transform tr;
	Shape tempShape;
	
	
	
	// Use this for initialization
	void Start () {
		
		Spawn ();
		tr = GetComponent<Transform> ();
		
		
	}
	
	
	public void Spawn() {
		
		if(spawning) {

			
			shapeIndex = Random.Range(0,shapesToSpawn.Length);
			
			spawnPos = tr.position;
			

			tempShape = Instantiate (shapesToSpawn[shapeIndex], spawnPos, tr.rotation);
			tempShape.originSpawner = this;

			
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
