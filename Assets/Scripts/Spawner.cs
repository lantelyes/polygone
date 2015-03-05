using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public GameObject[] shapesToSpawn;
	public float delay = 5;
	public bool spawning = false;
	private int shapeIndex = 0;
	private Vector3 spawnPos;



	// Use this for initialization
	void Start () {

		Invoke("Spawn", delay);
			
	}


	void Spawn() {

		if(spawning) {

			if(shapesToSpawn.Length-1 == shapeIndex)
				shapeIndex = 0;
			else 
				shapeIndex++;

			spawnPos = transform.position + new Vector3(Random.Range(-5,5),0,0);
			

			Instantiate (shapesToSpawn[shapeIndex], spawnPos, transform.rotation);
			Invoke("Spawn", delay);


	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
