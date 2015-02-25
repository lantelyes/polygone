using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public GameObject[] shapesToSpawn;
	public int delay = 5;
	public bool spawning = false;
	private int shapeIndex = 0;



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
			

			Instantiate (shapesToSpawn[shapeIndex], transform.position, transform.rotation);
			Invoke("Spawn", delay);


	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
